using MovieTeka.Models;

namespace MovieTeka.Data;

public class Seed
{
    public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Movies.Any())
                {
                    context.Movies.AddRange(new List<Movie>()
                    {
                        new Movie()
                        {
                            Title = "The Imitation Game",
                            Image = "https://m.media-amazon.com/images/M/MV5BOTgwMzFiMWYtZDhlNS00ODNkLWJiODAtZDVhNzgyNzJhYjQ4L2ltYWdlXkEyXkFqcGdeQXVyNzEzOTYxNTQ@._V1_.jpg?width=768&mxf-ref=https://bbfc.co.uk", 
                            Country = "USA",
                            Director = "Morten Tyldum",
                            PG = "12+"
                        },
                        new Movie()
                        {
                            Title = "The Shawshank Redemption",
                            Image = "https://upload.wikimedia.org/wikipedia/uk/8/87/%D0%92%D1%82%D0%B5%D1%87%D0%B0_%D0%B7_%D0%A8%D0%BE%D1%83%D1%88%D0%B5%D0%BD%D0%BA%D0%B0.jpg", 
                            Country = "USA",
                            Director = "Frank Darabont",
                            PG = "15+"
                        }
                    });
                    context.SaveChanges();
                }
                //Actors
                if (!context.Actors.Any())
                {
                    context.Actors.AddRange(new List<Actor>()
                    {
                        new Actor()
                        {
                            Name = "Benedict Cumberbatch",
                            Image = "https://m.media-amazon.com/images/M/MV5BMjE0MDkzMDQwOF5BMl5BanBnXkFtZTgwOTE1Mjg1MzE@._V1_.jpg",
                        },
                        new Actor()
                        {
                            Name = "Bruce Lee",
                            Image = "https://upload.wikimedia.org/wikipedia/commons/c/ca/Bruce_Lee_1973.jpg",
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
}