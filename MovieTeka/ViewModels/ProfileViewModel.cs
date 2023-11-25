using MovieTeka.Models;

namespace MovieTeka.ViewModels;

public class ProfileViewModel
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string? ProfileImageUrl { get; set; }
    
    public List<Movie> WatchList { get; set; }
    public List<Movie> FavoriteMovies { get; set; }
    public List<Actor> FavoriteActors { get; set; } 
}