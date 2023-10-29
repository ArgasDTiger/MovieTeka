using Microsoft.EntityFrameworkCore;
using MovieTeka.Data;
using MovieTeka.Interfaces;
using MovieTeka.Models;

namespace MovieTeka.Repository;

public class FavoriteActorsIdRepository : IFavoriteActorsIdRepository
{
    private readonly ApplicationDbContext _context;

    public FavoriteActorsIdRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<FavoriteActorsId> GetByIdAsync(int actorId, string userId)
    {
        return await _context.FavoriteActorsId.FirstOrDefaultAsync(m => m.ActorId == actorId && m.AppUserId == userId);
    }

    public async Task<FavoriteActorsId> IsInFavorites(int actorId, string userId)
    {
        return  await _context.FavoriteActorsId.FirstOrDefaultAsync(m => m.ActorId == actorId && m.AppUserId == userId);
    }
}