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

namespace TestForm
{
    public partial class Detail_Page : Form
    {


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


        public Detail_Page()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Detail_Page_Load(object sender, EventArgs e)
        {

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            na.Text = Passvalue;


            cmd.CommandText = ($"select * from emp_info where emp_name = '{Passvalue}'");
            rdr = cmd.ExecuteReader();
           
            while (rdr.Read())
            {
                string Emp_id = rdr["emp_id"] as string;

                string Emp_name = rdr["emp_name"] as string;

                string Emp_email = rdr["emp_email"] as string;

                string Emp_tel = rdr["emp_tel"] as string;

                string Emp_addr = rdr["emp_addr"] as string;

                string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                string blood_type= rdr["blood_type"] as string;

                string Dept_id = rdr["dept_id"] as string;



                emp_d.Text = Dept_id;
                emp_tel.Text = Emp_tel;
                emp_addr.Text = Emp_addr;
                emp_etel.Text = Emp_emer_tel;
                emp_bl.Text = blood_type;

                emp_id.Text = Emp_id;

            }


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

                    //na.Text = Passvalue;

                    cmd.CommandText = ($"delete from emp_info where emp_name = '{Passvalue}'");
                    cmd.ExecuteNonQuery();
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

            Emp_Update eu = new Emp_Update();
            eu.Passvalue2 = Passvalue;  // 전달자(Passvalue)를 통해서 dp페이지로 전달
            eu.Show();


        }
    }
}
