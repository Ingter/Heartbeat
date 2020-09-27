using MySql.Data.MySqlClient;
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

namespace TestForm
{
    public partial class Form1 : Form
    {
        public string strConn = "Server=192.168.0.22;" +
                               "Database=heartbeat;" +
                               "Uid=heartbeat;" +
                               "Pwd=1234;" +
                               "charset=utf8;";

        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;


        public Form1()
        {
            InitializeComponent();
        }






        private void button1_Click(object sender, EventArgs e) //가입버튼
        {

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;


            try
            {
                cmd.CommandText = $"insert into emp_info (emp_id, emp_name, " +
                                  $"emp_email, emp_tel, emp_addr, emp_emer_tel, blood_type, dept_id) values " +
                                  $"('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{textBox4.Text}','{textBox5.Text}','{textBox8.Text}','{textBox6.Text}','{textBox6.Text}')";
                



                cmd.ExecuteNonQuery();
                MessageBox.Show("직원 등록 완료되었습니다.");
                this.Close();
            }

            catch (MySqlException)
            {
                MessageBox.Show("중복된 아이디, 비밀빈호 입니다.");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) //이미지 저장 버튼
        {


            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(ofd.FileName);
                pictureBox1.Tag = ofd.FileName;
            }
           

            string SQL;
            UInt32 FileSize;
            byte[] rawData;
            FileStream fs;
 

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            /*try
            {
                fs = new FileStream(@txtPath.Text, FileMode.Open, FileAccess.Read); //testPath = 사진경로인듯
                FileSize = (UInt32)fs.Length;

                rawData = new byte[FileSize];
                fs.Read(rawData, 0, (int)FileSize);
                fs.Close();

                conn.Open();

                SQL = "INSERT INTO emp_info VALUES(@emp_img)";

                cmd.Connection = conn;
                cmd.CommandText = SQL;

                cmd.Parameters.AddWithValue("@emp_img", rawData);
                cmd.ExecuteNonQuery();

                MessageBox.Show("파일이 정상적으로 업로드 되었습니다.");

                conn.Close();
            }



            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("파일이 정상적으로 업로드 되지않았습니다.");
            } // end of try to catch finally*/

        }
    }   
}
