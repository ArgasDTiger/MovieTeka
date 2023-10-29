namespace MovieTeka.ViewModels;

public class ProfileViewModel
{
    public int Id { get; set; }
    //foreign key?
    public string Username { get; set; }
    public string? ProfileImageUrl { get; set; }
}