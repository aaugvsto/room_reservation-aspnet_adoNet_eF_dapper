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
    public class RESERVATIONMap : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            //Table
            builder.ToTable("RESERVATION");

            //Primary Key
            builder.HasKey(e => e.Id);

            // Columns
            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.EmployeeId)
                .HasColumnName("NAME")
                .IsRequired();

            builder.Property(e => e.StartDate)
               .HasColumnName("START_DATE")
               .IsRequired();

            builder.Property(e => e.EndDate)
               .HasColumnName("END_DATE")
               .IsRequired();

            builder.Property(e => e.RoomId)
                .HasColumnName("EMAIL")
                .IsRequired();
        }
    }
}
