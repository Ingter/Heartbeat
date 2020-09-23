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
    public partial class Regis_Page : Form
    {

        string strConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
    "(HOST=192.168.0.22)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=heartbeat;Password=heartbeat;";

        OracleConnection conn;
        OracleCommand cmd;


        public Regis_Page()
        {
            InitializeComponent();
        }

        private void Regis_Page_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn = new OracleConnection(strConn);
            cmd = new OracleCommand();
            conn.Open();
            cmd.Connection = conn;

            try
            {
                cmd.CommandText = $"insert into manager_info (manager_id, manager_pw, manager_name, " +
                                  $"manager_email, manager_pnumber) values " +
                                  $"('{textBox1.Text}','{textBox3.Text}','{textBox2.Text}','{textBox4.Text}','{textBox5.Text}')";
                cmd.ExecuteNonQuery();
                MessageBox.Show("관리자 등록 완료되었습니다.");
                this.Close();
            }

            catch
            {
                MessageBox.Show("중복된 아이디, 비밀빈호 입니다.");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
