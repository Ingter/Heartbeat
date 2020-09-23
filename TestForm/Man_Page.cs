using Oracle.ManagedDataAccess.Client;
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

        string strConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
     "(HOST=192.168.0.22)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=heartbeat;Password=heartbeat;";

        OracleConnection conn;
        OracleCommand cmd;

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
            Detail_Page DP = new Detail_Page();
            DP.Show();
        }

        private void Man_Page_Load(object sender, EventArgs e)
        {
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

            conn = new OracleConnection(strConn);
            cmd = new OracleCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = ("select * from emp_detail");
            OracleDataReader rdr = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();

            while (rdr.Read())
            {
                string Emp_id = rdr["emp_id"] as string;

                string Emp_name = rdr["emp_name"] as string;

                string Body_temp = rdr["body_temp"].ToString();

                string Heart_rate = rdr["heart_rate"].ToString();

                string[] emp_detail_data = new string[] { Emp_id, Emp_name, Body_temp, Heart_rate };

                dataGridView1.Rows.Add(emp_detail_data);

            }


        }
        private void LoginSuccess(string userName)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            conn = new OracleConnection(strConn);
            cmd = new OracleCommand();
            conn.Open();
            cmd.Connection = conn;




            if (comboBox1.SelectedIndex == 0)
            {
                cmd.CommandText = ("select * from emp_detail where dept_name = '검수팀'");
                OracleDataReader rdr = cmd.ExecuteReader();
                //StringBuilder sb = new StringBuilder();
                dataGridView1.Rows.Clear();
                while (rdr.Read())
                {
                    string Emp_id = rdr["emp_id"] as string;

                    string Emp_name = rdr["emp_name"] as string;

                    string Body_temp = rdr["body_temp"].ToString();

                    string Heart_rate = rdr["heart_rate"].ToString();

                    string[] emp_detail_data = new string[] { Emp_id, Emp_name, Body_temp, Heart_rate };

                    dataGridView1.Rows.Add(emp_detail_data);
                }
            }

            else if (comboBox1.SelectedIndex == 1)
            {
                cmd.CommandText = ("select * from emp_detail where dept_name = '포장팀'");
                OracleDataReader rdr = cmd.ExecuteReader();
                //StringBuilder sb = new StringBuilder();
                dataGridView1.Rows.Clear();
                while (rdr.Read())
                {
                    string emp_id = rdr["emp_id"] as string;

                    string emp_name = rdr["emp_name"] as string;

                    string body_temp = rdr["body_temp"].ToString();

                    string heart_rate = rdr["heart_rate"].ToString();

                    string[] emp_detail_data = new string[] { emp_id, emp_name, body_temp, heart_rate };

                    dataGridView1.Rows.Add(emp_detail_data);


                }
            }
            conn.Close();
        }
    }
}
