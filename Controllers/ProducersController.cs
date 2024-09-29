using Application.Services;
using Database.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace MiniNetflix.Controllers
{
    public class ProducersController : Controller
    {
        private readonly SerieService _Seriesservice;

        public ProducersController(ApplicationContext dbContext)
        {
            _Seriesservice = new SerieService(dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> Producers()
        {
            var producers = await _Seriesservice.GetAllProducersAsync();
            return View(producers);
        }
    }
}
