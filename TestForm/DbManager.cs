using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace TestForm
{
    class DbManager
    {

        public string strConn = "Server=192.168.0.173;" +
                               "Database=heartbeat;" +
                               "Uid=test;" +
                               "Pwd=1234;"
                               ;

        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;

        public bool LoginCheck(string id, string password, out string priority)
        {
            priority = "-1";
            bool rtnFlg = false;
            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = "select * from manager_info where manager_id = '" + id
                                                       + "' and manager_pw='" + password + "'";
            rdr = cmd.ExecuteReader();

            if (rdr.Read() == true)
            {
                string Man_id = rdr["manager_id"] as string;

                string Man_pw = rdr["manager_pw"] as string;

                priority = rdr["manager_grade"].ToString();

                rtnFlg = true;
            }
            else
            {
                rtnFlg = false;
            }

            return rtnFlg;
        }
    }
}
