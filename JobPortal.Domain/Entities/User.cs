using JobPortal.Domain.Common;
using JobPortal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JobPortal.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<UserSkill> UserSkills { get; set; }
        public ICollection<Job> JobsPosted { get; set; }
        public ICollection<JobApplication> Applications { get; set; }
    }
}
