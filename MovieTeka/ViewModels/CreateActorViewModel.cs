namespace MovieTeka.ViewModels;

public class CreateActorViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IFormFile Image { get; set; }
    public string Country { get; set; }
    public DateTime? DateOfBirth { get; set; }
}