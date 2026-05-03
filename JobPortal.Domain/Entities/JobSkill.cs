using JobPortal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Entities
{
    public class JobSkill : BaseEntity
    {
        public int JobId { get; set; }
        public int SkillId { get; set; }

        public Job Job { get; set; }
        public Skill Skill { get; set; }
    }
}
