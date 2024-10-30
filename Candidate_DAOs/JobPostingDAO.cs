using Candidate_BusinessObject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Candidate_DAOs
{
    public class JobPostingDAO
    {
        private ArrayList jobPostings;
        private static JobPostingDAO instance;
        private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jobPostings.txt");

        public static JobPostingDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new JobPostingDAO();
                }
                return instance;
            }
        }

        public JobPostingDAO()
        {
            jobPostings = new ArrayList();
            LoadDataFromFile(); // Tải dữ liệu từ file khi khởi tạo
        }

        public List<JobPosting> GetJobPostings()
        {
            return jobPostings.Cast<JobPosting>().ToList();
        }

        public JobPosting GetJobPosting(string id)
        {
            return jobPostings.Cast<JobPosting>().SingleOrDefault(jp => jp.PostingId.Equals(id));
        }

        public bool AddJobPosting(JobPosting jobPosting)
        {
            bool isSuccess = false;
            JobPosting existingJobPosting = GetJobPosting(jobPosting.PostingId);
            if (existingJobPosting == null)
            {
                jobPostings.Add(jobPosting);
                SaveDataToFile(); // Lưu dữ liệu vào file sau khi thêm
                isSuccess = true;
            }
            return isSuccess;
        }

        public bool DeleteJobPosting(string postingID)
        {
            bool isSuccess = false;
            JobPosting jobPosting = GetJobPosting(postingID);

            if (jobPosting != null)
            {
                bool hasDependencies = CandidateProfileDAO.Instance.GetCandidatesWithPostings()
                    .Any(candidate => candidate.PostingId == postingID);
                if (!hasDependencies)
                {
                    jobPostings.Remove(jobPosting);
                    SaveDataToFile(); // Lưu dữ liệu vào file sau khi xóa
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public bool UpdateJobPosting(JobPosting jobPosting)
        {
            bool isSuccess = false;
            JobPosting existingJobPosting = GetJobPosting(jobPosting.PostingId);
            if (existingJobPosting != null)
            {
                jobPostings.Remove(existingJobPosting);
                jobPostings.Add(jobPosting);
                SaveDataToFile(); // Lưu dữ liệu vào file sau khi cập nhật
                isSuccess = true;
            }
            return isSuccess;
        }

        private void LoadDataFromFile()
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var data = line.Split('\t'); // Sử dụng tab làm dấu phân cách
                    if (data.Length >= 4)
                    {
                        var jobPosting = new JobPosting
                        {
                            PostingId = data[0],
                            JobPostingTitle = data[1],
                            Description = data[2],
                            PostedDate = DateTime.Parse(data[3]) // Đảm bảo định dạng ngày hợp lệ
                        };
                        jobPostings.Add(jobPosting);
                    }
                }
            }
        }

        private void SaveDataToFile()
        {
            var lines = jobPostings.Cast<JobPosting>()
                                   .Select(jp => $"{jp.PostingId}\t{jp.JobPostingTitle}\t{jp.Description}\t{jp.PostedDate:yyyy-MM-dd}"); // Định dạng ngày
            File.WriteAllLines(filePath, lines);
        }
    }
}
