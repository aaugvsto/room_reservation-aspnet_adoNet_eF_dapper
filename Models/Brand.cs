using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Brand
    {
        public Brand()
        {
            Cars = new List<Car>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        IList<Car> Cars { get; set; }
    }
}
