#include <Wire.h>               //심박센서 라이브러리
#include <Adafruit_MLX90614.h>  //온도센서 라이브러리
#include <LiquidCrystal_I2C.h>  //LCD라이브러리
#include <AVR/interrupt.h>
#include <MsTimer2.h>           //인터럽트 타이머2 라이브러리
#include <String.h>
#include "ESP8266.h" //와이파이 라이브러리

#define SSID "heartbeat"
#define PASSWORD "12345678"
#define HOST_NAME   "192.168.0.170"  
#define HOST_PORT   (80)

#define SERVER_HOST_NAME   "192.168.0.170"
#define DEVICE_ID   "1"

#define WIFI_STANDBY_SECOND 20
#define WIFI_SEND_WAIT_SECOND 5

#define WIFI_TIMEOUT_CREATE_TCP  4000   //(unit: ms)  //연결 안될때 4초까지 기다림
#define WIFI_TIMEOUT_SEND        2000   //(unit: ms)
#define WIFI_TIMEOUT_CLOSE_TCP    500   //(unit: ms)

#define WIFI_WAIT      0
#define CREATE_TCP     1
#define SERVER_UPDATE  2

Adafruit_MLX90614 mlx = Adafruit_MLX90614();//온도센서 mlx90614 라이브러리 생성
LiquidCrystal_I2C lcd(0x27, 16, 2);
ESP8266 wifi(Serial1);

const bool bLCD_Time_Blink = true;     //LCD Timer Blink
const bool bLCD_Discon_Blink = true;   //LCD DisConnect Blink

// Sensor Data ////////////////////////////////////////////
unsigned char cHeart; //현재심박수

char chArrTemp[6]; // 온도 값
char chArrHeart[4];

float fTemp;
const int fTempFilter_Max = 30; //온도 데이터 평균 수치
float fTemp2[fTempFilter_Max];

// Timer2 Interrupt  ///////////////////////////////////////
// Timer_Calc
volatile unsigned int chHour = 0;
volatile unsigned int chMinute = 0;
volatile unsigned int chSecond = 0;
volatile unsigned int ch100ms = 0;

// Wifi Scheduler
volatile bool bStart = false;
volatile unsigned int nStartTime = 0;
volatile unsigned int nSendTime = 0;
volatile unsigned int nresult = 0;
volatile unsigned char cMode = WIFI_WAIT;
volatile bool bInterrupt = false;
volatile bool bSendflag = false;

// Wifi ////////////////////////////////////////////////////
char *time_get = (uint8_t*)malloc(256);
bool isConnected = false;
bool isGetTime = false;

bool bCreateTCP = false;
bool bServerUpdate = false;

long maxCreate = 0;
long maxSend = 0;
long maxRelease = 0;
long maxTime = 0;

// LCD User Character
byte byArrDotChar[8] = {
        B00110,
        B01001,
        B01001,
        B00110,
        B00000,
        B00000,
        B00000,
        B00000
};

//Interrupt/////////////////
//volatile unsigned int timeCurrent = 0;
//volatile unsigned int timeSet = 1000;


void setup()
{
  // put your setup code here, to run once:
    Serial.begin(115200);
    
    while (!Serial)
    {
        ; // wait for serial port to connect. Needed for native USB
    }
    Wifi_Setting();
    
    delay(30);
    lcd.init();
    lcd.backlight();
    lcd.createChar(1, byArrDotChar);
    lcd.clear();
    
    //심박 센서
    Wire.begin();
    
    //온도 센서
    mlx.begin();  
    
    MsTimer2::set(100, Timer_Calc);
    MsTimer2::start();
    
    Serial.println(">>START<<");
}


