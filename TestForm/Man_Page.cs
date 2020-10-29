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
        private int g_rHandle, g_retCode;
        private byte g_Sec, g_SID;
        private byte[] g_pKey = new byte[6];
        private bool g_isConnected = false;

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
/*            byte portState = 0x00;
            portState = Convert.ToByte(4);
            g_retCode = ACR120U.ACR120_WriteUserPort(g_rHandle, portState);
            Delay(250);
            portState = Convert.ToByte(64);
            g_retCode = ACR120U.ACR120_WriteUserPort(g_rHandle, portState);*/
            label2.Text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
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

            MessageBox.Show(dataGridView1.SelectedRows[0].Cells[0].FormattedValue.ToString());

            string name = dataGridView1.SelectedRows[0].Cells[1].FormattedValue.ToString();
            string id = dataGridView1.SelectedRows[0].Cells[0].FormattedValue.ToString();

            Detail_Page DP = new Detail_Page();
            DP.Passvalue = name;  // 전달자(Passvalue)를 통해서 dp페이지로 전달
            DP.Passvalues_id = id;
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
            
            RFID_conn();
            timer1.Interval = 2000;
            timer1.Start();
        }

        private void RFID_conn()
        {

            int ctr = 0;
            byte[] FirmwareVer = new byte[31];
            byte[] FirmwareVer1 = new byte[20];
            byte infolen = 0x00;
            string FirmStr;
            ACR120U.tReaderStatus ReaderStat = new ACR120U.tReaderStatus();

            if (g_isConnected)
            {
                DisplayMessage("Device is already connected.");
                return;

            }

            g_rHandle = ACR120U.ACR120_Open(0);
            if (g_rHandle != 0)
            {
                DisplayMessage("[X] " + ACR120U.GetErrMsg(g_rHandle));
            }
            else
            {

                DisplayMessage("Connected to USB" + string.Format("{0}", 0 + 1));
                g_isConnected = true;

                //Get the DLL version the program is using
                g_retCode = ACR120U.ACR120_RequestDLLVersion(ref infolen, ref FirmwareVer[0]);
                if (g_retCode < 0)

                    DisplayMessage("[X] " + ACR120U.GetErrMsg(g_retCode));

                else
                {
                    FirmStr = "";
                    for (ctr = 0; ctr < Convert.ToInt16(infolen) - 1; ctr++)
                        FirmStr = FirmStr + char.ToString((char)(FirmwareVer[ctr]));
                    DisplayMessage("DLL Version : " + FirmStr);
                }

                //Routine to get the firmware version.
                g_retCode = ACR120U.ACR120_Status(g_rHandle, ref FirmwareVer1[0], ref ReaderStat);
                if (g_retCode < 0)

                    DisplayMessage("[X] " + ACR120U.GetErrMsg(g_retCode));

                else
                {
                    FirmStr = "";
                    for (ctr = 0; ctr < Convert.ToInt16(infolen); ctr++)
                        if ((FirmwareVer1[ctr] != 0x00) && (FirmwareVer1[ctr] != 0xFF))
                            FirmStr = FirmStr + char.ToString((char)(FirmwareVer1[ctr]));
                    DisplayMessage("Firmware Version : " + FirmStr);
                }

            }
        }
        private void DisplayMessage(string Message)
        {
            RFID.Items.Add(Message);
            RFID.SelectedIndex = RFID.Items.Count - 1;
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
            emp_Regis f1 = new emp_Regis();
            f1.ShowDialog();
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
            label2.Text= System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }


    }
}
