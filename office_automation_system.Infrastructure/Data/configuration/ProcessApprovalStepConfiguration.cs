using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using office_automation_system.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.Data.configuration
{
    public class ProcessApprovalStepConfiguration : IEntityTypeConfiguration<ProcessApprovalStep>
    {
        public void Configure(EntityTypeBuilder<ProcessApprovalStep> builder)
        {

            // Each ProcessApprovalStep is assigned to one user (Owner)
            // But we don’t define the navigation property on ApplicationUser (so we use WithMany())
            builder
                .HasOne(p => p.Owner)
                .WithMany() // no collection of steps defined in ApplicationUser
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // do not delete step if user is deleted


        }
    }
}
