using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
namespace TestForm
{



    //public delegate void EventHandler(string userName);

    public partial class Login : Form
    {
        public UserInfo userinfo = new UserInfo();
        public event EventHandler loginEventHandler;
        //Man_Page mp;

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


        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();

        }



        private void button1_Click(object sender, EventArgs e)
        {
            DbManager login = new DbManager();
            if (ControlCheck())
            {
                if (login.LoginCheck(textBox1.Text, textBox2.Text, out string proiroty))
                {

                    //string userName = textBox1.Text;
                    userinfo.priority = proiroty;
                    userinfo.UserName = textBox1.Text;

                    //loginEventHandler(userName);
                    DialogResult = DialogResult.OK;
                    Login ls = this;
                    Man_Page mp = new Man_Page(userinfo, ls);
                    this.Visible = false;
                    //mp.Passvalue = snstr;
                    mp.ShowDialog();

                }
                else
                {
                    MessageBox.Show("로그인 실패");
                    textBox1.Clear();
                    textBox2.Clear();

                }
                emp_Regis ers = new emp_Regis(); // 인스턴스화(객체 생성)
                                                 //ers.Passvalue = snstr;
                                                 //Man_Page mp = new Man_Page();

                login.rdr.Close();
            }





        }

        private bool ControlCheck()
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("아이디와 비밀번호를 입력해 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }
            else if (String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("비밀번호를 입력해 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }
            return true;
        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("종료하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            if (!g_isConnected)
            {
                RFID_conn();
                //MessageBox.Show("RFID 연결상태를 확인해주세요.");
            }
            else
            {
                {
                    string time = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");


                    byte[] TagLength = new byte[51];
                    byte TagFound = 0;
                    byte[] TagType = new byte[51];
                    byte[] SN = new byte[451];
                    int ctr;
                    string snstr;
                    g_retCode = ACR120U.ACR120_ListTags(g_rHandle, ref TagFound, ref TagType[0], ref TagLength[0], ref SN[0]);

                    if (g_retCode < 0)
                    {
                        //DisplayMessage(ACR120U.GetErrMsg(g_retCode));
                    }
                    else
                    {

                        //DisplayMessage("List Tag Success");
                        //DisplayMessage("Tag Found: " + String.Format("{0}", TagFound));

                        // Parse the serial number array
                        snstr = "";
                        for (ctr = 0; ctr < TagLength[0]; ctr++)
                        {

                            snstr = snstr + string.Format("{0:X2} ", SN[ctr]);
                            userinfo.SnStr = snstr;

                        }
                        Detail_Page DP = new Detail_Page();
                        DP.Passvalue = snstr;  // 전달자(Passvalue)를 통해서 dp페이지로 전달

                        conn = new MySqlConnection(strConn);
                        cmd = new MySqlCommand();
                        conn.Open();
                        cmd.Connection = conn;
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


                        cmd.CommandText = ($"select emp_id, emp_name from emp_info where rfid = '{snstr} '");
                        rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {

                            Emp_id = rdr["emp_id"].ToString();
                            Emp_name = rdr["emp_name"] as string;
                            label1.Text = Emp_id;
                            label2.Text = Emp_name;

                        }
                        rdr.Close();

                        //label1.Text = snstr;

                        if (work == 1)
                        {


                            cmd.CommandText = $"insert into attendance_check (emp_id, emp_name, time) " +
                                                 $"values('{Emp_id}','{Emp_name}','{time}')";
                            cmd.ExecuteNonQuery();


                        }

                        if (string.IsNullOrEmpty(snstr) == false)
                        {
                            timer1.Stop();
                            Delay(2000);
                            timer1.Start();
                            return;
                        }

                        conn.Close();

                    }
                }
            }
            //여기에 update에서 클릭이 들어온다면
            //timer1.stop();
            //수정완료 클릭이 들어오면
            //timer1.start();

        }
        private void RFID_conn()
        {

            int ctr = 0;
            byte[] FirmwareVer = new byte[31];
            byte[] FirmwareVer1 = new byte[20];
            byte infolen = 0x00;
            string FirmStr;
            ACR120U.tReaderStatus ReaderStat = new ACR120U.tReaderStatus();



            g_rHandle = ACR120U.ACR120_Open(0);
            if (g_rHandle != 0)
            {
                //DisplayMessage("[X] " + ACR120U.GetErrMsg(g_rHandle));
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