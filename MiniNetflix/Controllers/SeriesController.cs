using Application.Services;
using Application.ViewModels;
using Database.Contexts;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MiniNetflix.Controllers
{
    public class SeriesController : Controller
    {
        private readonly SerieService _Seriesservice;

        public SeriesController(ApplicationContext dbContext)
        {
            _Seriesservice = new SerieService(dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm, int? genreId, int? producerId)
        {

            ViewBag.SearchController = "Series";
            ViewBag.SearchAction = "Search";
            ViewBag.SearchTerm = searchTerm;


            // Obtener todos los géneros y productoras para el filtro
            ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
            ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();

            // Obtener todas las series
            var series = await _Seriesservice.GetAllViewModel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                series = series.Where(s => s.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filtrar por género
            if (genreId.HasValue && genreId.Value > 0)
            {
                series = series.Where(s => s.GenreId == genreId.Value || s.SecondaryGenreIds.Contains(genreId.Value)).ToList();

            }

            // Filtrar por productora
            if (producerId.HasValue && producerId > 0)
            {
                series = series.Where(s => s.ProducerId == producerId).ToList();
            }

            // Retornar las series filtradas
            return View(series);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return View("Index", await _Seriesservice.GetAllViewModel());
            }

            var series = await _Seriesservice.SearchSeriesByTitleOrProducer(searchTerm);

            // Asegúrate de cargar los géneros y productores aquí
            ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
            ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();

            if (series.Count == 0)
            {
                ViewBag.NoResults = true;
            }

            return View("Index", series);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
            ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();
            return View("SaveSeries", new SaveSeriesViewModel()); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveSeriesViewModel vm)
        {
            if (vm.SecondaryGenreIds == null)
            {
                vm.SecondaryGenreIds = new List<int>();
            }

            if (vm.SecondaryGenreIds.Contains(vm.GenreId))
            {
                ModelState.AddModelError("", "El género primario no puede ser seleccionado como género secundario.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();
                ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
                return View("SaveSeries", vm); 
            }

            await _Seriesservice.Add(vm);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Save(SaveSeriesViewModel vm)
        {
            if (vm.SecondaryGenreIds == null)
            {
                vm.SecondaryGenreIds = new List<int>();
            }

            if (vm.SecondaryGenreIds.Contains(vm.GenreId))
            {
                ModelState.AddModelError("", "El género principal no puede ser seleccionado como género secundario.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();
                ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
                return View("SaveSeries", vm); 
            }

            if (vm.Id == 0) 
            {
                await _Seriesservice.Add(vm);
            }
            else 
            {
                await _Seriesservice.Update(vm);
            }

            return RedirectToAction("Index"); 
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var serie = await _Seriesservice.GetByIdAsync(id);

            if (serie == null)
            {
                return NotFound();
            }

            var vm = new SaveSeriesViewModel
            {
                Id = serie.Id,
                Title = serie.Title,
                VideoUrl = serie.VideoUrl,
                ImageUrl = serie.ImageUrl,
                Description = serie.Description,
                GenreId = serie.GenreId ?? 0,  
                ProducerId = serie.ProducerId ?? 0,
                SecondaryGenreIds = serie.SecondaryGenres
                         .Select(g => g.GenreId ?? 0) 
                         .ToList()  
            };


            ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
            ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();

            return View("SaveSeries", vm); 
        }


        [HttpPost]
        public async Task<IActionResult> Edit(SaveSeriesViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
                ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();
                return View("SaveSeries", vm); 
            }

            await _Seriesservice.Update(vm);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var serie = await _Seriesservice.GetByIdAsync(id);
            if (serie == null)
            {
                return NotFound(); // Manejo en caso de que la serie no exista
            }

            await _Seriesservice.Delete(id); // Llamada al método Delete en SerieService
            return RedirectToAction("Index"); // Redirige a la lista después de eliminar
        }













    }
}
