
using Database.Common;


namespace Database.Entities
{
    public class Genre : BaseBasicEntity
    {

        // Navegación para géneros secundarios

        public ICollection<GenreSerie> SecondaryGenres { get; set; }
    }
}
