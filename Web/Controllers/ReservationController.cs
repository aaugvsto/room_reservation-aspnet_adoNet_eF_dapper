using System.Diagnostics;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;
using Web.Models;

namespace Web.Controllers
{
    public class ReservationController : BaseController<Reservation>
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService) : base(reservationService)
        {
            _reservationService = reservationService;
        }
    }
}
