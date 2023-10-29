using MovieTeka.Models;

namespace MovieTeka.ViewModels;

public class EditActorViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IFormFile Image { get; set; }
    public string? URL { get; set; }
    public string Country { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
}