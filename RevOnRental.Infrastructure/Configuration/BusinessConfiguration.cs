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
    public class BusinessConfiguration : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.BusinessName)
                .HasMaxLength(100);

            builder.Property(b => b.BusinessRegistrationNumber)
                .HasMaxLength(50);

            builder.HasMany(b => b.Vehicles)
                .WithOne(v => v.Business)
                .HasForeignKey(v => v.BusinessID);

            builder.HasMany(b => b.BusinessDocuments)
               .WithOne(v => v.Business)
               .HasForeignKey(v => v.BusinessId).OnDelete(DeleteBehavior.Cascade); ;
        }
    }
}
