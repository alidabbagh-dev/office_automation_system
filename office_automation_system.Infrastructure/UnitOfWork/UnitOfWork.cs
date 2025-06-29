using office_automation_system.application.Contracts.Repositories;
using office_automation_system.application.Contracts.UnitOfWork;
using office_automation_system.Infrastructure.Data;
using office_automation_system.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IApplicationUserRepository Users { get; private set; }
        public IAdministrativeProcessRepository AdministrativeProcesses { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IProcessApprovalStepRepository ProcessApprovalSteps { get; private set; }
        public IRequestRepository Requests { get; private set; }
        public IRequestStepFileRepository RequestStepFiles { get; private set; }
        public IRequestStepRepository RequestSteps { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new ApplicationUserRepository(_context);
            AdministrativeProcesses = new AdministrativeProcessRepository(_context);
            Notifications = new NotificationRepository(_context);
            ProcessApprovalSteps = new ProcessApprovalStepRepository(_context);
            Requests = new RequestRepository(_context);
            RequestStepFiles = new RequestStepFileRepository(_context);
            RequestSteps = new RequestStepRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
