using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Reservation : Entity
    {
        public int EmployeeId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Room Room { get; set; }
    }
}
