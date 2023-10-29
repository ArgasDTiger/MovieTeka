using System.Collections;
using MovieTeka.Models;

namespace MovieTeka.ViewModels;

public class IndexViewModel
{
    public List<ButtonViewModel> ButtonViewModels  { get; set; }
    public IEnumerable<Movie> Movies { get; set; }
    
    public IEnumerable<Actor> Actors { get; set; }

}