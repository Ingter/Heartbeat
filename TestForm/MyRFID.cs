using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace TestForm
{
    public class MyRFID
    {
        private int g_rHandle, g_retCode;
        private byte g_Sec, g_SID;
        private byte[] g_pKey = new byte[6];
        private bool g_isConnected = false;

        public string strConn = "Server=192.168.0.173;" +
                               "Database=heartbeat;" +
                               "Uid=test;" +
                               "Pwd=1234;" +
                               "charset=utf8;";
        public MySqlConnection conn;
        // public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;
        public string snstr = "";

        public void Read_RFID_And_UPDATE_UserInfo()
        {
            //snstr = "";

            if (!g_isConnected)
            {
                RFID_conn();
                //MessageBox.Show("RFID 연결상태를 확인해주세요.");
            }
            else
            {
                //string time = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");


                byte[] TagLength = new byte[51];
                byte TagFound = 0;
                byte[] TagType = new byte[51];
                byte[] SN = new byte[451];
                int ctr;
                
                g_retCode = ACR120U.ACR120_ListTags(g_rHandle, ref TagFound, ref TagType[0], ref TagLength[0], ref SN[0]);

                if (g_retCode < 0) ;
                else
                {

                    for (ctr = 0; ctr < TagLength[0]; ctr++)
                    {

                        snstr = snstr + string.Format("{0:X2} ", SN[ctr]);
                        //userinfo.SnStr = snstr;

                    }
                    //userinfo.SnStr = snstr;
                    /*                    Detail_Page DP = new Detail_Page();
                                        DP.Passvalue = snstr;  // 전달자(Passvalue)를 통해서 dp페이지로 전달*/

                    conn = new MySqlConnection(strConn);
                    cmd = new MySqlCommand();
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = ($"select rfid from emp_info");
                    rdr = cmd.ExecuteReader();
                    string Emp_id = "";
                    string Emp_name = "";
                    string rfid = "";

                    while (rdr.Read())
                    {
                        rfid = rdr["rfid"] as string;
                        Console.WriteLine(rfid);

                    }
                    rdr.Close();


                    while (rdr.Read())
                    {

                        Emp_id = rdr["emp_id"].ToString();
                        Emp_name = rdr["emp_name"] as string;

                    }
                    rdr.Close();

                    //Emp_Update f1 = new Emp_Update(userinfo);


                    /*                    if (work == 1)
                                        {
                                            cmd.CommandText = $"insert into attendance_check (emp_id, emp_name, time) " +
                                                                 $"values('{Emp_id}','{Emp_name}','{time}')";
                                            cmd.ExecuteNonQuery();
                                        }*/

                    /*                    if (string.IsNullOrEmpty(snstr) == false)
                                        {
                                            timer1.Stop();
                                            Delay(2000);
                                            timer1.Start();
                                            return;
                                        }*/

                    conn.Close();

                }
            }
        }

        private void RFID_conn()
        {

            int ctr = 0;
            byte[] FirmwareVer = new byte[31];
            byte[] FirmwareVer1 = new byte[20];
            byte infolen = 0x00;
            string FirmStr;
            ACR120U.tReaderStatus ReaderStat = new ACR120U.tReaderStatus();



            g_rHandle = ACR120U.ACR120_Open(0);
            if (g_rHandle != 0)
            {
                //DisplayMessage("[X] " + ACR120U.GetErrMsg(g_rHandle));
            }
            else
            {

                //DisplayMessage("Connected to USB" + string.Format("{0}", 0 + 1));
                g_isConnected = true;

                //Get the DLL version the program is using
                g_retCode = ACR120U.ACR120_RequestDLLVersion(ref infolen, ref FirmwareVer[0]);
                if (g_retCode < 0) ;

                //DisplayMessage("[X] " + ACR120U.GetErrMsg(g_retCode));

                else
                {
                    FirmStr = "";
                    for (ctr = 0; ctr < Convert.ToInt16(infolen) - 1; ctr++)
                        FirmStr = FirmStr + char.ToString((char)(FirmwareVer[ctr]));
                    //DisplayMessage("DLL Version : " + FirmStr);
                }

                //Routine to get the firmware version.
                g_retCode = ACR120U.ACR120_Status(g_rHandle, ref FirmwareVer1[0], ref ReaderStat);
                if (g_retCode < 0) ;

                //DisplayMessage("[X] " + ACR120U.GetErrMsg(g_retCode));

                else
                {
                    FirmStr = "";
                    for (ctr = 0; ctr < Convert.ToInt16(infolen); ctr++)
                        if ((FirmwareVer1[ctr] != 0x00) && (FirmwareVer1[ctr] != 0xFF))
                            FirmStr = FirmStr + char.ToString((char)(FirmwareVer1[ctr]));
                    //DisplayMessage("Firmware Version : " + FirmStr);
                }

            }
        }
    }

    
}
