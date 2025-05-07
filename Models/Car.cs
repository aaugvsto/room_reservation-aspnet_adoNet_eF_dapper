using Entities;

namespace Entities
{
    public class Car
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
