using JobPortal.Application.DTOs.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Interfaces
{
    public interface IJobService
    {
        Task CreateJob(CreateJobDto dto, int recruiterId);
        Task<List<JobResponseDto>> SearchJobs(string? search, int page, int pageSize);
    }
}
