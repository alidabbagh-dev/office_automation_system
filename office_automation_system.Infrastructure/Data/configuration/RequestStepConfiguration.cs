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
    public class RequestStepConfiguration : IEntityTypeConfiguration<RequestStep>
    {
        public void Configure(EntityTypeBuilder<RequestStep> builder)
        {

            // Each RequestStep belongs to one Request (1:N)
            builder
                .HasOne(rs => rs.Request)
                .WithMany()
                .HasForeignKey(rs => rs.RequestId)
                .OnDelete(DeleteBehavior.Restrict); // prevent deletion of steps if request is deleted

            // Each RequestStep is handled by one user (Owner)
            builder
                .HasOne(rs => rs.Owner)
                .WithMany()
                .HasForeignKey(rs => rs.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // don't delete step if user is deleted


        }
    }
}
