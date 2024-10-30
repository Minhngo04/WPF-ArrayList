using Candidate_BusinessObject;
using Candidate_DAOs;
using Candidate_Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Service
{
    public class JobPostingService : IJobPostingService
    {

        private readonly IJobPostingRepo jobPostingRepo;
        public JobPostingService()
        {
            jobPostingRepo = new JobPostingRepo();
        }
        public List<JobPosting> GetJobPostings()
        {
            return jobPostingRepo.GetJobPostings();
        }

        public JobPosting GetJobPosting(string id)
        {
            return jobPostingRepo.GetJobPosting(id);
        }

        public bool updateJobPosting(JobPosting jobPosting)
        {
            return jobPostingRepo.updateJobPosting(jobPosting);
        }
        public bool deleteJobPosting(string postingID)
        {
            return jobPostingRepo.deleteJobPosting(postingID);
        }
        public bool addJobPosting(JobPosting jobPosting)
        {
            return jobPostingRepo.addJobPosting(jobPosting);
        }
    }
}
