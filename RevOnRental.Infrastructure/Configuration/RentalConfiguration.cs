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
    public class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.StartDate)
                .IsRequired();

            builder.Property(r => r.EndDate)
                .IsRequired();

            builder.Property(r => r.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(r => r.RentalStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.UserID).OnDelete(DeleteBehavior.Cascade); ;

            builder.HasOne(r => r.Vehicle)
                .WithMany(v => v.Rentals).HasForeignKey(r=>r.VehicleId).OnDelete(DeleteBehavior.Cascade);


        }
    }
}
