namespace MovieTeka.ViewModels;

public class CreateMovieViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public IFormFile Image { get; set; }
    public string Country { get; set; }
    public string Director { get; set; }
    //public ICollection<Genre> Genres { get; set; }
    public string PG { get; set; }
    //public ICollection<Actor> Actors { get; set; }
}