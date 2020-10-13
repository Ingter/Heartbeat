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

        public string strConn = "Server=192.168.0.31;" +
                         "Database=test;" +
                         "Uid=test;" +
                         "Pwd=1234;" +
                         "charset=utf8;";


        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;

        OpenFileDialog ofd = new OpenFileDialog();

        private string eu_value;

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

        public Emp_Update()
        {
            InitializeComponent();
        }

        private void Emp_Update_Load(object sender, EventArgs e)  // 창 로드
        {
            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            emp_name.Text = Passvalue2;

            cmd.CommandText = ($"select * from emp_info where emp_id = '{Passvalue2}'");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string Emp_id = rdr["emp_id"] as string;

                string Emp_name = rdr["emp_name"] as string;

                string Emp_email = rdr["emp_email"] as string;

                string Emp_tel = rdr["emp_tel"] as string;

                string Emp_addr = rdr["emp_addr"] as string;

                string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                string blood_type = rdr["blood_type"] as string;

                string Dept_id = rdr["dept_id"] as string;

                emp_name.Text = Emp_name;
                emp_email.Text = Emp_email;
                emp_tel.Text = Emp_tel;
                emp_etel.Text = Emp_emer_tel;
                emp_addr.Text = Emp_addr;
                emp_bl.Text = blood_type;
                emp_d.Text = Dept_id;
                

            }

          

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

                conn = new MySqlConnection(strConn);
                conn.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conn;  //DB 연결


                cmd.CommandText = ("select * from emp_info");   //직원 정보 테이블 쿼리

                rdr = cmd.ExecuteReader();

                while (rdr.Read())  //DB 데이터 가져옴
                {
                    string EmpI = rdr["emp_id"] as string;  //직원 id 값 변수에 넣음
                    EmpID = Convert.ToInt32(EmpI);         // int 값으로 변경
                    if (pass == EmpID)
                        break;
                }
                rdr.Close();

                if (imgPath != "")  // 사용자가 이미지를 등록했을 때  실행
                {

                    fs = new FileStream($@"C:\Heartbeat\employee_pic\{imgPath}", FileMode.Open, FileAccess.Read);
                    FileSize = (UInt32)fs.Length;

                    rawData = new byte[FileSize];
                    fs.Read(rawData, 0, (int)FileSize);
                    fs.Close();

                    cmd.CommandText = ("select count(*) from image");
                    object count = cmd.ExecuteScalar();
                    CountRow = Convert.ToInt32(count);

                    cmd.CommandText = ($"select * from image");
                    rdr = cmd.ExecuteReader();

                    if (CountRow != 0) //테이블에 행이 있을 때 실행
                    {
                        while (rdr.Read())
                        {

                            string ImageNum = rdr["ImageNo"].ToString();
                            int ImageNumber = Convert.ToInt32(ImageNum);

                            if (ImageNumber == EmpID) // 사용자 정보에 원래 사진이 있었을 때 실행
                            {
                                SQL = $"update image set Image = @Image, Image_name = @ImageName where ImageNo = '{EmpID}'";
                                SQL2 = $"update emp_info set emp_name='{emp_name.Text}', emp_email='{emp_email.Text}',emp_tel = '{emp_tel.Text}', emp_emer_tel='{emp_etel.Text}',emp_addr='{emp_addr.Text}', blood_type='{emp_bl.Text}', dept_id = '{emp_d.Text}' where emp_id = '{Passvalue2}'";
                                break;
                            }

                            else // 사용자 정보에 원래 사진이 없었을 때 실행asd
                            {
                                SQL = $"INSERT INTO image (ImageNo, Image, Image_name) VALUES(@ImageNo, @Image, @ImageName)";
                                SQL2 = $"update emp_info set emp_name='{emp_name.Text}', emp_email='{emp_email.Text}',emp_tel = '{emp_tel.Text}', emp_emer_tel='{emp_etel.Text}',emp_addr='{emp_addr.Text}', blood_type='{emp_bl.Text}', dept_id = '{emp_d.Text}' where emp_id = '{Passvalue2}'";

                            }

                        }
                    }
                    else
                    {
                        SQL = $"INSERT INTO image (ImageNo, Image, Image_name) VALUES(@ImageNo, @Image, @ImageName)";
                        SQL2 = $"update emp_info set emp_name='{emp_name.Text}', emp_email='{emp_email.Text}',emp_tel = '{emp_tel.Text}', emp_emer_tel='{emp_etel.Text}',emp_addr='{emp_addr.Text}', blood_type='{emp_bl.Text}', dept_id = '{emp_d.Text}' where emp_id = '{Passvalue2}'";
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


                    cmd.CommandText = ($"select * from emp_info where emp_id = '{Passvalue2}'");
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        string Emp_id = rdr["emp_id"] as string;

                        string Emp_name = rdr["emp_name"] as string;

                        string Emp_tel = rdr["emp_tel"] as string;

                        string Emp_addr = rdr["emp_addr"] as string;

                        string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                        string blood_type = rdr["blood_type"] as string;

                        string Dept_id = rdr["dept_id"] as string;

                        dp.na.Text = Emp_name;
                        dp.emp_tel.Text = Emp_tel;
                        dp.emp_etel.Text = Emp_emer_tel;
                        dp.emp_addr.Text = Emp_addr;
                        dp.emp_bl.Text = blood_type;
                        dp.emp_d.Text = Dept_id;

                    }

                    this.Close();
                }


                else
                {
                    cmd.CommandText = $"update emp_info set emp_name='{emp_name.Text}', emp_email='{emp_email.Text}',emp_tel = '{emp_tel.Text}', emp_emer_tel='{emp_etel.Text}',emp_addr='{emp_addr.Text}', blood_type='{emp_bl.Text}', dept_id = '{emp_d.Text}' where emp_id = '{Passvalue2}'";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = ($"select * from emp_info where emp_id = '{Passvalue2}'");
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        string Emp_id = rdr["emp_id"] as string;

                        string Emp_name = rdr["emp_name"] as string;

                        string Emp_tel = rdr["emp_tel"] as string;

                        string Emp_addr = rdr["emp_addr"] as string;

                        string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                        string blood_type = rdr["blood_type"] as string;

                        string Dept_id = rdr["dept_id"] as string;

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

        // 부모 창한테 데이터 보내려고 하는데 안됨
       private void Emp_Update_FormClosing(object sender, FormClosingEventArgs e)
        {
            //((Detail_Page)(this.Owner)).
        }
    }
}
