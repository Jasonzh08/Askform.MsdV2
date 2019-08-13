using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2.NewProject
{
    public class Apply
    {
        public string message { get; set; }

        public List<Applies> allApplys { get; set; }
    }

    public class Applies
    {
        public string personId { get; set; }

        public List<Profile> Applys { get; set; }
    }

    public class Profile
    {
        public string jobId { get; set; }

        public string personId { get; set; }

    }
}
