using Devart.Data.Zoho;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;

namespace Msd_V2.Zoho
{
    public class ToZohoAction
    {
        public static void Exec(bool isInsert = true)
        {
            string sqls = File.ReadAllText(@"D:\web\AskFormCustom\Zoho\Package\SQL.json");
            if (sqls.Trim().Length == 0) return;
            List<Sqls> ls = JsonConvert.DeserializeObject<List<Sqls>>(sqls);
            if (ls.Count == 0) return;

            //已执行的拥有者ID
            List<string> Owns = new List<string>();

            if (isInsert)
            {
                //所有添加语句
                var inserts = ls.FindAll(x => x.Email == string.Empty);
                inserts.ForEach(x =>
                {
                    try
                    {
                        ToZoho(x.SQL);

                        //移除已执行的新增语句
                        ls.Remove(x);
                    }
                    catch (Exception ex) { }
                });

                //所有修改语句
                ZohoWrite.Write(@"D:\web\AskFormCustom\Zoho\Package\SQL.json", JsonConvert.SerializeObject(ls));
                return;
            }
            else
            {
                //所有修改语句
                var updates = ls.FindAll(x => x.Email != string.Empty);

                foreach (var u in updates)
                {
                    if (!Owns.Contains(u.OwnID))
                    {
                        var dt = GetDtByOwnID(u.OwnID);

                        if (dt.Rows.Count == 0) continue;

                        //当前用户要有更新语句
                        var ownUpdates = updates.FindAll(n => n.OwnID == u.OwnID);

                        foreach (var o in ownUpdates)
                        {
                            DataRow[] drs = dt.Select(string.Format("Email='{0}'", o.Email));
                            if (drs.Length != 0)
                            {
                                string N_CONTACTID = drs[0]["CONTACTID"].ToString();
                                if (N_CONTACTID.Length > 0 && o.SQL.IndexOf("N_CONTACTID") >= 0)
                                {
                                    string sql = o.SQL.Replace("N_CONTACTID", N_CONTACTID);
                                    ToZoho(sql);
                                    Console.WriteLine("Zoho更新了一条数据");
                                }
                            }
                        }
                        Owns.Add(u.OwnID);
                    }
                }
            }
        }

        public static DataTable GetDtByOwnID(string OwnID)
        {
            const string connectionString = "security token=fe85458c694b2c124e2c161f8e2b834b;";
            //const string sql = "SELECT * FROM Contacts";
            DataTable dt = new DataTable();
            using (ZohoConnection connection = new ZohoConnection(connectionString))
            {
                connection.Open();
                using (ZohoCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"select CONTACTID,Email from Contacts where SMOWNERID=" + OwnID.Replace("\"", "");
                    using (ZohoDataAdapter adapter = new ZohoDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static int ToZoho(string sql)
        {
            const string connectionString = "security token=fe85458c694b2c124e2c161f8e2b834b;";
            int result = 0;
            using (ZohoConnection connection = new ZohoConnection(connectionString))
            {
                connection.Open();
                using (ZohoCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    result = command.ExecuteNonQuery();
                }
            }
            return result;
        }
    }
}
