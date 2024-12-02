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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.PaymentDate).IsRequired();
            builder.Property(p => p.PaymentType).HasMaxLength(50);
            builder.Property(p => p.PaymentStatus).HasMaxLength(50);

            // Relationship with User
            builder.HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade); ;

            // Relationship with Business
            builder.HasOne(p => p.Business)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BusinessId).OnDelete(DeleteBehavior.Cascade); ;

            // Optional relationship with Vehicle
            builder.HasOne(p => p.Vehicle)
                .WithMany(v => v.Payments)
                .HasForeignKey(p => p.VehicleId).OnDelete(DeleteBehavior.Cascade); ;
                
        }
    }

}
