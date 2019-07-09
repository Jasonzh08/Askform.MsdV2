using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2.Zoho
{
    public class ZohoWrite
    {
        private static object objlock = new object();
        public static void Write(string Path,string Content) {
            lock (objlock)
            {
                using (StreamWriter sw = new StreamWriter(Path, false))
                {
                    sw.Write(Content);
                }
            }
        }
    }
}
