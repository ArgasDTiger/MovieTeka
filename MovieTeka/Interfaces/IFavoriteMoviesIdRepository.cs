using MovieTeka.Models;

namespace MovieTeka.Interfaces;

public interface IFavoriteMoviesIdRepository
{
    Task<FavoriteMoviesId> GetByIdAsync(int movieId, string userId);
    Task<FavoriteMoviesId> IsInFavorites(int movieId, string userId);
}