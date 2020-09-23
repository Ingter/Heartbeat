using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForm
{
    class LoginHandler
    {
        string strConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
"(HOST=192.168.0.22)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=heartbeat;Password=heartbeat;";

        OracleConnection conn;
        OracleCommand cmd;

        public bool LoginCheck(string id, string password)
        {
            conn = new OracleConnection(strConn);
            cmd = new OracleCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = "select * from manager_info";
            OracleDataReader rdr = cmd.ExecuteReader();


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
