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
        public async Task<IActionResult> Index()
        {
            ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
            ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();
            var series = await _Seriesservice.GetAllViewModel();
            return View(series);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
            ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();
            return View("SaveSeries", new SaveSeriesViewModel()); // Asegúrate de que el nombre de la vista es correcto
        }


        [HttpPost]
        public async Task<IActionResult> Create(SaveSeriesViewModel vm)
        {
            // Valida si el género primario ya está en los géneros secundarios
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
                return View("SaveSeries", vm); // Retorna la vista con errores si el modelo no es válido
            }

            await _Seriesservice.Add(vm);
            return RedirectToAction("Index");
        }

        // Método para editar la serie

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var serie = await _Seriesservice.GetByIdAsync(id);

            if (serie == null)
            {
                return NotFound();
            }

            // Mapea la serie a SaveSeriesViewModel
            var vm = new SaveSeriesViewModel
            {
                Id = serie.Id,
                Title = serie.Title,
                VideoUrl = serie.VideoUrl,
                ImageUrl = serie.ImageUrl,
                Description = serie.Description,
                GenreId = serie.GenreId,
                ProducerId = serie.ProducerId,
                SecondaryGenreIds = serie.SecondaryGenres.Select(g => g.GenreId).ToList()
            };

            ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
            ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();

            return View("SaveSeries", vm); // Usa la misma vista de Create para editar
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveSeriesViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, recarga el formulario con los datos actuales
                ViewBag.Genres = await _Seriesservice.GetAllGenresAsync();
                ViewBag.Producers = await _Seriesservice.GetAllProducersAsync();
                return View("SaveSeries", vm); // Retorna la vista con los errores de validación
            }

            // Aquí se llama al servicio para actualizar los datos de la serie en la base de datos
            await _Seriesservice.Update(vm);

            // Redirige a la página principal de las series después de actualizar
            return RedirectToAction("Index");
        }






    }

}