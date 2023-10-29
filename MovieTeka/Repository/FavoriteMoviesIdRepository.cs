using Microsoft.EntityFrameworkCore;
using MovieTeka.Data;
using MovieTeka.Models;

namespace MovieTeka.Interfaces;

public class FavoriteMoviesIdRepository : IFavoriteMoviesIdRepository
{
    private readonly ApplicationDbContext _context;

    public FavoriteMoviesIdRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<FavoriteMoviesId> GetByIdAsync(int movieId, string userId)
    {
        return await _context.FavoriteMoviesId.FirstOrDefaultAsync(m => m.MovieId == movieId && m.AppUserId == userId);
    }

    public async Task<FavoriteMoviesId> IsInFavorites(int movieId, string userId)
    {
        return  await _context.FavoriteMoviesId.FirstOrDefaultAsync(m => m.MovieId == movieId && m.AppUserId == userId);
    }
}