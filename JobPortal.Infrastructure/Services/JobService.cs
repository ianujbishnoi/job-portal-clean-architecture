using JobPortal.Application.DTOs.Job;
using JobPortal.Application.Interfaces;
using JobPortal.Domain.Entities;
using JobPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Infrastructure.Services;

public class JobService : IJobService
{
    private readonly ApplicationDbContext _context;

    public JobService(ApplicationDbContext context)
    {
        _context = context;
    }

    // 🔹 CREATE JOB (Recruiter only)
    public async Task CreateJob(CreateJobDto dto, int recruiterId)
    {
        // Basic validation
        if (dto.ExpiryDate <= DateTime.UtcNow)
            throw new Exception("Expiry date must be in future");

        var job = new Job
        {
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,
            SalaryRange = dto.SalaryRange,
            ExperienceRequired = dto.ExperienceRequired,
            ExpiryDate = dto.ExpiryDate,
            RecruiterId = recruiterId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        // Add JobSkills (Many-to-Many)
        if (dto.SkillIds != null && dto.SkillIds.Any())
        {
            var jobSkills = dto.SkillIds.Select(skillId => new JobSkill
            {
                JobId = job.Id,
                SkillId = skillId
            }).ToList();

            _context.JobSkills.AddRange(jobSkills);
            await _context.SaveChangesAsync();
        }
    }

    // 🔹 SEARCH JOBS (with filters + pagination)
    public async Task<List<JobResponseDto>> SearchJobs(string? search, int page, int pageSize)
    {
        // Defensive coding
        if (page <= 0) page = 1;
        if (pageSize <= 0 || pageSize > 50) pageSize = 10;

        var query = _context.Jobs
            .Where(j => !j.IsDeleted)
            .AsQueryable();

        // Filter by title
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(j => j.Title.Contains(search));
        }

        var jobs = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(j => new JobResponseDto
            {
                Id = j.Id,
                Title = j.Title,
                Location = j.Location,
                Skills = j.JobSkills
                    .Select(js => js.Skill.Name)
                    .ToList()
            })
            .ToListAsync();

        return jobs;
    }

    // 🔹 GET JOB BY ID (used later for apply/matching)
    public async Task<JobResponseDto?> GetJobById(int jobId)
    {
        return await _context.Jobs
            .Where(j => j.Id == jobId && !j.IsDeleted)
            .Select(j => new JobResponseDto
            {
                Id = j.Id,
                Title = j.Title,
                Location = j.Location,
                Skills = j.JobSkills
                    .Select(js => js.Skill.Name)
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<JobDetailDto?> GetJobDetail(int jobId)
    {
        return await _context.Jobs
            .Where(j => j.Id == jobId && !j.IsDeleted)
            .Select(j => new JobDetailDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                Location = j.Location,
                ExpiryDate = j.ExpiryDate,
                Skills = j.JobSkills.Select(js => js.Skill.Name).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<JobMatchDto>> GetMatchedJobs(int userId, int page, int pageSize)
    {
        // 🔴 1. Get user skills
        var userSkills = await _context.UserSkills
            .Where(us => us.UserId == userId)
            .Select(us => us.SkillId)
            .ToListAsync();

        if (!userSkills.Any())
            return new List<JobMatchDto>();

        // 🔴 2. Get jobs with skills
        var jobs = await _context.Jobs
            .Where(j => !j.IsDeleted)
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(j => new
            {
                j.Id,
                j.Title,
                j.Location,
                JobSkills = j.JobSkills.Select(js => js.SkillId).ToList(),
                SkillNames = j.JobSkills.Select(js => js.Skill.Name).ToList()
            })
            .ToListAsync();

        // 🔴 3. Calculate match
        var result = jobs.Select(j =>
        {
            var matchCount = j.JobSkills.Intersect(userSkills).Count();
            var total = j.JobSkills.Count;

            int percentage = total == 0 ? 0 : (matchCount * 100) / total;

            return new JobMatchDto
            {
                JobId = j.Id,
                Title = j.Title,
                Location = j.Location,
                MatchPercentage = percentage,
                Skills = j.SkillNames
            };
        })
        .OrderByDescending(x => x.MatchPercentage) // 🔥 sort by best match
        .ToList();

        return result;
    }
}
