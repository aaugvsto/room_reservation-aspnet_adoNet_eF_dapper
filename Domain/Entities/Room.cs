using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class Room : Entity
    {
        public Room()
        {
            Reservations = new List<Reservation>();
        }

        public string Name { get; set; }
        public int Capacity { get; set; }
        public Location Location { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
