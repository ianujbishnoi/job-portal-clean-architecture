using JobPortal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JobPortal.Domain.Entities
{
    public class Job : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int RecruiterId { get; set; }

        public string Location { get; set; }
        public decimal SalaryRange { get; set; }
        public int ExperienceRequired { get; set; }

        public DateTime ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public User Recruiter { get; set; }
        public ICollection<JobSkill> JobSkills { get; set; }
        public ICollection<JobApplication> Applications { get; set; }
    }
}
