using MovieTeka.Models;

namespace MovieTeka.ViewModels;

public class EditMovieViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public IFormFile Image { get; set; }
    public string? URL { get; set; }
    public string Country { get; set; }
    public string Director { get; set; }
    public string PG { get; set; }
    public ICollection<string>? ActorNames { get; set; }
}