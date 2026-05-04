using JobPortal.Application.DTOs.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationResponseDto> ApplyToJob(ApplyJobDto dto, int userId);
        Task<List<MyApplicationDto>> GetMyApplications(int userId);
        Task<List<ApplicantDto>> GetApplicants(int jobId, int recruiterId);
        Task UpdateStatus(UpdateApplicationStatusDto dto, int recruiterId);
    }
}
