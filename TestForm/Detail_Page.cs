using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using System.IO;

namespace TestForm
{
    public partial class Detail_Page : Form
    {
        string imgPath = "";
        string ImgName = "";
        public string strConn = "Server=192.168.0.31;" +
                                 "Database=test;" +
                                 "Uid=test;" +
                                 "Pwd=1234;" +
                                 "charset=utf8;";


        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;


        private string dp_value;

        public string Passvalue
        {
            get { return this.dp_value; }
            set { this.dp_value = value; }  // 다른폼에서 전달받은 값을 쓰기
        }

        Man_Page mp;
        public Detail_Page(Man_Page _mp)
        {
            InitializeComponent();
            mp = _mp;
        }

        public Detail_Page()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Detail_Page_Load(object sender, EventArgs e)
        {

            UInt32 FileSize;
            byte[] rawData;
            FileStream fs;
            int ImgNum = 0;

            string SQL = "";
            string SQL2 = "";

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            emp_id.Text = Passvalue;

            // 직원 이미지 불러오기
            cmd.CommandText = ($"select * from image where ImageNo = '{Passvalue}'");
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string imgNum = rdr["ImageNo"].ToString();
                ImgName = rdr["Image_name"] as string;

                if (imgNum != "")   // 등록해뒀던 이미지가 있는 직원만 이미지 불러오도록 함
                {
                    pictureBox1.Image = Image.FromFile(ImgName);
                }
            }
            rdr.Close();

            // 직원 정보 불러오기
            cmd.CommandText = ($"select * from emp_info where emp_id = '{Passvalue}'");
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

                na.Text = Emp_name;
                emp_d.Text = Dept_id;
                emp_tel.Text = Emp_tel;
                emp_addr.Text = Emp_addr;
                emp_etel.Text = Emp_emer_tel;
                emp_bl.Text = blood_type;



            }

            conn.Close();

            conn.Open();
            cmd.CommandText = ($"select * from emp_detail  where emp_id = '{Passvalue}'");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string Emp_id = rdr["emp_id"] as string;

                string Emp_name = rdr["emp_name"] as string;

                string Body_temp = rdr["body_temp"].ToString();

                string Heart_rate = rdr["heart_rate"].ToString();

                string[] rw = new string[] { Emp_name, Body_temp, Heart_rate };

                dataGridView1.Rows.Add(rw);

            }
            conn.Close();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    Detail_Page dp = new Detail_Page();

                    conn = new MySqlConnection(strConn);
                    cmd = new MySqlCommand();
                    conn.Open();
                    cmd.Connection = conn;

                    cmd.CommandText = ($"delete from emp_info where emp_id = '{Passvalue}'");
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    conn.Open();
                    cmd.CommandText = ($"delete from image where imageNO = '{emp_id.Text}'");
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("직원 삭제 완료");

                    dp.Close();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    MessageBox.Show("직원 삭제 실패");
                    this.Close();
                }



                this.Close();
            }
            else { }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Detail_Page dp = this;
            Emp_Update eu = new Emp_Update(dp);
            eu.Passvalue2 = Passvalue;  // 전달자(Passvalue)를 통해서 dp페이지로 전달
            eu.ShowDialog(this);


        }

        private void Detail_Page_FormClosing(object sender, FormClosingEventArgs e)
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
                string Emp_id = rdr["emp_id"] as string;

                string Emp_name = rdr["emp_name"] as string;

                string Emp_email = rdr["emp_email"] as string;

                string Emp_tel = rdr["emp_tel"] as string;

                string Emp_addr = rdr["emp_addr"] as string;

                string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                string Blood_type = rdr["blood_type"] as string;

                string dept_id = rdr["dept_id"] as string;

                string[] emp_info = new string[] { Emp_id, Emp_name, Emp_email, Emp_tel, Emp_addr, Emp_emer_tel, Blood_type, dept_id };

                mp.dataGridView1.Rows.Add(emp_info);
            }
        }
    }
}
