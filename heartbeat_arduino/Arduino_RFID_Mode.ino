
#include <MsTimer2.h>           //타이머인터럽트 라이브러리
#include <LiquidCrystal_I2C.h>  //LCD 라이브러리


LiquidCrystal_I2C lcd(0x27, 16, 2);


//시간 받아오는 함수
unsigned char c;
unsigned char c_old;


//LCD 입출력 표시
String lcdState = "";

//connection state
volatile bool bTriger = false;
volatile bool bSend = false;
volatile bool bRecv = false;

// 버튼 상태를 위한 함수
int bt = 2;
int ledg = 13;
int ledr = 12;
volatile byte state = LOW;
bool enter = false;


char buf; /*통신을 읽는 버퍼*/
String  sCommand = "";/*시리얼 수신 데이터 저장*/
volatile int hh = 0;
volatile int mm = 0;
volatile int ss = 0;

volatile int vn500ms = 0;

bool bConndisplay = false;
bool bTimeRecv = false;
// 아두이노->윈폼 발신
String sSend = "send";

// 윈폼 -> 아두이노 수신체크
String sCheck = "";

void setup() {
  Serial.begin(9600);
  lcd.init();
  lcd.backlight();

  pinMode(ledg, OUTPUT);
  pinMode(ledr, OUTPUT);
  pinMode(bt, INPUT_PULLUP);
  //sw를 인풋_풀업으로 하여 스위치가 열려있다면 HIGH
  //스위치가 닫혀있다면 LOW값을 반환

  //timer함수 호출하는 타이머인터럽트
  MsTimer2::set(500, timer);
  MsTimer2::start();


  //LED, LCD 인터럽트로 돌아가도록 함
  attachInterrupt(digitalPinToInterrupt(bt), button, CHANGE);


  lcd.clear();
}

////////////////////////////////////////////////////////////
void timer() {            //시간 계산

  vn500ms++;
  if( vn500ms % 2 == 0 )
  {
      vn500ms = 0;
      ss++;
      if (ss >= 60) {
        mm++;
        ss = 0;
      }
      if (mm >= 60) {
        mm = 0;
        hh++;
      }
      if (hh >= 24) {
        hh = 0;
      }
      if      ( ss % 5 == 0 )  bSend = true;
      else if ( ss % 5 != 0 )  bRecv = true;
  }
}


void serialEvent()      //시리얼 통신
{
    sCommand = Serial.readString();
    //Serial.print(Serial.readString());
    
    if (sCommand.indexOf("TIME_SET") != -1) // CONNECT or ALREADY CONNECTED
    {
        bTimeRecv = true;
        bTriger = true;
        //Serial.println(sCommand);
        char temp[5];
      
        // string -> int
        sCommand.substring(9, 11).toCharArray(temp, 3);
        hh = atoi(temp);
        //Serial.print(hh);
        //Serial.print('/');
      
        sCommand.substring(11, 13).toCharArray(temp, 3);
        mm = atoi(temp);
        //Serial.print(mm);
        //Serial.print('/');
      
        sCommand.substring(13, 15).toCharArray(temp, 3);
        ss = atoi(temp);
        //Serial.print(ss);
        //Serial.print('/');
        //Serial.print(sCommand);
        Serial.println(temp);

        sCommand.substring(15, 16).toCharArray(temp, 2);
        if(atoi(temp)==1){
            state = HIGH;
            enter = true;
            lcdState = "Punch IN :      ";
        }

        else{
            state = LOW;
            enter = false;
            lcdState = "Punch OUT:      ";
        }
        //enter,state,lcd
                Serial.println(temp);
        
        sCommand = "";        //시리얼 수신값 초기화
        //    Serial.print(buf);
        //sCommand = 7;


    }
    else if (sCommand.indexOf("RECV") != -1)
    {
        bTriger = true;
    }
}

///////////////////////////////////////////////////////////
//윈폼 -> 아두이노 즉시응답
//약 1초간 딜레이가 생김 ... 왜????
//시리얼 받기 확인 됨 -> connection 넘어감
void serialCheck() 
{
    if( bTriger == false )
    {
        bConndisplay = false;
        bTimeRecv = false;
    }
    else
    {
        bConndisplay = true;
    }
}


//입출력 버튼 전환
//  입 출문 각 led구현
//  LCD구현
void button()
{

    if (digitalRead(bt) == HIGH)
    {
        delay(20);
        if ( enter == false )
        {
            //digitalWrite(led, HIGH);
            Serial.println("Mode1");
            state = HIGH;
            enter = true;
            lcdState = "Punch IN :      ";
            //Serial.println("where3");
        }
        else
        {
            //digitalWrite(led, HIGH);
            Serial.println("Mode0");
            state = LOW;
            enter = false;
            lcdState = "Punch OUT:      ";
            //Serial.println("where4");
        }
    }
}


void timerBuffer()
{
    //시간LCD 표시
    char timeBuf[30];
    sprintf(timeBuf , "%02d:%02d:%02d", hh, mm, ss);
    //sCommand = timeBuf;
    lcd.setCursor(0,1 );
    lcd.print("        ");
    lcd.setCursor(8, 1);
    lcd.print(timeBuf);
}

void ledCNT()
{
    //출퇴근 LED,LCD상태 표시
    digitalWrite(ledg, state);
    digitalWrite(ledr, !state);
    lcd.setCursor(0, 0);
    lcd.print(lcdState);
    //delay(3000);
    //Serial.println("where5");
}


//이름 및 부서명


//공지사항, 재고확인알림




void loop()
{
    if( bSend == true )
    {
        bTriger = false;
        bSend = false;
        if ( bTimeRecv == false )
        {
            Serial.println("TIME_REQUEST");
        }
        else
        {
            Serial.println("SEND");  
        }
    }
    if ( bRecv == true )
    {
        bRecv = false;
        serialCheck(); 
    }

    if(bConndisplay == true)
    {
        timerBuffer();
        ledCNT();  
    }
    else
    {
      //연결 X 깜빡깜빡
        if( vn500ms % 2 == 0 )
        {
            lcd.setCursor(0, 0);
            lcd.print("-Not  Connected-");
            lcd.setCursor(0, 1);
            lcd.print("                ");
            
            digitalWrite(ledg, HIGH);
            digitalWrite(ledr, LOW);
            //Serial.println("where6");
        }
        else
        {
            lcd.setCursor(0, 0);
            lcd.print("                ");
            lcd.setCursor(0, 1);
            lcd.print("XXXXXXXXXXXXXXXX");
            
            digitalWrite(ledg, LOW);
            digitalWrite(ledr, HIGH);
            //Serial.println("where7");
        }
    }
}
