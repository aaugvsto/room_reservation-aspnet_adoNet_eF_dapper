using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.DataAccess.EntityFramework;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReservationRepositoryEF : ReservationRepository
    {
        private DbSet<Reservation> DbSet { get { return _context.Set<Reservation>(); } }
        private readonly DBContext _context;

        public ReservationRepositoryEF(DBContext dbContext)
        {
            _context = dbContext;
        }

        public async override Task<Reservation> Add(Reservation entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;

            await DbSet.AddAsync(entity);

            return entity;
        }

        public async override Task<bool> Delete(int id)
        {
            var entity = await DbSet.FindAsync(id);

            if (entity == null)
                return false;

            DbSet.Remove(entity);

            return true;
        }

        public async override Task<IEnumerable<Reservation>> GetAll()
        {
            return await this.DbSet.ToListAsync();
        }

        public async override Task<Reservation> GetById(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async override Task<Reservation> Update(Reservation entity)
        {
            entity.ModifiedDate = DateTime.Now;

            var entry = this.DbSet.Entry(entity);
            entry.State = EntityState.Modified;

            await this.SaveChanges();

            return entity;
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
