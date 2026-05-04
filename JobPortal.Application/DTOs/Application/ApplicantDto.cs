using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Application
{
    public class ApplicantDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
