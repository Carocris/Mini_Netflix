namespace Database.Entities
{
    public class Serie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        public int? GenreId { get; set; } 
        public Genre? Genre { get; set; }


        public int? ProducerId { get; set; }
        public Producer? Producers { get; set; } 

        public ICollection<GenreSerie> SecondaryGenres { get; set; } 
    }


}