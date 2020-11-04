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
                    txtPath.Text = ofd.FileName;

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
                }
                rdr.Close();

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

                    SQL = $"INSERT INTO image (ImageNo, Image, Image_name) VALUES(@ImageNo, @Image, @ImageName)";
                    SQL2 = $"insert into emp_info (emp_id, emp_name, emp_email, emp_tel, emp_addr, emp_emer_tel, blood_type, dept_id, rfid) values ({textBox1.Text},'{textBox2.Text}','{textBox3.Text}','{textBox4.Text}','{textBox5.Text}','{textBox6.Text}','{comboBox1.Text}',{a} ,'{textBox7.Text}')";

                    cmd.CommandText = SQL;

                    cmd.Parameters.AddWithValue("@ImageNo", ImgNum);
                    cmd.Parameters.AddWithValue("@Image", rawData);
                    cmd.Parameters.AddWithValue("@ImageName", imgPath);

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = SQL2;
                    cmd.ExecuteNonQuery();

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
            catch(MySqlException ex)
            {

                MessageBox.Show(ex.ToString());
                MessageBox.Show("중복된 ID입니다.");
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



                string[] emp_info = new string[] { Emp_id, Emp_name, Emp_email, Emp_tel, Emp_addr, Emp_emer_tel, Blood_type, dept_id};

                mp.dataGridView1.Rows.Add(emp_info);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox7.Text = userInfo.SnStr;
        }
    }   
}
