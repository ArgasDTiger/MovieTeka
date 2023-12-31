﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTeka.Data;
using MovieTeka.Interfaces;
using MovieTeka.Models;
using MovieTeka.ViewModels;

namespace MovieTeka.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMovieRepository _movieRepository;
    private readonly IActorRepository _actorRepository;
    private readonly IFavoriteActorsIdRepository _favoriteActorsIdRepository;
    private readonly IFavoriteMoviesIdRepository _favoriteMoviesIdRepository;
    private readonly ApplicationDbContext _context;
    
    public AccountController(ApplicationDbContext context,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IMovieRepository movieRepository,
        IActorRepository actorRepository,
        IFavoriteActorsIdRepository favoriteActorsIdRepository,
        IFavoriteMoviesIdRepository favoriteMoviesIdRepository)
    {
        _context = context;
        _signInManager = signInManager;
        _movieRepository = movieRepository;
        _actorRepository = actorRepository;
        _favoriteActorsIdRepository = favoriteActorsIdRepository;
        _favoriteMoviesIdRepository = favoriteMoviesIdRepository;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        var response = new LoginViewModel();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid) return View(loginViewModel);
        var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
        if (user != null)
        {
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (passwordCheck)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }
        TempData["Error"] = "Wrong credentials. Please try again";
        return View(loginViewModel);
    }

    [HttpGet]
    public IActionResult Register()
    {
        var response = new RegisterViewModel();
        return View(response);
        
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid) return View(registerViewModel);
        var userEmail = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
        var userName = await _userManager.FindByNameAsync(registerViewModel.Username);
        if (userEmail != null)
        {
            TempData["Error"] = "This email is already in use";
            return View(registerViewModel);
        }
        if (userName != null)
        {
            TempData["Error"] = "This username is already in use";
            return View(registerViewModel);
        }

        var newUser = new AppUser()
        {
            UserName = registerViewModel.Username,
            Email = registerViewModel.EmailAddress
        };

        var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

        if (newUserResponse.Succeeded)
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;
        //TODO: add WatchList, FavoriteActors
        ProfileViewModel profileVM = new ProfileViewModel
        {
            FavoriteMovies = await _favoriteMoviesIdRepository.GetAllMoviesByUserId(userId)
        };
        return View(profileVM);
    }

    
}