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
        public string strConn = "Server=192.168.0.31;" +
                               "Database=test;" +
                               "Uid=test;" +
                               "Pwd=1234;" +
                               "charset=utf8;";

        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;

        OpenFileDialog ofd = new OpenFileDialog();

        public emp_Regis()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            if (imgPath != "")
            {

                fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                FileSize = (UInt32)fs.Length;

                rawData = new byte[FileSize];
                fs.Read(rawData, 0, (int)FileSize);
                fs.Close();

                SQL = $"INSERT INTO image (ImageNo, Image, Image_name) VALUES(@ImageNo, @Image, @ImageName)";
                SQL2 = $"insert into emp_info (emp_id, emp_name, emp_email, emp_tel, emp_addr, emp_emer_tel, blood_type, dept_id) values ('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{textBox4.Text}','{textBox5.Text}','{textBox6.Text}','{textBox7.Text}','{textBox8.Text}')";

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
                cmd.CommandText = $"insert into emp_info (emp_id, emp_name, emp_email, emp_tel, emp_addr, emp_emer_tel, blood_type, dept_id) " +
        $"values ('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{textBox4.Text}'," +
        $"'{textBox5.Text}','{textBox6.Text}','{textBox7.Text}','{textBox8.Text}')";
                cmd.ExecuteNonQuery();

                MessageBox.Show("작업자 등록이 완료되었습니다.");
                this.Close();
            }

        }
    }   
}
