using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MovieTeka.Models;

public class AppUser : IdentityUser
{
    public string? ProfileImageUrl { get; set; }
    public ICollection<FavoriteMoviesId> FavoriteMoviesId { get; set; }
    public ICollection<Actor> FavoriteActors { get; set; }
    public ICollection<Genre> FavoriteGenres { get; set; }
}