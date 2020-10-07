using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TestForm
{
    public partial class Emp_Update : Form
    {
        public string strConn = "Server=192.168.0.31;" +
                         "Database=test;" +
                         "Uid=test;" +
                         "Pwd=1234;" +
                         "charset=utf8;";


        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;

        private string eu_value;

        public string Passvalue2
        {
            get { return this.eu_value; }
            set { this.eu_value = value; }  // 다른폼에서 전달받은 값을 쓰기
  
        }

        public Emp_Update()
        {
            InitializeComponent();
        }

        private void Emp_Update_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            emp_name.Text = Passvalue2;




            cmd.CommandText = ($"select * from emp_info where emp_name = '{Passvalue2}'");
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


                emp_id.Text = Emp_id;
                emp_name.Text = Emp_name;
                emp_email.Text = Emp_email;
                emp_tel.Text = Emp_tel;
                emp_etel.Text = Emp_emer_tel;
                emp_addr.Text = Emp_addr;
                emp_bl.Text = blood_type;
                emp_d.Text = Dept_id;
                

            }

          

        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {

                conn = new MySqlConnection(strConn);
                cmd = new MySqlCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = ($"update emp_info set emp_id = '{emp_id.Text}', emp_name='{emp_name.Text}', emp_email='{emp_email.Text}',emp_tel = '{emp_tel.Text}', emp_emer_tel='{emp_etel.Text}',emp_addr='{emp_addr.Text}', blood_type='{emp_bl.Text}', dept_id = '{emp_d.Text}' where emp_name = '{Passvalue2}'");

                cmd.ExecuteNonQuery();
                MessageBox.Show("수정되었습니다.");


                this.Close();
            }

            catch(Exception ex)
            {                
                MessageBox.Show(ex.ToString());
                MessageBox.Show("수정되었습니다.");
            }

        }
    }
}
