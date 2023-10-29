using Microsoft.EntityFrameworkCore;
using MovieTeka.Data;
using MovieTeka.Interfaces;
using MovieTeka.Models;

namespace MovieTeka.Repository;

public class ActorRepository : IActorRepository
{
    private readonly ApplicationDbContext _context;
    
    public ActorRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Actor>> GetAll()
    {
        return await _context.Actors.ToListAsync();
    }

    public async Task<Actor> GetByIdAsync(int id)
    {
        return await _context.Actors.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Actor> GetByIdAsyncNoTracking(int id)
    {
        return await _context.Actors.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<int?> GetIdByNameAsync(string name)
    {
        var actor = await _context.Actors.FirstOrDefaultAsync(i => i.Name == name);
        return actor?.Id;
    }
    
    public bool Add(Actor actor)
    {
        _context.Add(actor);
        return Save();
    }

    public bool Update(Actor actor)
    {
        _context.Update(actor);
        return Save();
    }

    public bool Delete(Actor actor)
    {
        _context.Remove(actor);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}