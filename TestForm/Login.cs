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

        public static int stop = 0;

/*        public Emp_Update fm_update;

        public Emp_Update eu;
        public Login(Emp_Update _eu)
        {
            InitializeComponent();
            eu = _eu;

        }*/
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
            //fm_update.fm_login = this;
            


        }


        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Enter)
            { this.button1_Click(sender, e); }
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
                    timer1.Stop();
                    //mp.Passvalue = snstr;
                    mp.ShowDialog();

                }
                else
                {
                    MessageBox.Show("로그인 실패");
                    textBox1.Clear();
                    textBox2.Clear();

                }
                //emp_Regis ers = new emp_Regis(); // 인스턴스화(객체 생성)
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

        MyRFID mrfid = new MyRFID();
        public void timer1_Tick(object sender, EventArgs e)
        {
            mrfid.Read_RFID_And_UPDATE_UserInfo(userinfo);
 
            
            //여기에 update에서 클릭이 들어온다면
            //timer1.stop();
            //수정완료 클릭이 들어오면
            //timer1.start();

        }
        
        public void EventMehod(string str)
        {
            string test = str;
            MessageBox.Show(test);
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