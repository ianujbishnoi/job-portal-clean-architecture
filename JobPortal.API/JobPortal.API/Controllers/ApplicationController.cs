using System.Security.Claims;
using JobPortal.Application.DTOs.Application;
using JobPortal.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/applications")]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _service;

    public ApplicationController(IApplicationService service)
    {
        _service = service;
    }

    [HttpPost("apply")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> Apply(ApplyJobDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var result = await _service.ApplyToJob(dto, userId);

        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> MyApplications()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var result = await _service.GetMyApplications(userId);

        return Ok(result);
    }

    [HttpGet("{jobId}/applicants")]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> GetApplicants(int jobId)
    {
        var recruiterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var result = await _service.GetApplicants(jobId, recruiterId);

        return Ok(result);
    }

    [HttpPut("status")]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> UpdateStatus(UpdateApplicationStatusDto dto)
    {
        var recruiterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        await _service.UpdateStatus(dto, recruiterId);

        return Ok("Status updated");
    }
}