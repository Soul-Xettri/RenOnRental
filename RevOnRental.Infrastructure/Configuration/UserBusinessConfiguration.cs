using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Infrastructure.Configuration
{
    public class UserBusinessConfiguration : IEntityTypeConfiguration<UserBusiness>
    {
        public void Configure(EntityTypeBuilder<UserBusiness> builder)
        {
            builder.HasKey(ub => new { ub.UserId, ub.BusinessId });

            builder.HasOne(ub => ub.User)
                .WithOne(u => u.UserBusiness)
                .HasForeignKey<UserBusiness>(ub => ub.UserId).OnDelete(DeleteBehavior.Cascade); ;

            builder.HasOne(ub => ub.Business)
                .WithOne(b => b.UserBusiness)
                .HasForeignKey<UserBusiness>(ub => ub.BusinessId).OnDelete(DeleteBehavior.Cascade); ;
        }
    }
}
