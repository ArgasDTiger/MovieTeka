using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTeka.Models;

public class MovieActor
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Movie")]
    public int MovieId { get; set; }

    [ForeignKey("Actor")]
    public int ActorId { get; set; }
    public Actor Actor { get; set; }
}