using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2
{

    public class ErrorLog
    {
        private static object objlock = new object();
        private static string Path = System.AppDomain.CurrentDomain.BaseDirectory + @"\ErrorLog.txt";

        public static void WriteLog(ErrorObj eo,string _path="")
        {
            lock (objlock)
            {
                if (string.IsNullOrEmpty(_path))
                {
                    _path = Path;
                }
                else {
                    _path=System.AppDomain.CurrentDomain.BaseDirectory + @"\"+_path;
                }

                using (StreamWriter sw = new StreamWriter(_path, true))
                {
                    sw.WriteLine("------------------------------------------------------");
                    sw.WriteLine("CreateDate:" + eo.createDate);
                    sw.WriteLine("Appid:"+eo.ID);
                    sw.WriteLine("UserName:" + eo.UserName);
                    sw.WriteLine("ErrorMsg:" + eo.ErrorMsg);
                    sw.WriteLine("------------------------------------------------------");
                    sw.WriteLine();
                }
            }
        }

    }
}
