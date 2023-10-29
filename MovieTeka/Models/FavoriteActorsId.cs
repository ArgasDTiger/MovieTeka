using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTeka.Models;

public class FavoriteActorsId
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Actor")]
    public int ActorId { get; set; }

    public string AppUserId { get; set; }

    [ForeignKey("AppUserId")]
    public AppUser AppUser { get; set; }
}