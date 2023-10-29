using Microsoft.EntityFrameworkCore;
using MovieTeka.Data;
using MovieTeka.Interfaces;
using MovieTeka.Models;

namespace MovieTeka.Repository;

public class MovieActorRepository : IMovieActorRepository
{
    
    private readonly ApplicationDbContext _context;

    public MovieActorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovieActor>> GetAll()
    {
        return await _context.MovieActor.ToListAsync();
    }

    public async Task<MovieActor> GetByIdAsync(int id)
    {
        return await _context.MovieActor.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<MovieActor> GetByIdAsyncNoTracking(int id)
    {
        return await _context.MovieActor.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<MovieActor> GetByActorIdAsync(int actorId)
    {
        return await _context.MovieActor.FirstOrDefaultAsync(i => i.ActorId == actorId);
    }

    public async Task<MovieActor> GetByMovieIdAsync(int movieId)
    {
        return await _context.MovieActor.FirstOrDefaultAsync(i => i.MovieId == movieId);
    }
    
    public async Task<List<string>> GetAllActorNamesByMovieIdAsync(int movieId)
    {
        return await _context.MovieActor
            .Where(ma => ma.MovieId == movieId)
            .Include(ma => ma.Actor)
            .Select(ma => ma.Actor.Name)
            .ToListAsync();
    }


    public bool Add(MovieActor movieActor)
    {
        _context.Add(movieActor);
        return Save();
    }

    public bool Update(MovieActor movieActor)
    {
        _context.Update(movieActor);
        return Save();
    }

    public bool Delete(MovieActor movieActor)
    {
        _context.Remove(movieActor);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}