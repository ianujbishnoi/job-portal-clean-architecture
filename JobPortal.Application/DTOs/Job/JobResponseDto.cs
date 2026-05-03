using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTOs.Job
{
    public class JobResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public List<string> Skills { get; set; }
    }
}
