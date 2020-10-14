using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace TestForm
{

    public partial class Man_Page : Form
    {

        public string strConn = "Server=192.168.0.31;" +
                               "Database=test;" +
                               "Uid=test;" +
                               "Pwd=1234;" +
                               "charset=utf8;";

        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;

        Login login;
        public Man_Page()
        {
            InitializeComponent();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentRow.Index;

            string id = dataGridView1.SelectedRows[0].Cells[0].FormattedValue.ToString();

            //상세 정보 페이지에 
            Man_Page mp = this;
            Detail_Page DP = new Detail_Page(mp);
            DP.Passvalue = id;  // 전달자(Passvalue)를 통해서 dp페이지로 전달
            DP.ShowDialog(this);

        }

        private void Man_Page_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Login Li = new Login();
            Li.loginEventHandler += new EventHandler(LoginSuccess);
            switch (Li.ShowDialog())
            {
                case DialogResult.OK:
                    Li.Close();
                    break;

                case DialogResult.Cancel:
                    Dispose();
                    break;
            }

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = ("select * from emp_info");
            rdr = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();

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



                string[] emp_info = new string[] { Emp_id, Emp_name, Emp_email,
                                                    Emp_tel, Emp_addr, Emp_emer_tel, Blood_type, dept_id };

                dataGridView1.Rows.Add(emp_info);

            }


        }
        private void LoginSuccess(string userName)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;




            if (comboBox1.SelectedIndex == 0)
            {
                cmd.CommandText = ("select * from emp_info where dept_id = '1'");
                rdr = cmd.ExecuteReader();
                //StringBuilder sb = new StringBuilder();
                dataGridView1.Rows.Clear();


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

                    dataGridView1.Rows.Add(emp_info);

                }


            }

            else if (comboBox1.SelectedIndex == 1)
            {
                cmd.CommandText = ("select * from emp_info where dept_id = '2'");
                rdr = cmd.ExecuteReader();
                //StringBuilder sb = new StringBuilder();
                dataGridView1.Rows.Clear();

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

                    dataGridView1.Rows.Add(emp_info);


                }

            }
            else if (comboBox1.SelectedIndex == 2)
            {
                cmd.CommandText = ("select * from emp_info;");
                rdr = cmd.ExecuteReader();
                //StringBuilder sb = new StringBuilder();
                dataGridView1.Rows.Clear();

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

                    dataGridView1.Rows.Add(emp_info);

                }

            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Man_Page mp = this;
            emp_Regis f1 = new emp_Regis(mp);
            f1.ShowDialog(this);

/*            Man_Page mp = this;
            Detail_Page DP = new Detail_Page(mp);
            DP.Passvalue = id;  // 전달자(Passvalue)를 통해서 dp페이지로 전달
            DP.ShowDialog(this);*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = ("select * from emp_info;");
            rdr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();

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

                dataGridView1.Rows.Add(emp_info);
            }
        }

        private void Man_Regi_Click(object sender, EventArgs e)
        {
            Regis_Page RP = new Regis_Page();
            RP.ShowDialog();
        }
    }
}
