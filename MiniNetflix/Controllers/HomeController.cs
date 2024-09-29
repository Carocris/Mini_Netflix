using Application.Services;
using Database.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;


namespace MiniNetflix.Controllers
{
    public class HomeController : Controller
    {
        private readonly SerieService _Seriesservice;

        public HomeController(ApplicationContext dbContext)
        {
            _Seriesservice = new SerieService(dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.SearchController = "Home";
            ViewBag.SearchAction = "Search";
            ViewBag.SearchTerm = null; 

            ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
            ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();

            return View(await _Seriesservice.GetAllViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            ViewBag.SearchController = "Home";
            ViewBag.SearchAction = "Search";
            ViewBag.SearchTerm = searchTerm;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return View("Index", await _Seriesservice.GetAllViewModel());
            }

            var series = await _Seriesservice.SearchSeriesByTitleOrProducer(searchTerm);

            ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
            ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();

            if (series.Count == 0)
            {
                ViewBag.NoResults = true;
            }

            return View("Index", series);
        }


        [HttpGet]
        public async Task<IActionResult> FilteredSearch(int? genreId, int? producerId)
        {
            ViewBag.SearchController = "Home";
            ViewBag.SearchAction = "Search";

            var series = await _Seriesservice.GetAllViewModel();

            if (genreId.HasValue && genreId.Value != 0)
            {
                series = series.Where(s => s.GenreId == genreId.Value ||
                                           s.SecondaryGenreIds.Contains(genreId.Value)).ToList();
            }

            if (producerId.HasValue && producerId.Value != 0)
            {
                series = series.Where(s => s.ProducerId == producerId.Value).ToList();
            }

            ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
            ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();

            return View("Index", series);
        }
    }
}