using System.ComponentModel.DataAnnotations;

namespace MovieTeka.Models;

public class Actor
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Image { get; set; }
    public string Country { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public ICollection<Movie> Movies { get; set; }
}