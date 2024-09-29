using Application.ViewModels;
using Application.Services;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Database.Contexts;

namespace MiniNetflix.Controllers
{
    public class ProducersController : Controller
    {
        private readonly ProducerService _producerService;

        public ProducersController(ApplicationContext dbContext)
        {
            _producerService = new (dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            ViewBag.SearchController = "Producers";
            ViewBag.SearchAction = "Search";
            ViewBag.SearchTerm = searchTerm;

            var producers = await _producerService.GetAllViewModel(); 

            if (!string.IsNullOrEmpty(searchTerm))
            {
                producers = producers.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(producers); 
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            ViewBag.SearchController = "Producers";
            ViewBag.SearchAction = "Search";
            ViewBag.SearchTerm = searchTerm;

            var producers = await _producerService.GetAllViewModel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                producers = producers.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View("Index", producers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View("SaveProducer", new SaveProducerViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveProducerViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveProducer", vm); 
            }

            await _producerService.Add(vm);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var producer = await _producerService.GetByIdSaveViewModel(id);

            if (producer == null)
            {
                return NotFound();
            }

            var vm = new SaveProducerViewModel
            {
                Id = producer.Id,
                Name = producer.Name
            };

            return View("SaveProducer", vm); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveProducerViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveProducer", vm); 
            }

            await _producerService.Update(vm);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var producer = await _producerService.GetByIdSaveViewModel(id);
            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _producerService.Delete(id);
            return RedirectToRoute(new { controller = "Producers", action = "Index" });
        }
    }
}
