using Candidate_BusinessObject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Candidate_DAOs
{
    public class CandidateProfileDAO
    {
        private List<JobPosting> jobPostings; // Changed to List for better type safety
        private ArrayList candidateProfiles;
        private static CandidateProfileDAO instance;
        private string filePath = "candidateProfiles.txt"; // File path for candidate profiles
        private string postingFilePath = "jobPostings.txt"; // File path for job postings

        public static CandidateProfileDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CandidateProfileDAO();
                }
                return instance;
            }
        }

        public CandidateProfileDAO()
        {
            candidateProfiles = new ArrayList();
            jobPostings = LoadJobPostings(); // Load job postings on initialization
            LoadDataFromFile(); // Load candidate profiles from file
        }

        private void LoadDataFromFile()
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines.Skip(1)) // Skip header line
                {
                    var data = line.Split('\t'); // Use tab as the delimiter
                    if (data.Length >= 6) // Ensure sufficient data
                    {
                        var candidateProfile = new CandidateProfile
                        {
                            CandidateId = data[0],
                            Fullname = data[1],
                            Birthday = DateTime.Parse(data[2]),
                            ProfileShortDescription = data[3],
                            ProfileUrl = data[4],
                            PostingId = data[5]
                        };
                        candidateProfiles.Add(candidateProfile);
                    }
                }
            }
        }

        private List<JobPosting> LoadJobPostings()
        {
            var postings = new List<JobPosting>();
            if (File.Exists(postingFilePath))
            {
                var lines = File.ReadAllLines(postingFilePath);
                foreach (var line in lines.Skip(1)) // Skip header line
                {
                    var data = line.Split('\t'); // Use tab as the delimiter
                    if (data.Length >= 2) // Ensure sufficient data
                    {
                        var jobPosting = new JobPosting
                        {
                            PostingId = data[0], // Assuming PostingId is the first column
                            JobPostingTitle = data[1] // Assuming JobTitle is the second column
                        };
                        postings.Add(jobPosting);
                    }
                }
            }
            return postings;
        }

        public CandidateProfile GetCandidateProfile(string id)
        {
            return candidateProfiles.Cast<CandidateProfile>().SingleOrDefault(c => c.CandidateId.Equals(id));
        }

        public bool AddCandidateProfile(CandidateProfile candidateProfile)
        {
            bool isSuccess = false;
            CandidateProfile candidate = GetCandidateProfile(candidateProfile.CandidateId);
            if (candidate == null)
            {
                candidateProfiles.Add(candidateProfile);
                SaveDataToFile(); // Save data to file when adding
                isSuccess = true;
            }
            return isSuccess;
        }

        public bool DeleteCandidateProfile(string candidateID)
        {
            bool isSuccess = false;
            CandidateProfile candidate = GetCandidateProfile(candidateID);
            if (candidate != null)
            {
                candidateProfiles.Remove(candidate);
                SaveDataToFile(); // Save data to file when deleting
                isSuccess = true;
            }
            return isSuccess;
        }

        public bool UpdateCandidateProfile(CandidateProfile candidateProfile)
        {
            bool isSuccess = false;
            CandidateProfile existingCandidate = GetCandidateProfile(candidateProfile.CandidateId);
            if (existingCandidate != null)
            {
                candidateProfiles.Remove(existingCandidate);
                candidateProfiles.Add(candidateProfile);
                SaveDataToFile(); // Save data to file when updating
                isSuccess = true;
            }
            return isSuccess;
        }

        private void SaveDataToFile()
        {
            var lines = candidateProfiles.Cast<CandidateProfile>()
                                         .Select(cp => $"{cp.CandidateId}\t{cp.Fullname}\t{cp.Birthday:yyyy-MM-dd HH:mm:ss.fff}\t{cp.ProfileShortDescription}\t{cp.ProfileUrl}\t{cp.PostingId}"); // Format line
            File.WriteAllLines(filePath, new[] { "CandidateId\tName\tDateOfBirth\tSkills\tFilePath\tPostingId" }.Concat(lines)); // Add header
        }

        public List<CandidateProfile> GetCandidatesWithPostings()
        {
            foreach (CandidateProfile candidate in candidateProfiles)
            {
                // Find JobPosting based on PostingId
                var posting = jobPostings.SingleOrDefault(p => p.PostingId.ToString() == candidate.PostingId);
                candidate.Posting = posting; // Assuming Posting is a property of CandidateProfile
            }

            return candidateProfiles.Cast<CandidateProfile>().ToList();
        }
    }
}
