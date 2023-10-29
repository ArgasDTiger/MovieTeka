using MovieTeka.Models;

namespace MovieTeka.Interfaces;

public interface IActorRepository
{
    Task<IEnumerable<Actor>> GetAll();
    Task<Actor> GetByIdAsync(int id);
    Task<Actor> GetByIdAsyncNoTracking(int id);
    Task<int?> GetIdByNameAsync(string name);

    bool Add(Actor actor);
    bool Update(Actor actor);
    bool Delete(Actor actor);
    bool Save();
}