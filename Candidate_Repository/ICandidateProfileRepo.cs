using Candidate_BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate_Repository
{
    public interface ICandidateProfileRepo
    {
        public List<CandidateProfile> GetCandidates();
        public CandidateProfile GetCandidateProfile(string id);

        public bool addCandidateProfile(CandidateProfile candidateProfile);

        public bool deleteCandidateProfile(string candidateID);
        public bool updateCandidateProfile(CandidateProfile candidateProfile);
    }
}

