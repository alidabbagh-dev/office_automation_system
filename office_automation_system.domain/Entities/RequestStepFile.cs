using Microsoft.AspNetCore.Http;
using office_automation_system.domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.domain.Entities
{
    public class RequestStepFile : BaseEntity
    {
        public string? Title { get; set; }
        public string? FileUrl {  get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public Guid? RequestStepId { get; set; }
        public RequestStep? RequestStep { get; set; }
       

    }
}
