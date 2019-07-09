using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2
{
    public class ErrorObj
    {
        public DateTime createDate { get; set; } = DateTime.Now;
        public long ID { get; set; }
        public string UserName { get; set; }
        public string ErrorMsg { get; set; }
    }
}
