using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Job
{
    public class CreateJobDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal SalaryRange { get; set; }
        public int ExperienceRequired { get; set; }
        public DateTime ExpiryDate { get; set; }
        public List<int> SkillIds { get; set; }
    }
}
