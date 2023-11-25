using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTeka.Data;
using MovieTeka.Interfaces;
using MovieTeka.Models;
using MovieTeka.ViewModels;
using Newtonsoft.Json;
using RunGroopWebApp.Interfaces;

namespace MovieTeka.Controllers;

public class MoviesController : Controller
{
    private readonly IFavoriteMoviesIdRepository _favoriteMoviesIdRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IActorRepository _actorRepository;
    private readonly IMovieActorRepository _movieActorRepository;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPhotoService _photoService;
    private readonly UserManager<AppUser> _userManager;
    public MoviesController(ApplicationDbContext context, IMovieRepository movieRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService, IActorRepository actorRepository, UserManager<AppUser> userManager, IFavoriteMoviesIdRepository favoriteMoviesIdRepository, IMovieActorRepository movieActorRepository)
    {
        _context = context;
        _movieRepository = movieRepository;
        _httpContextAccessor = httpContextAccessor;
        _photoService = photoService;
        _actorRepository = actorRepository;
        _userManager = userManager;
        _favoriteMoviesIdRepository = favoriteMoviesIdRepository;
        _movieActorRepository = movieActorRepository;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Movie> movies = await _movieRepository.GetAll();
        var buttonViewModels = new List<ButtonViewModel>();
        var user = await _userManager.GetUserAsync(User);

        foreach (var movie in movies)
        {
            var isInFavorites = await _favoriteMoviesIdRepository.IsInFavorites(movie.Id, user.Id);
            ButtonViewModel buttonViewModel;
            if (isInFavorites == null)
            {
                buttonViewModel = new ButtonViewModel
                {
                    ButtonName = "Add to Favorites",
                    ButtonAction = "AddToFavorites"
                };
            }
            else
            {
                buttonViewModel = new ButtonViewModel
                {
                    ButtonName = "Remove from Favorites",
                    ButtonAction = "RemoveFromFavorites"
                };
            }
            buttonViewModels.Add(buttonViewModel);
        }

        var viewModel = new IndexViewModel
        {
            Movies = movies,
            ButtonViewModels = buttonViewModels,
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Movie movie = await _movieRepository.GetByIdAsync(id);
        return View(movie);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateMovieViewModel movieVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(movieVM.Image);
            var movie = new Movie()
            {
                Title = movieVM.Title,
                Image = result.Url.ToString(),
                Country = movieVM.Country,
                Director = movieVM.Director,
                PG = movieVM.PG,
            };
            _movieRepository.Add(movie);
            return RedirectToAction("Index");
        }
        ModelState.AddModelError("", "Photo upload failed");
        return View(movieVM);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);

        if (movie == null) return View("Error");
        var movieVM = new EditMovieViewModel()
        {
            Title = movie.Title,
            URL = movie.Image,
            Country = movie.Country,
            Director = movie.Director,
            PG = movie.PG,
            ActorNames = await _movieActorRepository.GetAllActorNamesByMovieIdAsync(id)
        };

        return View(movieVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditMovieViewModel movieVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Could not edit the movie");
            return View("Edit", movieVM);
        }
        var userMovie = await _movieRepository.GetByIdAsyncNoTracking(id);
        
        if (userMovie != null)
        {
            try
            {
                await _photoService.DeletePhotoAsync(userMovie.Image);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Could not delete the photo");
                return View(movieVM);
            }
            
            
            var photoResult = await _photoService.AddPhotoAsync(movieVM.Image);
            var movie = new Movie
            {
                Id = id,
                Title = movieVM.Title,
                Image = photoResult.Url.ToString(),
                Country = movieVM.Country,
                Director = movieVM.Director,
                PG = movieVM.PG,
            };
            _movieRepository.Update(movie);

            string actorsJson = Request.Form["actorsArray"];
            List<string>? inputActors = JsonConvert.DeserializeObject<List<string>>(actorsJson);
            
            var actorsInMovie = await _movieActorRepository.GetAllActorNamesByMovieIdAsync(id);
            if (inputActors != null)
            {
                foreach (string actor in inputActors)
                {
                    var actorId = await _actorRepository.GetIdByNameAsync(actor);

                    if (actorId == null)
                        continue;
                    //if there is already such a connection then do this
                    if (await _movieActorRepository.GetByMovieAndActorIdAsync(id, (int)actorId) == null)
                    {
                        var movieActor = new MovieActor
                        {
                            MovieId = movieVM.Id,
                            ActorId = actorId.Value
                        };
                        _movieActorRepository.Update(movieActor);
                    }
                }
            }

            List<string> actorsToRemove = actorsInMovie.Where(name => !inputActors.Contains(name)).ToList();
            
            if (actorsToRemove.Count == 0)
                return RedirectToAction("Index");
            
            foreach (var name in actorsToRemove)
            {
                var actorId = await _actorRepository.GetIdByNameAsync(name);

                var movieActor = await _movieActorRepository.GetByMovieAndActorIdAsync(id, actorId.Value);
                if (movieActor != null)
                    _movieActorRepository.Delete(movieActor);
            }

            return RedirectToAction("Index");
        }
        return View(movieVM);
    }
    
    
    [Authorize(Roles="admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var movieDetails = await _movieRepository.GetByIdAsync(id);
        if (movieDetails == null) return View("Error");
        return View(movieDetails);
    }
    
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles="admin")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movieDetails = await _movieRepository.GetByIdAsync(id);
        if (movieDetails == null) return View("Error");
        
        _movieRepository.Delete(movieDetails);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddToFavorites(int movieId)
    {
        var user = await _userManager.GetUserAsync(User);
        var favoriteMovie = new FavoriteMoviesId()
        {
            MovieId = movieId,
            AppUserId = user.Id
        };

        _context.FavoriteMoviesId.Add(favoriteMovie);

        await _context.SaveChangesAsync();

        return Json(new { action = "RemoveFromFavorites" });
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromFavorites(int movieId)
    {
        var user = await _userManager.GetUserAsync(User);

        var favoriteMovie = await _favoriteMoviesIdRepository.GetByIdAsync(movieId, user.Id);

        if (favoriteMovie != null)
        {
            _context.FavoriteMoviesId.Remove(favoriteMovie);
            await _context.SaveChangesAsync();
        }

        return Json(new { action = "AddToFavorites" });
    }


}