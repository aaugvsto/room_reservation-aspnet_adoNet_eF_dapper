using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityFramework.Mapping
{
    public class EMPLOYEEMap : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //Table
            builder.ToTable("EMPLOYEE");

            //Primary Key
            builder.HasKey(e => e.Id);

            // Columns
            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Name)
                .HasColumnName("NAME")
                .IsRequired();

            builder.Property(e => e.Email)
                .HasColumnName("EMAIL")
                .IsRequired();

            builder.Property(e => e.Department)
                .HasColumnName("DEPARTMENT")
                .IsRequired();

            builder.Property(e => e.CreationDate)
               .HasColumnName("CREATION_DATE")
               .IsRequired();

            builder.Property(e => e.ModifiedDate)
               .HasColumnName("MODIFICATION_DATE")
               .IsRequired();
        }
    }
}
