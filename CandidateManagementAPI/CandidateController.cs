using Candidate_Service;
using Microsoft.AspNetCore.Mvc;

namespace CandidateManagementAPI
{
    [ApiController]
    [Route("[Controller]")]
    public class CandidateController : Controller
    {
        private ICandidateProfileService profileService;

        public CandidateController()
        {
            profileService = new CandidateProfileService();    

       }
        [HttpGet(Name = "GetCandidates")]
        public IActionResult GetAllCandidate()
        {
            return Ok(profileService.GetCandidates().ToList());
        }

    }
}
