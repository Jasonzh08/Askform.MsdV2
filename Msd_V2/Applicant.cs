using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msd_V2
{
    public class Applicant
    {
        public string ApplicantId { get; set; }
        public string CadidateId { get; set; }
        public Profile Profile { get; set; }

        public List<ApplyJobSummaries> ApplyJobSummaries { get; set; }

    }
}
