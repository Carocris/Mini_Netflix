using Database.Common;

namespace Database.Entities
{
    public class Producer : BaseBasicEntity
    {

        public ICollection<Serie>? Series { get; set; }
    }
}
