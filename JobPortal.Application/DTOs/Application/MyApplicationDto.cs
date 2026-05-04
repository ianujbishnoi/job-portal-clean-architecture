using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Application
{
    public class MyApplicationDto
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string Status { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
