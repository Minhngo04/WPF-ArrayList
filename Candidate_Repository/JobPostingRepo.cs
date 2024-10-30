using Candidate_BusinessObject;
using Candidate_DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Repository
{
    public class JobPostingRepo : IJobPostingRepo
    {
        public JobPosting GetJobPosting(string id) => JobPostingDAO.Instance.GetJobPosting(id);
        public List<JobPosting> GetJobPostings() => JobPostingDAO.Instance.GetJobPostings();
        public bool updateJobPosting(JobPosting jobPosting) => JobPostingDAO.Instance.UpdateJobPosting(jobPosting);
        public bool deleteJobPosting(string postingID) => JobPostingDAO.Instance.DeleteJobPosting(postingID);
        public bool addJobPosting(JobPosting jobPosting) => JobPostingDAO.Instance.AddJobPosting(jobPosting);
    }
}
