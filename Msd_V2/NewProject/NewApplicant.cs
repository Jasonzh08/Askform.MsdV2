using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2.NewProject
{
    public class NewApplicant
    {
        public string message { get; set; }

        public List<NewApplicants> applicants { get; set; }
    }

    public class NewApplicants
    {
        public string ElinkUrl { get; set; }

        public List<ApplicantProfile> ApplicantProfile { get; set; }
    }

    public class ApplicantProfile
    {
        public string PropertyName { get; set; }

        public string PropertyShortName { get; set; }

        public string Code { get; set; }

        public string Value { get; set; }
    }
}
