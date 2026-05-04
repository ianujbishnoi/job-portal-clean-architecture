using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Application
{
    public class UpdateApplicationStatusDto
    {
        public int ApplicationId { get; set; }
        public string Status { get; set; } // Viewed, Shortlisted, Rejected
    }

}
