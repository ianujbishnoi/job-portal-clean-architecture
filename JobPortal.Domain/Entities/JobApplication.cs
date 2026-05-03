using JobPortal.Domain.Common;
using JobPortal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Entities
{
    public class JobApplication : BaseEntity
    {
        public int UserId { get; set; }
        public int JobId { get; set; }

        public ApplicationStatus CurrentStatus { get; set; }

        public string ResumeUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public User User { get; set; }
        public Job Job { get; set; }

        public ICollection<ApplicationStatusHistory> StatusHistory { get; set; }
    }
}
