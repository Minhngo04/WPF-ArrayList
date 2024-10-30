using Candidate_BusinessObject;
using Candidate_DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Service
{
    public interface IJobPostingService
    {
        public List<JobPosting> GetJobPostings();

        public JobPosting GetJobPosting(string id);

        public bool updateJobPosting(JobPosting jobPosting) => JobPostingDAO.Instance.UpdateJobPosting(jobPosting);
        public bool deleteJobPosting(string postingID) => JobPostingDAO.Instance.DeleteJobPosting(postingID);
        public bool addJobPosting(JobPosting jobPosting) => JobPostingDAO.Instance.AddJobPosting(jobPosting);
    }
}
