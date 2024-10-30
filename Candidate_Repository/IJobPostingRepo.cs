using Candidate_BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Repository
{
    public interface IJobPostingRepo
    {
        public bool updateJobPosting(JobPosting jobPosting);
        public bool deleteJobPosting(string postingID);
        public bool addJobPosting(JobPosting jobPosting);
        public JobPosting GetJobPosting(string id);
        public List<JobPosting> GetJobPostings();
    }
}
