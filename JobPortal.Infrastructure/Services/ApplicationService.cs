using JobPortal.Application.DTOs.Application;
using JobPortal.Application.Interfaces;
using JobPortal.Domain.Entities;
using JobPortal.Domain.Enums;
using JobPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Infrastructure.Services;

public class ApplicationService : IApplicationService
{
    private readonly ApplicationDbContext _context;

    public ApplicationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationResponseDto> ApplyToJob(ApplyJobDto dto, int userId)
    {
        // 🔴 1. Validate Job Exists
        var job = await _context.Jobs
            .FirstOrDefaultAsync(j => j.Id == dto.JobId && !j.IsDeleted);

        if (job == null)
            throw new Exception("Job not found");

        // 🔴 2. Check Expiry
        if (job.ExpiryDate < DateTime.UtcNow)
            throw new Exception("Job has expired");

        // 🔴 3. Prevent Duplicate Apply (Code-level check)
        var alreadyApplied = await _context.Applications
            .AnyAsync(a => a.UserId == userId && a.JobId == dto.JobId);

        if (alreadyApplied)
            throw new Exception("You have already applied to this job");

        // 🔴 4. Create Application
        var application = new JobApplication
        {
            UserId = userId,
            JobId = dto.JobId,
            CurrentStatus = ApplicationStatus.Applied,
            ResumeUrl = dto.ResumeUrl,
            CreatedAt = DateTime.UtcNow
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        // 🔴 5. Add Status History
        var history = new ApplicationStatusHistory
        {
            ApplicationId = application.Id,
            Status = ApplicationStatus.Applied,
            ChangedAt = DateTime.UtcNow
        };

        _context.ApplicationStatusHistories.Add(history);
        await _context.SaveChangesAsync();

        return new ApplicationResponseDto
        {
            ApplicationId = application.Id,
            Status = application.CurrentStatus.ToString(),
            AppliedAt = application.CreatedAt
        };
    }

    public async Task<List<MyApplicationDto>> GetMyApplications(int userId)
    {
        return await _context.Applications
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new MyApplicationDto
            {
                JobId = a.JobId,
                JobTitle = a.Job.Title,
                Status = a.CurrentStatus.ToString(),
                AppliedAt = a.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<ApplicantDto>> GetApplicants(int jobId, int recruiterId)
    {
        // Ensure recruiter owns this job
        var job = await _context.Jobs
            .FirstOrDefaultAsync(j => j.Id == jobId && j.RecruiterId == recruiterId);

        if (job == null)
            throw new Exception("Unauthorized access");

        return await _context.Applications
            .Where(a => a.JobId == jobId)
            .Select(a => new ApplicantDto
            {
                UserId = a.UserId,
                UserName = a.User.Name,
                Status = a.CurrentStatus.ToString(),
                AppliedAt = a.CreatedAt
            })
            .ToListAsync();
    }
}