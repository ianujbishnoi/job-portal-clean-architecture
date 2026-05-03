using JobPortal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Entities
{
    public class UserSkill : BaseEntity
    {
        public int UserId { get; set; }
        public int SkillId { get; set; }

        public string Level { get; set; } // Beginner, Intermediate, Expert

        public User User { get; set; }
        public Skill Skill { get; set; }
    }
}
