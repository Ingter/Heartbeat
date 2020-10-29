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
	public partial class RFID : Form
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
		private string dp_value2;
		public string Passvalue_id
		{
			get { return this.dp_value2; }
			set { this.dp_value2 = value; }  // 다른폼에서 전달받은 값을 쓰기

		}
		
		public RFID()
		{
			InitializeComponent();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void DisplayMessage(string Message)
		{
			lbMessage.Items.Add(Message);
			lbMessage.SelectedIndex = lbMessage.Items.Count - 1;
		}

		private void RFID_Load(object sender, EventArgs e)
		{
			DisplayMessage("Program Ready");
			label2.Text = Passvalue_id;


			timer1.Interval = 1000;
			timer1.Start();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			conn = new MySqlConnection(strConn);
			cmd = new MySqlCommand();
			conn.Open();
			cmd.Connection = conn;
			cmd.CommandText = ($"select rfid from emp_info ");
			rdr = cmd.ExecuteReader();
			int work = 0;
			while (rdr.Read())
			{

				string rfid = rdr["rfid"] as string;
				if (rfid == label1.Text)
					work = 1;
			}
			rdr.Close();
			if (work==1)
			{
				cmd.CommandText = $"insert into rfid (rfid, emp_id,time) values " +
								  $"('{label1.Text}','{label2.Text}','{label3.Text}')";
				cmd.ExecuteNonQuery();


				MessageBox.Show("출근 완료되었습니다.");
				this.Close();
			}
			else
			{
				MessageBox.Show("없는 rfid 입니다.");
				this.Close();
			}

		}

        private void button5_Click(object sender, EventArgs e)
        {
			
			byte portState = 0x00;
			portState = Convert.ToByte(4);
			g_retCode = ACR120U.ACR120_WriteUserPort(g_rHandle, portState);
			Delay(250);
			portState = Convert.ToByte(64);
			g_retCode = ACR120U.ACR120_WriteUserPort(g_rHandle, portState);
		}

        private void button1_Click(object sender, EventArgs e)
		{

			{

				//=====================================================================
				// This function opens the port(connection) to ACR120 reader
				//=====================================================================

				// Variable declarations
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

					DisplayMessage("[X] " + ACR120U.GetErrMsg(g_rHandle));

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
		}

		private void button2_Click(object sender, EventArgs e)
		{

			{
				//=====================================================================
				// This function list the serial number of all tags within the
				// readable antenna range
				//=====================================================================

				//Variable Declarations
				byte[] TagLength = new byte[51];
				byte TagFound = 0;
				byte[] TagType = new byte[51];
				byte[] SN = new byte[451];
				byte[] byte2 = new byte[451];
				byte[] byte3 = new byte[451];
				byte[] byte4 = new byte[451];
				int ctr, ctr1, indx;
				string snstr;


				g_retCode = ACR120U.ACR120_ListTags(g_rHandle, ref TagFound, ref TagType[0], ref TagLength[0], ref SN[0]);
/*				g_retCode = ACR120U.ACR120_DirectSend(g_rHandle, TagFound, ref byte2[0],
												   ref byte3[0], ref byte4[0], 10);*/
				//DisplayMessage("1:" + String.Format("{0}", g_retCode) + "2:" + String.Format("{0}", TagFound) + "3:" + String.Format("{0}", byte2) + "4:" + String.Format("{0}", byte3) + "5:" + String.Format("{0}", byte4));
				
			/*	public static extern int ACR120_ListTags(int hReader, ref byte pNumTagFound, ref byte pTagType,
										 ref byte pTagLength, ref byte pSN);*/
				
				if (g_retCode < 0)
				{

					//DisplayMessage("[X] " + ACR120U.GetErrMsg(g_retCode));
					DisplayMessage(ACR120U.GetErrMsg(g_retCode));
				}
				else
				{

					DisplayMessage("List Tag Success");
					DisplayMessage("Tag Found: " + String.Format("{0}", TagFound));
					//DisplayMessage()

					// Parse the serial number array
					snstr = "";
					for (ctr = 0; ctr < TagLength[0]; ctr++)
					{

						snstr = snstr + string.Format("{0:X2} ", SN[ctr]);

					}
					
					/*					for (ctr1 = 0; ctr1 < TagFound; ctr1++)
										{
											indx = ctr1 * 10;
											snstr = "";
											for (ctr = indx; ctr < (TagLength[ctr1] + indx); ctr++)
											{

												snstr = snstr + string.Format("{0:X2} ", SN[ctr]);

											}

											DisplayMessage("Tag(" + string.Format("{0}", ctr1) + ") : " + snstr + " ( " + ACR120U.GetTagType1(TagType[ctr1]) + " )");
											label1.Text = snstr;
										}*/
					label1.Text = snstr;
				}

			}
		}

        private void timer1_Tick(object sender, EventArgs e)
        {
			{
				string time = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
				label3.Text = time;

				byte[] TagLength = new byte[51];
				byte TagFound = 0;
				byte[] TagType = new byte[51];
				byte[] SN = new byte[451];
				int ctr;
				string snstr;
				g_retCode = ACR120U.ACR120_ListTags(g_rHandle, ref TagFound, ref TagType[0], ref TagLength[0], ref SN[0]);

				if (g_retCode < 0)
				{
					DisplayMessage(ACR120U.GetErrMsg(g_retCode));
				}
				else
				{

					DisplayMessage("List Tag Success");
					DisplayMessage("Tag Found: " + String.Format("{0}", TagFound));

					// Parse the serial number array
					snstr = "";
					for (ctr = 0; ctr < TagLength[0]; ctr++)
					{

						snstr = snstr + string.Format("{0:X2} ", SN[ctr]);

					}
					label1.Text = snstr;
					conn = new MySqlConnection(strConn);
					cmd = new MySqlCommand();
					conn.Open();
					cmd.Connection = conn;
					cmd.CommandText = ($"select rfid from emp_info ");
					rdr = cmd.ExecuteReader();
					int work = 0;
					while (rdr.Read())
					{
						string rfid = rdr["rfid"] as string;
						if (rfid == snstr)
							work = 1;
					}
					rdr.Close();
					if (work == 1)
					{
						cmd.CommandText = $"insert into rfid (rfid, emp_id,time) values " +
										  $"('{snstr}','{Passvalue_id}','{time}')";
						cmd.ExecuteNonQuery();

					}

					if (string.IsNullOrEmpty(snstr) == false)
                    {
						timer1.Stop();
						Delay(2000);
						timer1.Start();
						return;
                    }
					conn.Close();
				}
			}
		}

        private void button4_Click(object sender, EventArgs e)
        {
			conn = new MySqlConnection(strConn);
			cmd = new MySqlCommand();
			conn.Open();
			cmd.Connection = conn;

			cmd.CommandText = ($"select rfid from emp_info ");
			rdr = cmd.ExecuteReader();

			while (rdr.Read())
			{

				string rfid = rdr["rfid"] as string;


				string[] emp_info = new string[] {rfid };

				if (label1.Text == rfid)
                {
					cmd.CommandText = $"insert into rfid (rfid, emp_id) values " +
				  $"('{label1.Text}','{label2.Text}')";

				}
                else
                {

                }
			}




			


			/*			try
						{
							cmd.CommandText = $"insert into rfid (rfid, emp_id) values " +
											  $"('{label1.Text}','{label2.Text}')";




							cmd.ExecuteNonQuery();
							MessageBox.Show("rfid 등록 완료되었습니다.");
							this.Close();
						}

						catch (MySqlException)
						{
							MessageBox.Show("중복된 rfid 입니다.");
							this.Close();
						}*/
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