void Wifi_Setting()
{
    Serial1.begin(115200);
    //Serial1.setTimeout(500);

    while (!Serial1)
    {
        ; // wait for serial port to connect.
    }

    Serial.print("setup begin\r\n");

    if (wifi.restart())
    {
        Serial.print("Wifi Module Restart ok\r\n");
    }
    else
    {
        Serial.print("Wifi Module Restart err\r\n");
    }
    
    Serial.print("FW Version:");
    Serial.println(wifi.getVersion().c_str());
    
    if (wifi.setOprToStationSoftAP())   // Set Station Mode
    {
        Serial.print("to station + softap ok\r\n");
    }
    else
    {
        Serial.print("to station + softap err\r\n");
    }

    if (wifi.joinAP(SSID, PASSWORD))   // Join in AP.
    {
        Serial.print("Join AP success\r\n");

        Serial.print("IP: ");
        Serial.println( wifi.getLocalIP().c_str());
    }
    else
    {
        Serial.print("Join AP failure\r\n");
    }
    
    if (wifi.disableMUX())   //Disable IP MUX(single connection mode). 
    {  
        Serial.print("single ok\r\n");
    }
    else
    {
        Serial.print("single err\r\n");
    }

    if ( Wifi_createTCP(HOST_NAME, HOST_PORT) == true )  //Create TCP connection in single mode. 
    //if (Wifi_createTCP2(HOST_NAME, HOST_PORT))
    {
        Serial.print("create tcp ok\r\n");
        isConnected = true;   //왜 true였지?
        Get_Server_Time();

        if ( Wifi_Close_TCP() == true )
        {
            Serial.print("Close tcp ok1\r\n");
        }
        else
        {
            Serial.print("Close tcp err1\r\n");
        }
    }
    else
    {
        Serial.print("Create tcp err\r\n");
    }
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////  WIFI Library START  ////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


void Wifi_Buffer_Clear()
{
  //void ESP8266::rx_empty(void) 
    while(Serial1.available())
    {
        Serial1.read();
    }
}


String Wifi_RecvString(String target, uint32_t timeout)
{
  //String ESP8266::recvString(String target, uint32_t timeout)
    String data;
    char a;
    unsigned long start = millis();

    //Serial.println("RecvString1 START");
    
    while (millis() - start < timeout)
    {
        while(Serial1.available())
        {
            a = Serial1.read();
            //Serial.print(a);
            if(a == '\0') continue;
            data += a;
        }
        if (data.indexOf(target) != -1)
        {
            break;
        }   
    }
    //Serial.print("\n");
    //Serial.println("RecvString1 END");
    return data;
}


String Wifi_RecvString(String target1, String target2, String target3, uint32_t timeout)
{
  //String ESP8266::recvString(String target1, String target2, String target3, uint32_t timeout)
    String data;
    char a;
    unsigned long start = millis();

    //Serial.println("RecvString3 START");
    
    while (millis() - start < timeout)
    {
        while(Serial1.available())
        {
            a = Serial1.read();
            //Serial.print(a);
            if(a == '\0') continue;
            data += a;
        }
        if (data.indexOf(target1) != -1)
        {
            break;
        }
        else if (data.indexOf(target2) != -1)
        {
            break;
        }
        else if (data.indexOf(target3) != -1)
        {
            break;
        }
    }
    //Serial.print("\n");
    //Serial.println("RecvString3 END");
    return data;
}


bool Wifi_recvFind(String target, uint32_t timeout)
{
  //bool ESP8266::recvFind(String target, uint32_t timeout)
    String data_tmp;
    data_tmp = Wifi_RecvString(target, timeout);
    if (data_tmp.indexOf(target) != -1)    return true;
    return false;
}


bool Wifi_createTCP(String addr, uint32_t port)
{
  //bool ESP8266::createTCP(String addr, uint32_t port)
  //bool ESP8266::sATCIPSTARTSingle(String type, String addr, uint32_t port)
    String data;
    
    Wifi_Buffer_Clear();

    data = "AT+CIPSTART=";
    
    data += "\"";
    data += "TCP";
    data += "\",";

    data += "\"";
    data += addr;
    data += "\",";

    data += port;

    Serial1.println(data);

    data = Wifi_RecvString("OK", "ERROR", "ALREADY CONNECT", WIFI_TIMEOUT_CREATE_TCP);
    if (data.indexOf("OK") != -1 || data.indexOf("ALREADY CONNECT") != -1)
    {
        return true;
    }
    return false;
}


bool Wifi_createTCP2(String addr, uint32_t port)
{
    return wifi.createTCP(addr, port);
}

bool Wifi_Close_TCP()
{
  //bool ESP8266::releaseTCP(void)
  //bool ESP8266::eATCIPCLOSESingle(void)
    
    String data;
    
    Wifi_Buffer_Clear();
    
    data = "AT+CIPCLOSE";

    Serial1.println(data);
    
    data = Wifi_RecvString("OK", WIFI_TIMEOUT_CLOSE_TCP);
    //data = Wifi_RecvString("OK", "OK", "OK", 200);
    if (data.indexOf("OK") != -1)
    {
        return true;
    }
    return false;
}


bool Wifi_Close_TCP2()
{

    return  wifi.releaseTCP();
}


bool Wifi_Send(String buffer, uint32_t timeout)
{
  //bool ESP8266::send(const uint8_t *buffer, uint32_t len)
  //bool ESP8266::sATCIPSENDSingle(const uint8_t *buffer, uint32_t len)
    Wifi_Buffer_Clear();
    
    Serial1.print("AT+CIPSEND=");
    Serial1.println( buffer.length() );
    
    if ( Wifi_recvFind(">", timeout) )
    //if ( Serial1.find(">", 5000) )
    //if ( Serial1.find(">") )
    {
        Wifi_Buffer_Clear();
        
        /////////////////////////////////////
//        for (uint32_t i = 0; i < buffer.length(); i++)
//        {
//            Serial1.write( buffer.charAt(i) );
//        }
        /////////////////////////////////////
        Serial1.print( buffer );

        return Wifi_recvFind("SEND OK", timeout);
    }
    return false;
}


bool Wifi_Send2(const uint8_t *buffer, uint32_t len, uint32_t timeout)
{
    Wifi_Buffer_Clear();
    
    Serial1.print("AT+CIPSEND=");
    Serial1.println( len );

    if ( Wifi_recvFind(">", timeout) )
    //if ( Serial1.find(">", 5000) )
    //if ( Serial1.find(">") )
    {
        Wifi_Buffer_Clear();
        
        /////////////////////////////////////
        for (uint32_t i = 0; i < len; i++)
        {
            Serial1.write( buffer[i] );
        }
        /////////////////////////////////////
        return Wifi_recvFind("SEND OK", timeout);
    }
    return false;
}


int Wifi_Recv(uint8_t *buffer, uint32_t buffer_size, uint32_t timeout)
{
  //uint32_t ESP8266::recv(uint8_t *buffer, uint32_t buffer_size, uint32_t timeout)
  //uint32_t ESP8266::recvPkg(uint8_t *buffer, uint32_t buffer_size, uint32_t *data_len, uint32_t timeout, uint8_t *coming_mux_id)
    String data;
    char a;
    int32_t index_PIPDcomma = -1;
    int32_t index_colon = -1; /* : */
    int32_t len = -1;
    bool has_data = false;
    uint32_t ret;
    unsigned long start;
    uint32_t i;
    
    if (buffer == NULL)
    {
        return 0;
    }
    
    start = millis();
    while (millis() - start < timeout)
    {
        if( Serial1.available() )
        {
            a = Serial1.read();
            data += a;
        }
        
        index_PIPDcomma = data.indexOf("+IPD,");
        if (index_PIPDcomma != -1)
        {
            index_colon = data.indexOf(':', index_PIPDcomma + 5);
            if (index_colon != -1)
            {
                len = data.substring(index_PIPDcomma + 5, index_colon).toInt();
                if (len <= 0)   return 0;

                has_data = true;
                break;
            }
        }
    }
    
    if (has_data)
    {
        i = 0;
        ret = len > buffer_size ? buffer_size : len;
        start = millis();
        while (millis() - start < 3000)
        {
            while( Serial1.available() > 0 && i < ret)
            {
                a = Serial1.read();
                buffer[i++] = a;
            }
            if (i == ret)
            {
                Wifi_Buffer_Clear();
                return ret;
            }
        }
    }
    return 0;
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////  WIFI Library END  ////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


void Timer_Calc()  //시간 계산
{
    ch100ms++;
    
    if( ch100ms % 10 == 0 )  //1Sec
    {
        chSecond++;
        
        if ( chSecond / 60 ) chMinute++;
        if ( chMinute / 60 ) chHour++;
        
        chSecond %= 60;
        chMinute %= 60;
        chHour %= 24;

        if ( bStart == false )    //WIFI STANDBY TIME WAIT
        {
            if ( nStartTime >= 86400 - WIFI_STANDBY_SECOND )    //WIFI_STANDBY_SECOND + 서버 시작 시간 > 24시 초과시
            {
                if ( chHour*3600 + chMinute*60 + chSecond >= nStartTime + WIFI_STANDBY_SECOND - 86400 )
                {
                    bStart = true;
                    bCreateTCP = true;
                }
            }
            else
            {
                if ( chHour*3600 + chMinute*60 + chSecond >= nStartTime + WIFI_STANDBY_SECOND )
                {
                    bStart = true;
                    bCreateTCP = true;
                }                            
            }

        }
    }

    if ( bStart == true )
    {
        if      ( bSendflag == true )
        {
            bSendflag = false;
            nSendTime  = chHour*3600 + chMinute*60 + chSecond;  
        }

        if  (( isConnected == true ) || ( chSecond % 10 == 0 ))
        {
            if      ( bCreateTCP == true )
            {
                cMode = CREATE_TCP;
            }
            else if ( bServerUpdate == true )
            {   //WIFI SEND TIME WAIT
                if ( nSendTime >= 86400 - WIFI_SEND_WAIT_SECOND )    //WIFI Send Wait Time + 마지막 전송 시간 > 24시 초과시
                {
                    if ( chHour*3600 + chMinute*60 + chSecond >= nSendTime + WIFI_SEND_WAIT_SECOND - 86400 )
                    {
                        cMode = SERVER_UPDATE;
                    }
                    else
                    {
                        cMode = WIFI_WAIT;
                    }
                }
                else
                {
                    if ( chHour*3600 + chMinute*60 + chSecond >= nSendTime + WIFI_SEND_WAIT_SECOND )
                    {
                        cMode = SERVER_UPDATE;
                    }
                    else
                    {
                        cMode = WIFI_WAIT;
                    }
                }
            }
            else
            {
                cMode = WIFI_WAIT;
            }
        }
    }

    ch100ms %= 50;  //5Sec
    bInterrupt = true;
}


void Time_Update()
{
    //서버 시간 업데이트
    char temp[5];
    String sCommand = time_get;
    
    // string -> int
    sCommand.substring(0, 2).toCharArray(temp, 3);
    chHour = atoi(temp);
    
    sCommand.substring(2, 4).toCharArray(temp, 3);
    chMinute = atoi(temp);
    
    sCommand.substring(4, 6).toCharArray(temp, 3);
    chSecond = atoi(temp);
    
    nStartTime = chHour*3600 + chMinute*60 + chSecond;
    
    Serial.println(chHour);
    Serial.println(chMinute);
    Serial.println(chSecond);
    sCommand = "";
}


bool Get_Server_Time()
{
    String strData;

    strData = "GET /time.php HTTP/1.1\r\n";
    
    strData += "Host: ";
    strData += SERVER_HOST_NAME;
    strData += "\r\n";
    
    strData += "Connection: close\r\n\r\n";

    if ( Wifi_Send( strData, WIFI_TIMEOUT_SEND ) == true )
    {
        uint8_t buffer[256] = {0};
        uint32_t len = Wifi_Recv(buffer, sizeof(buffer), 10000);

        memset(time_get,0,sizeof(time_get));
        if ( len > 0 ) 
        {
            int j=0;
            char time1[3];
    
            bool bTimeStart = false;
            bool bTimeEnd = false;

            Serial.print("Received:[");
            for(int i = 0; i < len; i++) 
            {
                Serial.print((char)buffer[i]);
                //Serial.print((char)buffer[i]);
                if((char)buffer[i] == '>')    bTimeEnd = true;
                
                if( bTimeStart == true && bTimeEnd == false )
                {
                  sprintf(time1, "%c", (char)buffer[i]);
                  strcat(time_get, time1);
                }
                
                if((char)buffer[i] == '<')    bTimeStart = true;
                //Serial.println("start3");
            }
            Serial.println("]");

            if( bTimeStart == true && bTimeEnd == true )
            {
                isGetTime = true;//필요할까
                Time_Update();
            }
        }
    }

    return isGetTime;
}


bool Get_Server_Time3()
{
    char *hello = (uint8_t*)malloc(256);     
    strcat(hello, "GET /time.php HTTP/1.1\r\nHost: 192.168.0.170\r\nConnection: close\r\n\r\n");

    if ( wifi.send( (const uint8_t*)hello, strlen(hello) ) == true )
    {
        uint8_t buffer[256] = {0};
        uint32_t len = wifi.recv(buffer, sizeof(buffer), 10000);

        memset(time_get,0,sizeof(time_get));
        if ( len > 0 ) 
        {
            int j=0;
            char time1[3];
    
            bool bTimeStart = false;
            bool bTimeEnd = false;

            Serial.print("Received:[");
            for(int i = 0; i < len; i++) 
            {
                Serial.print((char)buffer[i]);
                //Serial.print((char)buffer[i]);
                if((char)buffer[i] == '>')    bTimeEnd = true;
                
                if( bTimeStart == true && bTimeEnd == false )
                {
                  sprintf(time1, "%c", (char)buffer[i]);
                  strcat(time_get, time1);
                }
                
                if((char)buffer[i] == '<')    bTimeStart = true;
                //Serial.println("start3");
            }
            Serial.println("]");

            if( bTimeStart == true && bTimeEnd == true )
            {
                isGetTime = true;//필요할까
                Time_Update();
            }
        }
    }
    
    return isGetTime;
}


bool Set_Server_Update()
{
    String strData;
    
    strData = "GET /receive_data.php?";
    
    strData += "emp_id=";
    strData += DEVICE_ID;
    
    strData += "&heart_rate=";
    strData += chArrHeart;
    
    strData += "&tem_rate=";
    strData += chArrTemp;
    
    strData += " HTTP/1.1\r\nHost: ";
    strData += SERVER_HOST_NAME;
    strData += "\r\n";
    
    strData += "Connection: close\r\n\r\n";
    
    //Serial.println(strData);
    return  Wifi_Send( strData , WIFI_TIMEOUT_SEND );
}


void TempUpdate()
{
    float fTemp_Buf = 0;
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    // 1번, fTemp 2개 더한 값을 나누기
    //fTemp = ( fTemp + mlx.readObjectTempC() ) / 2;    //측정 물체 온도 측정 범위: -70ºC ~ 380ºC
    ////fTemp = mlx.readAmbientTempC(); //센서 자체 온도 측정 범위: -40ºC ~ 125ºC // 센서 자체 온도 API로 현재는 불필요
    //////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    // 2번, fTemp2를 fTempFilter_Max 선언된 갯수만큼 총합의 평균 값
    memmove(fTemp2+1, fTemp2, sizeof(fTemp2) - sizeof(float));
    fTemp2[0] = mlx.readObjectTempC();

    for(int i = 0; i < sizeof(fTemp2)/sizeof(float); i++)
    {
        fTemp_Buf += fTemp2[i];
    }
    fTemp = fTemp_Buf / (sizeof(fTemp2)/sizeof(float));
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    
    if      ( fTemp < -70 )  fTemp = -70;
    else if ( fTemp > 380 )  fTemp = 380;
    
    if      ( fTemp >= 100 )  dtostrf(fTemp, 5, 1, chArrTemp);  // XXX.XºC
    else if ( fTemp >= 10  )  dtostrf(fTemp+1, 5, 2, chArrTemp);  // XX.XXºC
    else if ( fTemp >= 0   )  dtostrf(fTemp, 5, 2, chArrTemp);  // XX.XXºC
    else if ( fTemp <= -10 )  dtostrf(fTemp, 5, 1, chArrTemp);  // -XX.XºC
    else if ( fTemp < -0   )  dtostrf(fTemp, 5, 2, chArrTemp);  // -X.XXºC
}


void HeartbeatUpdate()
{
    Wire.flush();
    Wire.requestFrom(0xA0 >> 1, 1);// request 1 bytes from slave device
    
    cHeart = 0;
    //memset(chArrHeart, 0, sizeof(chArrHeart));
    
    while(Wire.available())        // slave may send less than requested
    {
        cHeart = Wire.read();     // receive heart rate value (a byte)
    }

    if      ( cHeart < 0 )    cHeart = 0;
    else if ( cHeart > 255 )  cHeart = 255;

    sprintf(chArrHeart, "%d", cHeart);
}


void LCD_Sensor()
{
    char chArrSensorBuf[20];
    //memset(chArrSensorBuf,0,sizeof(chArrSensorBuf));
    sprintf(chArrSensorBuf , "%3sBPM / %5s", chArrHeart, chArrTemp );
    
    lcd.setCursor(0,0);
    lcd.print(chArrSensorBuf);
    lcd.write(byte(1));
    lcd.print("C");
}


void LCD_Blink_On()
{
    char chArrLCDBuf[20];
    
    LCD_Sensor();
    
    lcd.setCursor(0,1);
    if (isConnected == true)    sprintf(chArrLCDBuf , "Connect %02d:%02d:%02d", chHour, chMinute, chSecond);
    else                        sprintf(chArrLCDBuf , " DisCon %02d:%02d:%02d", chHour, chMinute, chSecond);
    
    lcd.print(chArrLCDBuf);
}


void LCD_Blink_Off()
{
    char chArrLCDBuf[20];
    
    LCD_Sensor();
    
    lcd.setCursor(0,1);
    if (isConnected == true)
    {
        if ( bLCD_Time_Blink == true )
        {
            sprintf(chArrLCDBuf , "Connect %02d %02d %02d", chHour, chMinute, chSecond);
        }
        else
        {
            sprintf(chArrLCDBuf , "Connect %02d:%02d:%02d", chHour, chMinute, chSecond);
        }
    }
    else
    {
        if (( bLCD_Time_Blink == true ) && ( bLCD_Discon_Blink == true ))
        {
            sprintf(chArrLCDBuf , "        %02d %02d %02d", chHour, chMinute, chSecond);
        }
        else if (( bLCD_Time_Blink == true ) && ( bLCD_Discon_Blink == false ))
        {
            sprintf(chArrLCDBuf , " DisCon %02d %02d %02d", chHour, chMinute, chSecond);
        }
        else if (( bLCD_Time_Blink == false ) && ( bLCD_Discon_Blink == true ))
        {
            sprintf(chArrLCDBuf , "        %02d:%02d:%02d", chHour, chMinute, chSecond);
        }
        else if (( bLCD_Time_Blink == false ) && ( bLCD_Discon_Blink == false ))
        {
            sprintf(chArrLCDBuf , " DisCon %02d:%02d:%02d", chHour, chMinute, chSecond);
        }
    }
    
    lcd.print(chArrLCDBuf);
}


void loop()
{
  // put your main code here, to run repeatedly:
    long start;
    long start_release;
    long end;
    
    TempUpdate();
    
    if ( bInterrupt == true )
    {
        ///////////////////////////////////// LCD /////////////////////////////////////
        if ( ch100ms % 10 < 5 )    //0 ~ 0.5초 미만 사이에서 0.1초 마다 진입
        {
            LCD_Blink_On();
        }
        else                       //0.5 ~ 1초 미만 사이에서 0.1초 마다 진입
        {
            LCD_Blink_Off();
        }
        ///////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////// HeartBeat //////////////////////////////////
        if ( ch100ms % 5 == 0 )   //0.5초 마다 진입
        {
            HeartbeatUpdate();
        }
        ///////////////////////////////////////////////////////////////////////////////
        if ( cMode == CREATE_TCP )
        {
            bCreateTCP = false;
            start = millis();
            if ( Wifi_createTCP(HOST_NAME, HOST_PORT) == true )
            {
                Serial.print("create tcp ok\r\n");
                bCreateTCP = false;
                //isConnected = true;
                if ( isGetTime == false )
                {
                      start = millis();
                      if ( Get_Server_Time() == true )
                      {
                          isConnected = true; 
                      }
                      bSendflag = true;
                      bServerUpdate = true;
                      end = millis() - start;
                      Serial.print("Get_Server_Time Time: ");
                      Serial.println(end);
                      if (maxTime < end)  maxTime = end;
                }
                else    bServerUpdate = true;
            }
            else
            {
                Serial.print("create tcp err\r\n");
                isGetTime = false;
                bCreateTCP = true;
                bServerUpdate = false;
                isConnected = false;
            }
            
            end = millis() - start;
            Serial.print("Wifi_createTCP Time: ");
            Serial.println(end);
            if (maxCreate < end)  maxCreate = end;
        }
        
        if ( cMode == SERVER_UPDATE )
        {
            bServerUpdate = false;
            start = millis();
            if ( Set_Server_Update() == true )
            {
                bSendflag = true;
                isConnected = true;
                start_release = millis();
                Serial.print("OK, SEND!!!\n");
                if ( Wifi_Close_TCP() == true )     Serial.print("release tcp ok3\r\n");
                else                                Serial.print("release tcp err3\r\n");
                end = millis() - start_release;
                Serial.print("Wifi_Close_TCP Time: ");
                Serial.println(end);
                if (maxRelease < end)  maxRelease = end;
                
                Serial.print("Max Create Time: ");
                Serial.print(maxCreate);
                
                Serial.print(", Max Send Time: ");
                Serial.print(maxSend);
                
                Serial.print(", Max Release Time: ");
                Serial.print(maxRelease);
                
                Serial.print(", Max Get Server Time: ");
                Serial.println(maxTime);
            }
            else
            {
                bSendflag = true;
                isConnected = false;
                Serial.print("FAIL, SEND!!!\n");
            }
            end = millis() - start;
            Serial.print("Set_Server_Update Time: ");
            Serial.println(end);
            
            if (maxSend < end)  maxSend = end;
            bCreateTCP = true;
        }
        bInterrupt = false;
    }
}
