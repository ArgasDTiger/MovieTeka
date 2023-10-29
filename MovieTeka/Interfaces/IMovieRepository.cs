using MovieTeka.Models;

namespace MovieTeka.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAll();
    Task<Movie> GetByIdAsync(int id);
    Task<Movie> GetByIdAsyncNoTracking(int id);
    bool Add(Movie movie);
    bool Update(Movie movie);
    bool Delete(Movie movie);
    bool Save();
}