using System.ComponentModel.DataAnnotations;

namespace MovieTeka.Models;

public class Movie
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Image { get; set; }
    public string Country { get; set; }
    public string Director { get; set; }
    public ICollection<Genre> Genres { get; set; }
    public string PG { get; set; }
    public ICollection<Actor> Actors { get; set; }
}