using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.EntityFramework
{
    public class DBContext : DbContext
    {
        private readonly string ConnectionString;

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBContext).Assembly);
        }
    }
}
