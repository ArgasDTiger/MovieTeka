using Microsoft.EntityFrameworkCore;
using MovieTeka.Data;
using MovieTeka.Interfaces;
using MovieTeka.Models;

namespace MovieTeka.Repository;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Movie>> GetAll()
    {
        return await _context.Movies.ToListAsync();
    }

    public async Task<Movie> GetByIdAsync(int id)
    {
        return await _context.Movies.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Movie> GetByIdAsyncNoTracking(int id)
    {
        return await _context.Movies.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
    }

    public bool Add(Movie movie)
    {
        _context.Add(movie);
        return Save();
    }

    public bool Update(Movie movie)
    {
        _context.Update(movie);
        return Save();
    }

    public bool Delete(Movie movie)
    {
        _context.Remove(movie);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}