using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public abstract class RoomRepository : IRoomRepository
    {
        public abstract Task<Room> Add(Room entity);

        public abstract Task<bool> Delete(int id);

        public abstract Task<IEnumerable<Room>> GetAll();

        public abstract Task<Room> GetById(int id);

        public abstract Task<Room> Update(Room entity);
    }
}
