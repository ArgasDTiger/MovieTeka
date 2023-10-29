using MovieTeka.Models;

namespace MovieTeka.Interfaces;

public interface IMovieActorRepository
{
    Task<IEnumerable<MovieActor>> GetAll();
    Task<MovieActor> GetByIdAsync(int id);
    Task<MovieActor> GetByIdAsyncNoTracking(int id);
    Task<MovieActor> GetByActorIdAsync(int actorId);
    Task<MovieActor> GetByMovieIdAsync(int movieId);
    Task<List<string>> GetAllActorNamesByMovieIdAsync(int movieId);
    bool Add(MovieActor movieActor);
    bool Update(MovieActor movieActor);
    bool Delete(MovieActor movieActor);
    bool Save();
}