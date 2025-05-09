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
    public class ROOMMap : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            //Table
            builder.ToTable("ROOM");

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

            builder.Property(e => e.Location)
                .HasColumnName("LOCATION")
                .IsRequired();

            builder.Property(e => e.Capacity)
               .HasColumnName("CAPACITY")
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
