using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForm
{
    class LoginHandler
    {

        public string strConn = "Server=192.168.0.22;" +
                                "Database=heartbeat;" +
                                "Uid=heartbeat;" +
                                "Pwd=1234;" +
                                "charset=utf8;";

        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;

        public bool LoginCheck(string id, string password)
        {
            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = "select * from manager_info";
            rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {
                string Man_id = rdr["manager_id"] as string;

                string Man_pw = rdr["manager_pw"] as string;


                if (id.Equals(Man_id) && password.Equals(Man_pw)) // 테스트용 아이디와 비밀번호 입니다.
                {
                    return true;
                }
            }
            return false;
        }
    }
}
