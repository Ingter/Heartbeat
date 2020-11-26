using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
    public partial class RFID : Form
    {
        //아두이노 
        private string[] portNames;
        private string[] portNames_Old;
        private int portIndex = 0;
        private bool bConnect = false;
        public string inout = "1";

        //RFID
        private int g_rHandle, g_retCode;
        private byte g_Sec, g_SID;
        private byte[] g_pKey = new byte[6];
        private bool g_isConnected = false;

        public string strConn = "Server=192.168.0.173;" +
                               "Database=heartbeat;" +
                               "Uid=test;" +
                               "Pwd=1234;" +
                               "charset=utf8;";
        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;
        string RFID_STATE = "";

        public RFID()
        {
            InitializeComponent();
            this.Load += TrayIcon_Load;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        }

        public void TrayIcon_Load(object sender, EventArgs e)

        {
            Tray_icon.ContextMenuStrip = Context_TrayIcon;

        }



        private void button1_Click(object sender, EventArgs e)
        {
            //Button_start();
            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = ($"select RFID_STATE from RFID_STATE");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                RFID_STATE = rdr["RFID_STATE"].ToString();

            }
            rdr.Close();

            if (RFID_STATE == "0")
            {
                RFID_STATE = "1";
                cmd.CommandText = ($"update RFID_STATE set RFID_STATE ='{RFID_STATE}'");
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            timer1.Interval = 700;
            timer1.Start();
            label2.Text = "RFID ON";
        }
        public void Button_start()
        {
            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = ($"select RFID_STATE from RFID_STATE");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                RFID_STATE = rdr["RFID_STATE"].ToString();

            }
            rdr.Close();

            if (RFID_STATE == "0")
            {
                RFID_STATE = "1";
                cmd.CommandText = ($"update RFID_STATE set RFID_STATE ='{RFID_STATE}'");
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            timer1.Interval = 700;
            timer1.Start();
            label2.Text = "RFID ON";
        }

        public void Read_RFID_And_UPDATE_UserInfo()
        {
            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = ($"select RFID_STATE from RFID_STATE");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                RFID_STATE = rdr["RFID_STATE"].ToString();
            }
            rdr.Close();
            
            if (RFID_STATE == "0") ;
            else
            {

                if (!g_isConnected)
                {
                    RFID_conn();
                    //MessageBox.Show("RFID 연결상태를 확인해주세요.");
                }
                else
                {
                    string time = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");


                    byte[] TagLength = new byte[51];
                    byte TagFound = 0;
                    byte[] TagType = new byte[51];
                    byte[] SN = new byte[451];
                    byte portState = 0x00;
                    int ctr;
                    string snstr;
                    g_retCode = ACR120U.ACR120_ListTags(g_rHandle, ref TagFound, ref TagType[0], ref TagLength[0], ref SN[0]);

                    if (g_retCode < 0)
                    {
                        //DisplayMessage(ACR120U.GetErrMsg(g_retCode));
                    }
                    else
                    {
                        snstr = "";
                        for (ctr = 0; ctr < TagLength[0]; ctr++)
                        {

                            snstr = snstr + string.Format("{0:X2} ", SN[ctr]);

                        }

                        cmd.CommandText = ($"select rfid from emp_info");
                        rdr = cmd.ExecuteReader();
                        int work = 0;
                        string Emp_id = "";
                        string Emp_name = "";
                        string rfid = ""; 

                        while (rdr.Read())
                        {
                            // Emp_id = rdr["emp_id"].ToString();
                            // Emp_name = rdr["emp_name"] as string;
                            rfid = rdr["rfid"] as string;
                            Console.WriteLine(rfid);
                            if (rfid == snstr)
                                work = 1;
                        }
                        rdr.Close();
                        label3.Text = snstr;

                        cmd.CommandText = ($"select emp_id, emp_name from emp_info where rfid = '{snstr} '");
                        rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            Emp_id = rdr["emp_id"].ToString();
                            Emp_name = rdr["emp_name"] as string;
                        }
                        rdr.Close();


                        if (work == 1)
                        {
                            cmd.CommandText = $"insert into attendance_check (emp_id, emp_name, time,io) " +
                                                 $"values('{Emp_id}','{Emp_name}','{time}',{inout})";
                            cmd.ExecuteNonQuery();

                        }

                        if (string.IsNullOrEmpty(snstr) == false && work==1)
                        {
                            portState = Convert.ToByte(4);// 0(소리X led X) ,4(소리 O) ,64(led O), 68(LED,소리O)
                            g_retCode = ACR120U.ACR120_WriteUserPort(g_rHandle, portState);
                            Thread.Sleep(300);
                            //Delay(300);
                            portState = Convert.ToByte(64);
                            g_retCode = ACR120U.ACR120_WriteUserPort(g_rHandle, portState);

                            //Delay(2000);
                            Thread.Sleep(2000); // thread delay(test)
                                
                            return;
                        }
                        else if (string.IsNullOrEmpty(snstr) == false && work == 1)
                        {

                        }

                    }
                }
            }
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button_stop();
/*            label2.Text = "RFID OFF";


            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;


            cmd.CommandText = ($"select RFID_STATE from RFID_STATE");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                //Emp_id = rdr["emp_id"].ToString();
                RFID_STATE = rdr["RFID_STATE"].ToString();

            }
            rdr.Close();

            if (RFID_STATE == "1")
            {
                RFID_STATE = "0";
                cmd.CommandText = ($"update RFID_STATE set RFID_STATE ='{RFID_STATE}'");
                cmd.ExecuteNonQuery();
            }


            conn.Close();
            timer1.Stop();*/
        }
        public void button_stop()
        {
            label2.Text = "RFID OFF";


            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;


            cmd.CommandText = ($"select RFID_STATE from RFID_STATE");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                //Emp_id = rdr["emp_id"].ToString();
                RFID_STATE = rdr["RFID_STATE"].ToString();

            }
            rdr.Close();

            if (RFID_STATE == "1")
            {
                RFID_STATE = "0";
                cmd.CommandText = ($"update RFID_STATE set RFID_STATE ='{RFID_STATE}'");
                cmd.ExecuteNonQuery();
            }


            conn.Close();
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Read_RFID_And_UPDATE_UserInfo();
        }



        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("RFID가 종료되었습니다.");
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //button_start();
            portIndex = 0;

            portNames = SerialPort.GetPortNames();
            portNames_Old = portNames;

            serialPort_Open();

            timer2.Interval = 5000;
            timer3.Interval = 1000;
            timer2.Start();

            RFID_Hide();
        }
        private void RFID_Hide()
        {
            this.ShowInTaskbar = false;
            this.Visible = false;

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if(this.WindowState== FormWindowState.Minimized)
            RFID_Hide();
        }

        private void Tray_icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;

            this.Visible = true;

            this.WindowState = FormWindowState.Normal;
        }


        private void timer2_Tick(object sender, EventArgs e)
        {   //아두이노와 연결됄 때까지 Serial Port 찾는 Timer
            if (bConnect == true)
            {
                timer2.Stop();
                timer3.Start();
            }
            else
            {
                portNames = SerialPort.GetPortNames();

                if (portNames.SequenceEqual(portNames_Old) == false)
                {
                    portNames_Old = portNames;
                    portIndex = 0;
                }

                if (serialPort1.IsOpen)  //시리얼포트가 열려있을 때만
                {
                    serialPort1.Close();
                    this.richTextBox1.AppendText(serialPort1.PortName + "포트가 닫혔습니다." + "\n");
                }

                serialPort_Open();
            }

        }

        private void RFID_conn()
        {

            int ctr = 0;
            byte[] FirmwareVer = new byte[31];
            byte[] FirmwareVer1 = new byte[20];
            byte infolen = 0x00;
            string FirmStr;
            int port = 0;
            ACR120U.tReaderStatus ReaderStat = new ACR120U.tReaderStatus();



            g_rHandle = ACR120U.ACR120_Open(0);
            //g_rHandle = 0;
            if (g_rHandle != 0)
            {
                //DisplayMessage("[X] " + ACR120U.GetErrMsg(g_rHandle));
                //port = port + 1;
                //g_rHandle = ACR120U.ACR120_Open(port);
                g_rHandle = ACR120U.ACR120_Reset(g_rHandle);
            }
            else
            {

                //DisplayMessage("Connected to USB" + string.Format("{0}", 0 + 1));
                g_isConnected = true;

                //Get the DLL version the program is using
                g_retCode = ACR120U.ACR120_RequestDLLVersion(ref infolen, ref FirmwareVer[0]);
                if (g_retCode < 0) ;

                //DisplayMessage("[X] " + ACR120U.GetErrMsg(g_retCode));

                else
                {
                    FirmStr = "";
                    for (ctr = 0; ctr < Convert.ToInt16(infolen) - 1; ctr++)
                        FirmStr = FirmStr + char.ToString((char)(FirmwareVer[ctr]));
                    //DisplayMessage("DLL Version : " + FirmStr);
                }

                //Routine to get the firmware version.
                g_retCode = ACR120U.ACR120_Status(g_rHandle, ref FirmwareVer1[0], ref ReaderStat);
                if (g_retCode < 0) ;

                //DisplayMessage("[X] " + ACR120U.GetErrMsg(g_retCode));

                else
                {
                    FirmStr = "";
                    for (ctr = 0; ctr < Convert.ToInt16(infolen); ctr++)
                        if ((FirmwareVer1[ctr] != 0x00) && (FirmwareVer1[ctr] != 0xFF))
                            FirmStr = FirmStr + char.ToString((char)(FirmwareVer1[ctr]));
                    //DisplayMessage("Firmware Version : " + FirmStr);
                }

            }
        }

        private void RFID_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)  //시리얼포트가 열려있을 때만
            {
                serialPort1.Close();
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            String ReceiveData = serialPort1.ReadLine();  //시리얼 버터에 수신된 데이타를 ReceiveData 읽어오기

            if (ReceiveData != string.Empty)
            {
                String upperString = ReceiveData.ToUpper();

                string recvTime, sendTime;
                recvTime = DateTime.Now.ToString("HH:mm:ss:fff");

                if (upperString.IndexOf("SEND") != -1)
                {
                    sendTime = DateTime.Now.ToString("HH:mm:ss:fff");

                    bConnect = true;
                    //Thread.Sleep(50);
                    Delay(50);  //아두이노 수신시간을 벌어주기 위한 delay
                    serialPort1.WriteLine("RECV");
                    sendTime = DateTime.Now.ToString("HH:mm:ss:fff");
                }
                else if (upperString.IndexOf("TIME_REQUEST") != -1)
                {
                    bConnect = true;

                    string arduinoTimeSet;
                    arduinoTimeSet = DateTime.Now.ToString("HHmmss");

                    string dateSet = "TIME_SET:" + arduinoTimeSet + inout;


                    //Thread.Sleep(50);
                    Delay(50);  //아두이노 수신시간을 벌어주기 위한 delay
                    serialPort1.WriteLine(dateSet);
                    
                    sendTime = DateTime.Now.ToString("HH:mm:ss:fff");
                }

                if (upperString.IndexOf("MODE1") != -1)
                {   //Punch IN
                    inout = "1";
                    bConnect = true;
                }
                else if (upperString.IndexOf("MODE0") != -1)
                {   //Punch OUT
                    inout = "0";
                    bConnect = true;
                }
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {   //연결된 Serial Port가 연결 해제됐는지 Check 하는 Timer
            if (!serialPort1.IsOpen)  //시리얼포트가 열려있을 때만
            {
                bConnect = false;
                portIndex = 0;
                portNames = SerialPort.GetPortNames();

                this.richTextBox1.AppendText(serialPort1.PortName + "포트가 닫혔습니다." + "\n");
                timer2.Start();
                timer3.Stop();
            }

        }

        private void serialPort_Open()
        {
            if (portIndex < portNames.Length)
            {
                serialPort1.PortName = portNames[portIndex];

                if (portIndex < portNames.Length - 1)
                {
                    portIndex++;
                }
                else
                {
                    portIndex = 0;
                }

                try
                {
                    serialPort1.BaudRate = 9600;  //아두이노에서 사용할 전송률를 지정하자
                    serialPort1.DataBits = 8;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Parity = Parity.None;

                    serialPort1.Open();  //시리얼포트 열기

                    this.richTextBox1.AppendText(serialPort1.PortName + "포트가 열렸습니다." + "\n");
                }
                catch (System.IO.IOException)
                {
                    this.richTextBox1.AppendText(serialPort1.PortName + "는 준비되지 않은 포트입니다." + "\n");
                }
                catch (System.UnauthorizedAccessException)
                {
                    this.richTextBox1.AppendText(serialPort1.PortName + "는 이미 사용중인 포트입니다." + "\n");
                }
            }
            else
            {
                portIndex = 0;
            }
        }
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }



    }
}
