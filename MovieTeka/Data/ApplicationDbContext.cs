using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTeka.Models;

namespace MovieTeka.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<FavoriteMoviesId> FavoriteMoviesId { get; set; }
    public DbSet<FavoriteActorsId> FavoriteActorsId { get; set; }
    public DbSet<MovieActor> MovieActor { get; set; }
}

