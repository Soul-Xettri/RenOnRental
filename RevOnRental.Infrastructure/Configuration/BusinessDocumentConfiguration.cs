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
    public class BusinessDocumentConfiguration : IEntityTypeConfiguration<BusinessDocument>
    {
        public void Configure(EntityTypeBuilder<BusinessDocument> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.DocumentType)
               .IsRequired();

          

        }
    }
    
}
