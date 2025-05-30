﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Base;
using Domain.Interfaces.Services;

namespace Application.Services
{

    public class RoomService : Service<Room>, IRoomService
    {
        private readonly IRoomRepository _repository;

        public RoomService(IRoomRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
