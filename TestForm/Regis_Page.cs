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
    public partial class Regis_Page : Form
    {

        public string strConn = "Server=192.168.0.173;" +
                                "Database=heartbeat;" +
                                "Uid=test;" +
                                "Pwd=1234;" +
                                "charset=utf8;";

        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;
        public int i; //매니저 등급 (1-최종/ 2-부)

        public Regis_Page()
        {
            InitializeComponent();
        }

        private void Regis_Page_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;


            try
            {
                cmd.CommandText = $"insert into manager_info (manager_id, manager_pw, manager_name, " +
                                  $"manager_email, manager_tel, manager_grade) values " +
                                  $"('{textBox1.Text}','{textBox3.Text}','{textBox2.Text}','{textBox4.Text}','{textBox5.Text}',{i})";
                cmd.ExecuteNonQuery();
                MessageBox.Show("관리자 등록 완료되었습니다.");
                this.Close();
            }
            
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("중복된 아이디, 비밀빈호 입니다.");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            i = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            i = 2;
        }
    }
}
