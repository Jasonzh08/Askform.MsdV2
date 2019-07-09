using Devart.Data.Zoho;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Msd_V2.Zoho
{
    public class GetZohoInfoAction
    {
        public static void Exec()
        {
            //const string sql = "SELECT * FROM Contacts";
            DataTable dt = new DataTable();
            dt = GetInfo("SELECT USERID,Name,Email FROM Users");
            List<ZohoUser> lzus = new List<Zoho.ZohoUser>();
            foreach (DataRow row in dt.Rows)
            {
                lzus.Add(new Zoho.ZohoUser()
                {
                    USERID = row["USERID"].ToString(),
                    NameEm = string.Format("{0}  ({1})", row["Name"], row["Email"])
                });
            }

            ZohoWrite.Write(@"D:\web\AskFormCustom\Zoho\Package\ZohoUser.json", JsonConvert.SerializeObject(lzus));

            dt = GetInfo("select* from Contacts where  CONTACTID = 123456789");
            List<string> columns = new List<string>();
            foreach (DataColumn col in dt.Columns)
                columns.Add(col.ColumnName);

            ZohoWrite.Write(@"D:\web\AskFormCustom\Zoho\Package\ZohoColumns.json", JsonConvert.SerializeObject(columns));
        }

        private static DataTable GetInfo(string sql)
        {
            const string connectionString = "security token=fe85458c694b2c124e2c161f8e2b834b;";
            DataTable dt = new DataTable();
            using (ZohoConnection connection = new ZohoConnection(connectionString))
            {
                connection.Open();
                using (ZohoCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using (ZohoDataAdapter adapter = new ZohoDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
