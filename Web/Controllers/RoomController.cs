using System.Diagnostics;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;
using Web.Models;

namespace Web.Controllers
{
    public class RoomController : BaseController<Room>
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService) : base(roomService)
        {
            _roomService = roomService;
        }
    }
}
