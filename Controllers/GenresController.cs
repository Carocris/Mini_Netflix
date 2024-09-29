using Application.Services;
using Database.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace MiniNetflix.Controllers
{
    public class GenresController : Controller
    {
        private readonly SerieService _Seriesservice;

        public GenresController(ApplicationContext dbContext)
        {
            _Seriesservice = new SerieService(dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> Genres()
        {
            var genres = await _Seriesservice.GetAllGenresAsync();
            return View(genres);
        }
    }
}
