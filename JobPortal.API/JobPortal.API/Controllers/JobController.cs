using JobPortal.Application.DTOs.Job;
using JobPortal.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobPortal.API.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> CreateJob(CreateJobDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _jobService.CreateJob(dto, userId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> SearchJobs(string? search, int page = 1, int pageSize = 10)
        {
            var result = await _jobService.SearchJobs(search, page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(int id)
        {
            var job = await _jobService.GetJobDetail(id);

            if (job == null)
                return NotFound();

            return Ok(job);
        }
    }
}
