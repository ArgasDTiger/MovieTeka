using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTeka.Models;

public class FavoriteMoviesId
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Movie")]
    public int MovieId { get; set; }

    public string AppUserId { get; set; }

    [ForeignKey("AppUserId")]
    public AppUser AppUser { get; set; }
}