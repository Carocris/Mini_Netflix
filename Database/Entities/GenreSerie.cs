using Database.Entities;

public class GenreSerie
{
    public int? SerieId { get; set; }  
    public Serie? Series { get; set; }

    public int? GenreId { get; set; }  
    public Genre? Genre { get; set; }
}
