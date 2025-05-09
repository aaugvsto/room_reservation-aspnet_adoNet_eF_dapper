using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services
{

    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomService)
        {
            _roomRepository = roomService;
        }

        public Task<Room> AddAsync(Room entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await this._roomRepository.GetAll();
        }

        public Task<Room> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Room> UpdateAsync(Room entity)
        {
            throw new NotImplementedException();
        }
    }
}
