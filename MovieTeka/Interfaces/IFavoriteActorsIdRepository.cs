using MovieTeka.Models;

namespace MovieTeka.Interfaces;

public interface IFavoriteActorsIdRepository
{
    Task<FavoriteActorsId> GetByIdAsync(int movieId, string userId);
    Task<FavoriteActorsId> IsInFavorites(int movieId, string userId);
}