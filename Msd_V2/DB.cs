using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2
{
    public class DB
    {
        private static readonly string ConnectionString = new Setting().ConnectionString;


        public static object ExecuteScalar(string Sql)
        {
            object returnValue;
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                using (var Comm = new SqlCommand() { CommandText = Sql, Connection = cn, CommandType = System.Data.CommandType.Text })
                {
                    returnValue = Comm.ExecuteScalar();
                };
                cn.Close();
            }
            return returnValue;
        }


        public static object ExecuteScalar(string Sql, bool toCenter = false)
        {
            object returnValue;
            var commstr = toCenter ? ConnectionString.Replace("Askform2.0", "AskForm_Center") : ConnectionString;
            using (SqlConnection cn = new SqlConnection(commstr))
            {
                cn.Open();
                using (var Comm = new SqlCommand() { CommandText = Sql, Connection = cn, CommandType = System.Data.CommandType.Text })
                {
                    returnValue = Comm.ExecuteScalar();
                };
                cn.Close();
            }
            return returnValue;
        }


        public static DataTable ExecuteQuery(string Sql)
        {
            SqlDataAdapter da;
            DataTable dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand() { Connection = cn, CommandType = CommandType.Text, CommandTimeout = 1500, CommandText = Sql })
                {
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                cn.Close();
            }

            return dt;
        }

        public static Int64 ExecuteNonQuery(string Sql, bool toCenter = false)
        {
            var commstr = toCenter ? ConnectionString.Replace("Askform2.0", "AskForm_Center") : ConnectionString;
            using (SqlConnection cn = new SqlConnection(commstr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand() { Connection = cn, CommandType = CommandType.Text, CommandTimeout = 1500, CommandText = Sql })
                {
                    cmd.ExecuteNonQuery();
                }
                cn.Close();
            }
            return 1;
        }

    }
}
