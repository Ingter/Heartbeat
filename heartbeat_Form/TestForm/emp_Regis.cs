using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
    public partial class emp_Regis : Form
    {
        string imgPath = "";
        public string strConn = "Server=192.168.0.173;" +
                               "Database=heartbeat;" +
                               "Uid=test;" +
                               "Pwd=1234;" +
                               "charset=utf8;";

        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;

        OpenFileDialog ofd = new OpenFileDialog();

        Man_Page mp;


        private string Form2_value;
        public string Passvalue
        {
            get { return this.Form2_value; }
            set { this.Form2_value = value; }  // 다른폼(Form1)에서 전달받은 값을 쓰기
        }

        public UserInfo userInfo;
        public emp_Regis(Man_Page _mp, UserInfo _uinfo)
        {
            InitializeComponent();
            mp = _mp;
            userInfo = _uinfo;
        }

        public emp_Regis()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            string RFID_STATE = "";
            cmd.CommandText = ($"select RFID_STATE from RFID_STATE");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
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

            timer1.Interval = 1000;
            timer1.Start();
            
        }

        private void button3_Click(object sender, EventArgs e) //이미지 저장 버튼
        {
            FileStream fs;
            try
            {

                //ofd.Filter = "jpg file(*.jpg)|*.jpg|png file(*.png)|*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    pictureBox1.Image = new Bitmap(ofd.FileName);
                    pictureBox1.Tag = ofd.FileName;
                    fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    //여기서 db에 저장


                    txtPath.Text = ofd.FileName;

                    fs.Close();

                }
                imgPath = txtPath.Text;

                MessageBox.Show("파일이 정상적으로 업로드 되었습니다.");

                /////
                ///불러오는 부분
            }


            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("파일이 정상적으로 업로드 되지 않았습니다.");
            } // end of try to catch finally

        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }






        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {

                UInt32 FileSize;
                byte[] rawData;
                FileStream fs;
                int ImgNum = 0;
                int overlap = 0;
                conn = new MySqlConnection(strConn);
                conn.Open();

                cmd = new MySqlCommand();

                cmd.Connection = conn;


                cmd.CommandText = ("select * from emp_info");

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string ImgN = textBox1.Text;
                    ImgNum = Convert.ToInt32(ImgN);
                    string EmpI = rdr["emp_id"].ToString();  //직원 id 값 변수에 넣음
                    string em_rfid = rdr["RFID"] as string;
                    if (em_rfid == textBox7.Text&& textBox7.Text != "")
                        overlap = 1;

                }
                rdr.Close();


                if (overlap == 1)
                {
                    cmd.CommandText = ($"update emp_info set rfid ='null' where rfid ='{textBox7.Text}'");
                    cmd.ExecuteNonQuery();
                }


                string SQL;
                string SQL2;
                int a = 0;
                int b = 0;




                if (imgPath != "")
                {

                    if (comboBox2.SelectedIndex == 0)
                    {
                        a = 1;
                    }
                    else if (comboBox2.SelectedIndex == 1)
                    {
                        a = 2;
                    }


                    fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                    FileSize = (UInt32)fs.Length;

                    rawData = new byte[FileSize];
                    fs.Read(rawData, 0, (int)FileSize);
                    fs.Close();

                    SQL = $"INSERT INTO emp_img (ImageNo, Image, Image_name) VALUES(@ImageNo, @Image, @ImageName)";
                    SQL2 = $"insert into emp_info (emp_id, emp_name, emp_email, emp_tel, emp_addr, emp_emer_tel, blood_type, dept_id, rfid) values ({textBox1.Text},'{textBox2.Text}','{textBox3.Text}','{textBox4.Text}','{textBox5.Text}','{textBox6.Text}','{comboBox1.Text}',{a} ,'{textBox7.Text}')";

                    //쿼리 넣는 순서를 emp_info로 변경해서 오류 수정
                    cmd.CommandText = SQL2;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = SQL;

                    cmd.Parameters.AddWithValue("@ImageNo", ImgNum);
                    cmd.Parameters.AddWithValue("@Image", rawData);
                    cmd.Parameters.AddWithValue("@ImageName", imgPath);

                    cmd.ExecuteNonQuery();

/*                    cmd.CommandText = SQL2;
                    cmd.ExecuteNonQuery();*/

                    MessageBox.Show("작업자 등록이 완료되었습니다.");
                    this.Close();
                }

                else
                {

                    if (comboBox2.SelectedIndex == 0)
                    {
                        a = 1;
                    }
                    else if (comboBox2.SelectedIndex == 1)
                    {
                        a = 2;
                    }

                    cmd.CommandText = $"insert into emp_info (emp_id, emp_name, emp_email, emp_tel, emp_addr, emp_emer_tel, blood_type, dept_id, rfid) " +
                     $"values ({textBox1.Text},'{textBox2.Text}','{textBox3.Text}','{textBox4.Text}'," +
                    $"'{textBox5.Text}','{textBox6.Text}'  ,'{comboBox1.Text}',{a} ,'{textBox7.Text}')";
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("작업자 등록이 완료되었습니다.");
                    this.Close();
                }

            }
            catch(FormatException ex)
            {

                //MessageBox.Show(ex.ToString());
                MessageBox.Show("모든 정보를 입력해주세요.");
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.ToString());
                //두개 구분가능한가?
                MessageBox.Show("중복ID확인 또는 빈칸을 모두 채워주세요.");
            }
        }

        private void emp_Regis_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = ("select * from emp_info;");
            rdr = cmd.ExecuteReader();
            mp.dataGridView1.Rows.Clear();

            string a = "";

            while (rdr.Read())
            {
                string Emp_id = rdr["emp_id"].ToString();

                string Emp_name = rdr["emp_name"] as string;

                string Emp_email = rdr["emp_email"] as string;

                string Emp_tel = rdr["emp_tel"] as string;

                string Emp_addr = rdr["emp_addr"] as string;

                string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                string Blood_type = rdr["blood_type"] as string;

                string dept_id = rdr["dept_id"].ToString();

                if (dept_id == "1")
                {
                    a = "포장팀";
                }

                else if (dept_id == "2")
                {
                    a = "검수팀";
                }

                string[] emp_info = new string[] { Emp_id, Emp_name, Emp_email, Emp_tel, Emp_addr, Emp_emer_tel, Blood_type, a};

                mp.dataGridView1.Rows.Add(emp_info);
            }
            rdr.Close();

            string RFID_STATE = "";
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
            timer1.Stop();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Read_RFID_And_UPDATE_UserInfo();
            if (snstr != "")
                textBox7.Text = snstr;
        }

        private int g_rHandle, g_retCode;
        private bool g_isConnected = false;
        public string snstr = "";

        public void Read_RFID_And_UPDATE_UserInfo()
        {

            if (!g_isConnected)
            {
                RFID_conn();
                //MessageBox.Show("RFID 연결상태를 확인해주세요.");
            }
            else
            {

                byte[] TagLength = new byte[51];
                byte TagFound = 0;
                byte[] TagType = new byte[51];
                byte[] SN = new byte[451];
                int ctr;
                snstr = "";

                g_retCode = ACR120U.ACR120_ListTags(g_rHandle, ref TagFound, ref TagType[0], ref TagLength[0], ref SN[0]);

                if (g_retCode < 0) ;
                else
                {

                    for (ctr = 0; ctr < TagLength[0]; ctr++)
                    {

                        snstr = snstr + string.Format("{0:X2} ", SN[ctr]);
                        //userinfo.SnStr = snstr;

                    }
                    //userinfo.SnStr = snstr;
                    /*                    Detail_Page DP = new Detail_Page();
                                        DP.Passvalue = snstr;  // 전달자(Passvalue)를 통해서 dp페이지로 전달*/

                    conn = new MySqlConnection(strConn);
                    cmd = new MySqlCommand();
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = ($"select rfid from emp_info");
                    rdr = cmd.ExecuteReader();
                    string Emp_id = "";
                    string Emp_name = "";
                    string rfid = "";

                    while (rdr.Read())
                    {
                        rfid = rdr["rfid"] as string;
                        Console.WriteLine(rfid);

                    }
                    rdr.Close();
                    conn.Close();

                }
            }
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
    }   
}
