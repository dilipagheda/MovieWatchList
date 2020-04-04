using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieWatchList.Data;
using MovieWatchList.ViewModels;

namespace MovieWatchList.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                var userId = await GetCurrentUserId();
                var userMovies = await _context.UserMovies.Where(m => m.UserId == userId).ToListAsync();

                var allMovies = await _context.Movies.ToListAsync();

                var model = allMovies.Select(m =>
                {
                    var userMovie = userMovies.FirstOrDefault(userMovie => userMovie.MovieId == m.Id);

                    if (userMovie != null)
                    {
                        return new MovieViewModel()
                        {
                            MovieId = m.Id,
                            Title = m.Title,
                            Year = m.Year,
                            InWatchlist = true,
                            Watched = userMovie.Watched,
                            Rating = userMovie.Rating
                        };
                    }
                    else
                    {
                        return new MovieViewModel()
                        {
                            MovieId = m.Id,
                            Title = m.Title,
                            Year = m.Year,
                            InWatchlist = false
                        };
                    }
                });
                return View(model);
            }
            else
            {
                var allMovies = await _context.Movies.ToListAsync();

                var model = allMovies.Select(m =>
                {
                    return new MovieViewModel()
                    {
                        MovieId = m.Id,
                        Title = m.Title,
                        Year = m.Year,
                    };
                });
                return View(model);
            }
           
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Year")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Year")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<bool> AddRemove([FromQuery] int movieId, bool val)
        {
            var movie = await _context.Movies.FindAsync(movieId);

            if (movie == null) return true;
            var currentUserId = await GetCurrentUserId();

            if (val)
            {
                //Add movie to watchlist
                var userMovie = new UserMovie()
                {
                    UserId = currentUserId,
                    MovieId = movieId,
                    Watched = false,
                    Rating = 0
                };
                _context.UserMovies.Add(userMovie);
            }
            else
            {
                //Remove
                var userMovie = await _context.UserMovies.FindAsync(movieId, currentUserId);
                _context.UserMovies.Remove(userMovie);
            }

            await _context.SaveChangesAsync();
            return val;
        }
        private async Task<string> GetCurrentUserId()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return user.Id;
        }
    }
}
