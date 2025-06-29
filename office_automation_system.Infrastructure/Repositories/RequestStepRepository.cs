using office_automation_system.application.Contracts.Repositories;
using office_automation_system.domain.Entities;
using office_automation_system.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.Repositories
{
    public class RequestStepRepository : Repository<RequestStep>, IRequestStepRepository
    {
        public RequestStepRepository(ApplicationDbContext context) : base(context) { }




    }
}
