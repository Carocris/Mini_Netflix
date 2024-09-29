using Application.ViewModels;
using Application.Services;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Database.Contexts;

namespace MiniNetflix.Controllers
{
    public class GenresController : Controller
    {
        private readonly GenreService _genreService;

        public GenresController(ApplicationContext dbContext)
        {
            _genreService = new GenreService(dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            ViewBag.SearchController = "Genres";
            ViewBag.SearchAction = "Search";
            ViewBag.SearchTerm = searchTerm;

            var genres = await _genreService.GetAllViewModel(); 

            if (!string.IsNullOrEmpty(searchTerm))
            {
                genres = genres.Where(g => g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(genres); 
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            ViewBag.SearchController = "Genres";
            ViewBag.SearchAction = "Search";
            ViewBag.SearchTerm = searchTerm;

            var genres = await _genreService.GetAllViewModel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                genres = genres.Where(g => g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View("Index", genres);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("SaveGenre", new SaveGenreViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveGenreViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveGenre", vm); 
            }

            await _genreService.Add(vm);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var genre = await _genreService.GetByIdSaveViewModel(id);

            if (genre == null)
            {
                return NotFound();
            }

            var vm = new SaveGenreViewModel
            {
                Id = genre.Id,
                Name = genre.Name
            };

            return View("SaveGenre", vm); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveGenreViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveGenre", vm); 
            }

            await _genreService.Update(vm);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _genreService.GetByIdSaveViewModel(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _genreService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
