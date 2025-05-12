using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public abstract class ReservationRepository : IReservationRepository
    {
        public abstract Task<Reservation> Add(Reservation entity);

        public abstract Task<bool> Delete(int id);

        public abstract Task<IEnumerable<Reservation>> GetAll();

        public abstract Task<Reservation> GetById(int id);

        public abstract Task<Reservation> Update(Reservation entity);
    }
}
