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

        
     
        public string strConn = "Server=192.168.0.173;" +
                               "Database=heartbeat;" +
                               "Uid=test;" +
                               "Pwd=1234;" +
                               "charset=utf8;";

        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;
        string a;

        Login ls;
        public UserInfo userInfo;
        public Man_Page()
        {
            InitializeComponent();
        }


        public Man_Page(UserInfo _uinfo, Login _ls)
        {
            InitializeComponent();
            userInfo = _uinfo;
            ls = _ls;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString("시간 : HH : mm : ss");


        }

        private string Form2_value;
        public string Passvalue
        {
            get { return this.Form2_value; }
            set { this.Form2_value = value; }  // 다른폼(Form1)에서 전달받은 값을 쓰기
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

        public void Man_Page_Load(object sender, EventArgs e)
        {

            label1.Text = Passvalue;

            comboBox_init();

            label2.Text = System.DateTime.  Now.ToString("hh:mm:ss");

            this.dataGridView1.Font = new Font("Malgun Gothic", 9, FontStyle.Bold);
            this.dataGridView1.DefaultCellStyle.Font = new Font("Gothic", 9, FontStyle.Regular);

            if (userInfo.priority.Equals("1"))
                Man_Regi.Enabled = true;
            else
                Man_Regi.Enabled = false;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;
             
            cmd.CommandText = ("select * from emp_info order by emp_id");
            rdr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();

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

                if(Man_Regi.Enabled == false)               //버튼 사용 불가 계정으로 진입 시 관리자 버튼 디자인
                {
                    Man_Regi.BackColor = Color.FromArgb(130,39,34);
                    Man_Regi.FlatAppearance.BorderColor = Color.FromArgb(130, 39, 34);
                }

                if (dept_id == "1")
                {
                    a = "포장팀";
                }

                else if (dept_id == "2")
                {
                    a = "검수팀";
                }

                string[] emp_info = new string[] { Emp_id, Emp_name, Emp_email,
                                                    Emp_tel, Emp_addr, Emp_emer_tel, Blood_type, a };

                dataGridView1.Rows.Add(emp_info);

                label2.Text = DateTime.Now.ToString("시간 : HH : mm : ss");
                timer1.Interval = 1000;
                timer1.Start();
            }


        }
        private void LoginSuccess(string userName)
        {

        }




        public void comboBox_init()
        {
            //comboBox1.Items.Add("검수팀");
            //comboBox1.Items.Add("포장팀");
            //comboBox1.Items.Add("전체");
            comboBox1.SelectedIndex = 2; 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;




            if (comboBox1.SelectedIndex == 0)
            {
                cmd.CommandText = ("select * from emp_info where dept_id = 1 order by emp_id");
                rdr = cmd.ExecuteReader();
                //StringBuilder sb = new StringBuilder();
                dataGridView1.Rows.Clear();


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

                    if (dept_id == "1")
                    {
                        a = "포장팀";
                    }

                    else if (dept_id == "2")
                    {
                        a = "검수팀";
                    }
                    string[] emp_info = new string[] { Emp_id, Emp_name, Emp_email, Emp_tel, Emp_addr, Emp_emer_tel, Blood_type, a };

                    dataGridView1.Rows.Add(emp_info);

                }


            }

            else if (comboBox1.SelectedIndex == 1)
            {
                cmd.CommandText = ("select * from emp_info where dept_id = 2 order by emp_id");
                rdr = cmd.ExecuteReader();
                //StringBuilder sb = new StringBuilder();
                dataGridView1.Rows.Clear();

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

                    if (dept_id == "1")
                    {
                        a = "포장팀";
                    }

                    else if (dept_id == "2")
                    {
                        a = "검수팀";
                    }
                    string[] emp_info = new string[] { Emp_id, Emp_name, Emp_email, Emp_tel, Emp_addr, Emp_emer_tel, Blood_type, a };

                    dataGridView1.Rows.Add(emp_info);


                }

            }
            else if (comboBox1.SelectedIndex == 2)
            {
                cmd.CommandText = ("select * from emp_info order by emp_id");
                rdr = cmd.ExecuteReader();
                //StringBuilder sb = new StringBuilder();
                dataGridView1.Rows.Clear();

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
                    
                    if (dept_id == "1")
                    {
                        a = "포장팀";
                    }

                    else if (dept_id == "2")
                    {
                        a = "검수팀";
                    }
                    string[] emp_info = new string[] { Emp_id, Emp_name, Emp_email, Emp_tel, Emp_addr, Emp_emer_tel, Blood_type, a };

                    dataGridView1.Rows.Add(emp_info);

                }

            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Man_Page mp = this;
            emp_Regis f1 = new emp_Regis(mp, userInfo);
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
                string Emp_id = rdr["emp_id"].ToString();

                string Emp_name = rdr["emp_name"] as string;

                string Emp_email = rdr["emp_email"] as string;

                string Emp_tel = rdr["emp_tel"] as string;

                string Emp_addr = rdr["emp_addr"] as string;

                string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                string Blood_type = rdr["blood_type"] as string;

                string dept_id = rdr["dept_id"].ToString();

                string[] emp_info = new string[] { Emp_id, Emp_name, Emp_email, Emp_tel, Emp_addr, Emp_emer_tel, Blood_type, dept_id };

                dataGridView1.Rows.Add(emp_info);
            }
        }

        private void Man_Regi_Click(object sender, EventArgs e)
        {
            Regis_Page RP = new Regis_Page();
            RP.ShowDialog();
        }

        private void Man_Page_FormClosing(object sender, FormClosingEventArgs e)
        {
            ls.Visible = true;
            ls.textBox1.Text = "";
            ls.textBox2.Text = "";
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Logout_Click(object sender, EventArgs e)
        {
            this.Close();

        }

    }
}
