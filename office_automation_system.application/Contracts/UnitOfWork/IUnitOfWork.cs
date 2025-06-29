using office_automation_system.application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Contracts.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository Users { get; }

        IAdministrativeProcessRepository AdministrativeProcesses { get; }

        INotificationRepository Notifications { get; }
        IProcessApprovalStepRepository ProcessApprovalSteps { get; }
        IRequestRepository Requests { get; }    
        IRequestStepRepository RequestSteps { get; }
        IRequestStepFileRepository RequestStepFiles { get; }
        Task<int> CompleteAsync();
    }
}
