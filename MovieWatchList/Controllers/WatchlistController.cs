using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieWatchList.Data;
using MovieWatchList.ViewModels;

namespace MovieWatchList.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public WatchlistController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            //Get the id  of currently logged in user
            var id = await GetCurrentUserId();

            //Retrieve the watch list data for this id
            var userMovies = _dbContext.UserMovies.Where(m => m.UserId == id);

            //Convert to view model
            var model = userMovies.Select(m => new MovieViewModel()
            {
                MovieId = m.MovieId,
                Title = m.Movie.Title,
                Year = m.Movie.Year,
                InWatchlist = true,
                Watched = m.Watched,
                Rating = m.Rating
            }).ToList();

            //Pass view model to View
            return View(model);
        }

        private async Task<string> GetCurrentUserId()
        {
           var user = await _userManager.GetUserAsync(HttpContext.User);
            return user.Id;
        }
    }
}

