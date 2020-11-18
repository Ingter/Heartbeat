using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TestForm
{
    public partial class Emp_Update : Form
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

        private string eu_value;
        public UserInfo userinfo;

        public Login fm_login;

        public string Passvalue2
        {
            get { return this.eu_value; }
            set { this.eu_value = value; }  // 다른폼에서 전달받은 값을 쓰기
  
        }


        Detail_Page dp;
        public Emp_Update(Detail_Page _dp)
        {
            InitializeComponent();
            dp = _dp;
            
        }

        public Emp_Update(UserInfo _userinfo)
        {
            InitializeComponent();
            userinfo = _userinfo;
        }

        public Emp_Update()
        {
            InitializeComponent();
        }

        private void Emp_Update_Load(object sender, EventArgs e)  // 창 로드
        {
            string[] bl = { "rh+ A","rh+ B","rh+ O","rh+ AB","rh- A","rh- B","rh- O","rh- AB"};
            string[] d = { "포장팀", "검수팀" };
            

           // Login lo = new Login(eu);
            //lo.userinfo.stop = 1;  // 전달자(Passvalue)를 통해서 dp페이지로 전달
            //lo.stop = 1;
            // 각 콤보박스에 데이타를 초기화
            emp_bl.Items.AddRange(bl);
            emp_d.Items.AddRange(d);

            string a;

            // 처음 선택값 지정. 첫째 아이템 선택
            //emp_bl.SelectedIndex = 0;
           // emp_d.SelectedIndex = 0;
            



            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            emp_name.Text = Passvalue2;

            cmd.CommandText = ($"select * from emp_info where emp_id = {Passvalue2}");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string Emp_id = rdr["emp_id"].ToString();

                string Emp_name = rdr["emp_name"] as string;

                string Emp_email = rdr["emp_email"] as string;

                string Emp_tel = rdr["emp_tel"] as string;

                string Emp_addr = rdr["emp_addr"] as string;

                string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                string blood_type = rdr["blood_type"] as string;

                string Dept_id = rdr["dept_id"].ToString();

                string RFID = rdr["rfid"] as string;


                if(Dept_id=="1")
                {
                    a = "포장팀";
                    emp_d.Text = a;
                }
                else if (Dept_id == "2")
                {
                    a = "검수팀";
                    emp_d.Text = a;
                }


                emp_name.Text = Emp_name;
                emp_email.Text = Emp_email;
                emp_tel.Text = Emp_tel;
                emp_etel.Text = Emp_emer_tel;
                emp_addr.Text = Emp_addr;
                emp_bl.Text = blood_type;
                emp_rfid.Text = RFID;

            }
            rdr.Close();
            string RFID_STATUS = "";
            cmd.CommandText = ($"select RFID_STATUS from RFID_STATUS");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                RFID_STATUS = rdr["RFID_STATUS"].ToString();

            }
            rdr.Close();
            if (RFID_STATUS == "1")
            {
                RFID_STATUS = "0";
                cmd.CommandText = ($"update RFID_STATUS set rfid_status ='{RFID_STATUS}'");
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            timer1.Interval = 1000;
            timer1.Start();



        }

        private void button2_Click(object sender, EventArgs e)  // 수정 버튼
        {

            try
            {
                string SQL = "";
                string SQL2 = "";

                UInt32 FileSize;
                byte[] rawData;
                FileStream fs;
                int EmpID = 0;
                int pass = Convert.ToInt32(Passvalue2);
                int CountRow = 0;
                int overlap = 0;

                conn = new MySqlConnection(strConn);
                conn.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conn;  //DB 연결


                cmd.CommandText = ("select * from emp_info");   //직원 정보 테이블 쿼리

                rdr = cmd.ExecuteReader();

                while (rdr.Read())  //DB 데이터 가져옴
                {
                    string EmpI = rdr["emp_id"].ToString();  //직원 id 값 변수에 넣음
                    string em_rfid = rdr["RFID"] as string;
                    EmpID = Convert.ToInt32(EmpI);         // int 값으로 변경
                    if (em_rfid == snstr&&snstr!="")
                        overlap = 1;

/*                    if (pass == EmpID)
                        break;*/
                }
                rdr.Close();
                if (overlap == 1)
                {
                    cmd.CommandText = ($"update emp_info set rfid ='null' where rfid ='{snstr}'");
                    cmd.ExecuteNonQuery();
                }

                if (imgPath != "")  // 사용자가 이미지를 등록했을 때  실행
                {


                    fs = new FileStream($@"C:\Heartbeat\employee_pic\{imgPath}", FileMode.Open, FileAccess.Read);
                    FileSize = (UInt32)fs.Length;

                    rawData = new byte[FileSize];
                    fs.Read(rawData, 0, (int)FileSize);
                    fs.Close();

                    cmd.CommandText = ("select count(*) from emp_img");
                    object count = cmd.ExecuteScalar();
                    CountRow = Convert.ToInt32(count);

                    cmd.CommandText = ($"select * from emp_img");
                    rdr = cmd.ExecuteReader();

                    if (CountRow != 0) //테이블에 행이 있을 때 실행
                    {
                        while (rdr.Read())
                        {

                            int a = 0;

                            if (emp_d.Text == "포장팀")
                            {
                                a = 1;
                            }
                            else
                            {
                                a = 2;
                            }


                            string ImageNum = rdr["ImageNo"].ToString();
                            int ImageNumber = Convert.ToInt32(ImageNum);
//여기 바꿔야 
                            if (ImageNumber == EmpID) // 사용자 정보에 원래 사진이 있었을 때 실행
                            {
                                SQL = $"update emp_img set Image = @Image, Image_name = @ImageName where ImageNo = {EmpID}";
                                SQL2 = $"update emp_info set emp_name='{emp_name.Text}', emp_email='{emp_email.Text}',emp_tel = '{emp_tel.Text}', emp_emer_tel='{emp_etel.Text}',emp_addr='{emp_addr.Text}', blood_type='{emp_bl.Text}', dept_id = '{a}', rfid= '{snstr}' where emp_id = {Passvalue2}";
                                break;
                            }

                            else // 사용자 정보에 원래 사진이 없었을 때 실행asd
                            {
                                SQL = $"INSERT INTO emp_img (ImageNo, Image, Image_name) VALUES(@ImageNo, @Image, @ImageName)";
                                SQL2 = $"update emp_info set emp_name='{emp_name.Text}', emp_email='{emp_email.Text}',emp_tel = '{emp_tel.Text}', emp_emer_tel='{emp_etel.Text}',emp_addr='{emp_addr.Text}', blood_type='{emp_bl.Text}', dept_id = {a}, rfid= '{snstr}' where emp_id = {Passvalue2}";
                            }

                        }
                    }
                    else
                    {
                        int a = 0;

                        if (emp_d.Text == "포장팀")
                        {
                            a = 1;
                        }
                        else
                        {
                            a = 2;
                        }

                        SQL = $"INSERT INTO emp_img (ImageNo, Image, Image_name) VALUES(@ImageNo, @Image, @ImageName)";
                        SQL2 = $"update emp_info set emp_name='{emp_name.Text}', emp_email='{emp_email.Text}',emp_tel = '{emp_tel.Text}', emp_emer_tel='{emp_etel.Text}',emp_addr='{emp_addr.Text}', blood_type='{emp_bl.Text}', dept_id = {a}, rfid= '{snstr}' where emp_id = {Passvalue2}";
                    }

                    rdr.Close();

                    cmd.CommandText = SQL;

                    cmd.Parameters.AddWithValue("@ImageNo", EmpID);
                    cmd.Parameters.AddWithValue("@Image", rawData);
                    cmd.Parameters.AddWithValue("@ImageName", $@"C:\Heartbeat\employee_pic\{imgPath}");

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = SQL2;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("수정되었습니다.");


                    cmd.CommandText = ($"select * from emp_info where emp_id = {Passvalue2}");
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        string Emp_id = rdr["emp_id"].ToString();

                        string Emp_name = rdr["emp_name"] as string;

                        string Emp_tel = rdr["emp_tel"] as string;

                        string Emp_addr = rdr["emp_addr"] as string;

                        string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                        string blood_type = rdr["blood_type"] as string;

                        string Dept_id = rdr["dept_id"].ToString();

                        dp.na.Text = Emp_name;
                        dp.emp_tel.Text = Emp_tel;
                        dp.emp_etel.Text = Emp_emer_tel;
                        dp.emp_addr.Text = Emp_addr;
                        dp.emp_bl.Text = blood_type;
                        dp.emp_d.Text = Dept_id;

                    }
                    rdr.Close();
                   
                    cmd.CommandText = ($"select * from emp_img where ImageNo = {Passvalue2}");
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        string imgNum = rdr["ImageNo"].ToString();
                        string ImgName = rdr["Image_name"] as string;

                        if (imgNum != "")   // 등록해뒀던 이미지가 있는 직원만 이미지 불러오도록 함
                        {
                            dp.pictureBox1.Image = Image.FromFile(ImgName);
                        }
                    }
                    rdr.Close();

                    this.Close();
                }


                else
                {
                    int a = 0;

                    if(emp_d.Text=="포장팀")
                    {
                        a = 1;
                    }
                    else
                    {
                        a = 2;
                    }

                    

                    cmd.CommandText = $"update emp_info set emp_name='{emp_name.Text}', emp_email='{emp_email.Text}',emp_tel = '{emp_tel.Text}', emp_emer_tel='{emp_etel.Text}',emp_addr='{emp_addr.Text}', blood_type='{emp_bl.Text}', dept_id = {a}, rfid= '{snstr}' where emp_id = {Passvalue2}";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = ($"select * from emp_info where emp_id = {Passvalue2}");
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        string Emp_id = rdr["emp_id"].ToString();

                        string Emp_name = rdr["emp_name"] as string;

                        string Emp_tel = rdr["emp_tel"] as string;

                        string Emp_addr = rdr["emp_addr"] as string;

                        string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                        string blood_type = rdr["blood_type"] as string;

                        string Dept_id = rdr["dept_id"].ToString();

                        dp.na.Text = Emp_name;
                        dp.emp_tel.Text = Emp_tel;
                        dp.emp_etel.Text = Emp_emer_tel;
                        dp.emp_addr.Text = Emp_addr;
                        dp.emp_bl.Text = blood_type;
                        dp.emp_d.Text = Dept_id;

                    }

                    MessageBox.Show("수정되었습니다.");
                    this.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("수정되었습니다.");
            }
            conn.Close();


        }

        private void button1_Click(object sender, EventArgs e) // 이미지 변경 버튼
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
                    txtPath.Text = ofd.SafeFileName;

                    fs.Close();

                }

                imgPath = txtPath.Text;

                MessageBox.Show("파일이 정상적으로 업로드 되었습니다.");
            }


            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("파일이 정상적으로 업로드 되지 않았습니다.");
            } // end of try to catch finally
        }

       private void Emp_Update_FormClosing(object sender, FormClosingEventArgs e)
        {

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            string RFID_STATUS = "";
            cmd.CommandText = ($"select RFID_STATUS from RFID_STATUS");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                RFID_STATUS = rdr["RFID_STATUS"].ToString();

            }
            rdr.Close();
            if (RFID_STATUS == "0")
            {
                RFID_STATUS = "1";
                cmd.CommandText = ($"update RFID_STATUS set rfid_status ='{RFID_STATUS}'");
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            Read_RFID_And_UPDATE_UserInfo();
            if (snstr != "")
                emp_rfid.Text = snstr;

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
                    //snstr = rfid;


/*                    while (rdr.Read())
                    {

                        Emp_id = rdr["emp_id"].ToString();
                        Emp_name = rdr["emp_name"] as string;

                    }
                    rdr.Close();*/

                    //Emp_Update f1 = new Emp_Update(userinfo);


                    /*                    if (work == 1)
                                        {
                                            cmd.CommandText = $"insert into attendance_check (emp_id, emp_name, time) " +
                                                                 $"values('{Emp_id}','{Emp_name}','{time}')";
                                            cmd.ExecuteNonQuery();
                                        }*/

                    /*                    if (string.IsNullOrEmpty(snstr) == false)
                                        {
                                            timer1.Stop();
                                            Delay(2000);
                                            timer1.Start();
                                            return;
                                        }*/

                    conn.Close();

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
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
