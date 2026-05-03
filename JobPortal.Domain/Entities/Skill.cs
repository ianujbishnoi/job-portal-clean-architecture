using JobPortal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Entities
{

    public class Skill : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<UserSkill> UserSkills { get; set; }
        public ICollection<JobSkill> JobSkills { get; set; }
    }
}
