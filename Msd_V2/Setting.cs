using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2
{
    public class Setting
    {
        public string ConstString { get { return "Bearer"; } }
        public string Tenant_ID { get { return "102854"; } }
        public string AccessToken { get { return "22645fc78af0436f85b15c56e90ee655"; } }

        public int IntervalSecond { get { return 300000; } } 
        public string ConnectionString { get { return "Data Source=121.40.25.174;Initial Catalog=Askform2.0;uid=askformdb;pwd=Passw0rd@askform;"; } }

        public long FormID { get { return 8174480001; } }
        public long FormApplicationID { get { return 7331550001; } }
        public long CompanyID { get { return 270829; } }

        public long FormPageID { get { return 0; } }

        #region 字段编号
        public long ApplicantIdFieldID { get { return 68354260001; } }
        public long NameFieldID { get { return 68354270001; } }
        public long CadidateIdFieldID { get { return 68354280001; } }
        public long EmailFieldID { get { return 68354290001; } }
        public long ReportFieldID { get { return 68354300001; } }
        public long ScoreFieldID { get { return 68354310001; } }
        public long JobIdFieldID { get { return 68354320001; } }
        public long JobIdsFieldID { get { return 68403680001; } }

        public long NormsetIdFieldID { get { return 68505380001; } }

        #endregion
    }
}
