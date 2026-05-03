using JobPortal.Domain.Common;
using JobPortal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Entities
{
    public class ApplicationStatusHistory : BaseEntity
    {
        public int ApplicationId { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime ChangedAt { get; set; }

        // Navigation
        public JobApplication Application { get; set; }
    }
}
