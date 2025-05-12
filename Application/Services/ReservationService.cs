using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services
{

    public class ReservationService : Service<Reservation>, IReservationService
    {
        protected readonly IReservationRepository _repository;

        public ReservationService(IReservationRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
