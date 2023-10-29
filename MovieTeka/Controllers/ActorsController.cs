using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTeka.Data;
using MovieTeka.Interfaces;
using MovieTeka.Models;
using MovieTeka.ViewModels;
using RunGroopWebApp.Interfaces;

namespace MovieTeka.Controllers;

public class ActorsController : Controller
{
    private readonly IFavoriteActorsIdRepository _favoriteActorsIdRepository;
    private readonly IActorRepository _actorRepository;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPhotoService _photoService;
    private readonly UserManager<AppUser> _userManager;
    public ActorsController(ApplicationDbContext context, IActorRepository ActorRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService, UserManager<AppUser> userManager, IFavoriteActorsIdRepository favoriteActorsIdRepository)
    {
        _context = context;
        _actorRepository = ActorRepository;
        _httpContextAccessor = httpContextAccessor;
        _photoService = photoService;
        _userManager = userManager;
        _favoriteActorsIdRepository = favoriteActorsIdRepository;
    }
    public async Task<IActionResult> Index()
    {
        IEnumerable<Actor> actors = await _actorRepository.GetAll();
        var buttonViewModels = new List<ButtonViewModel>();
        var user = await _userManager.GetUserAsync(User);

        foreach (var actor in actors)
        {
            var isInFavorites = await _favoriteActorsIdRepository.IsInFavorites(actor.Id, user.Id);
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
            Actors = actors,
            ButtonViewModels = buttonViewModels,
        };

        return View(viewModel);
    }
    
    public async Task<IActionResult> Detail(int id)
    {
        Actor actor = await _actorRepository.GetByIdAsync(id);
        return View(actor);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateActorViewModel actorVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(actorVM.Image);
            var actor = new Actor()
            {
                Name = actorVM.Name,
                Image = result.Url.ToString(),
                Country = actorVM.Country,
                DateOfBirth = actorVM.DateOfBirth
            };
            _actorRepository.Add(actor);
            return RedirectToAction("Index");
        }
        ModelState.AddModelError("", "Photo upload failed");
        return View(actorVM);
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);
        if (actor == null) return View("Error");
        var actorVM = new EditActorViewModel()
        {
            Name = actor.Name,
            URL = actor.Image,
            Country = actor.Country,
            DateOfBirth = actor.DateOfBirth
        };
        return View(actorVM);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditActorViewModel actorVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Could not edit the actor");
            return View("Edit", actorVM);
        }

        var userActor = await _actorRepository.GetByIdAsyncNoTracking(id);
        if (userActor != null)
        {
            try
            {
                await _photoService.DeletePhotoAsync(userActor.Image);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Could not delete the photo");
                return View(actorVM);
            }

            var photoResult = await _photoService.AddPhotoAsync(actorVM.Image);
            var actor = new Actor()
            {
                Id = id,
                Name = actorVM.Name,
                Image = photoResult.Url.ToString(),
                Country = actorVM.Country,
                DateOfBirth = actorVM.DateOfBirth
            };
            _actorRepository.Update(actor);
            return RedirectToAction("Index");
        }
        
        return View(actorVM);
    }
    [Authorize(Roles="admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var actorDetails = await _actorRepository.GetByIdAsync(id);
        if (actorDetails == null) return View("Error");
        return View(actorDetails);
    }
    
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles="admin")]
    public async Task<IActionResult> DeleteActor(int id)
    {
        var actorDetails = await _actorRepository.GetByIdAsync(id);
        if (actorDetails == null) return View("Error");
        
        _actorRepository.Delete(actorDetails);
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public async Task<IActionResult> AddToFavorites(int actorId)
    {
        var user = await _userManager.GetUserAsync(User);
        var favoriteActor = new FavoriteActorsId()
        {
            ActorId = actorId,
            AppUserId = user.Id
        };

        _context.FavoriteActorsId.Add(favoriteActor);

        await _context.SaveChangesAsync();

        return Json(new { action = "RemoveFromFavorites" });
    }
    
    [HttpPost]
    public async Task<IActionResult> RemoveFromFavorites(int actorId)
    {
        var user = await _userManager.GetUserAsync(User);

        var favoriteActor = await _favoriteActorsIdRepository.GetByIdAsync(actorId, user.Id);

        if (favoriteActor != null)
        {
            _context.FavoriteActorsId.Remove(favoriteActor);
            await _context.SaveChangesAsync();
        }

        return Json(new { action = "AddToFavorites" });
    }
}