using Application.Services;
using Database.Contexts;
using Microsoft.AspNetCore.Mvc;

public class DetailController : Controller
{
    private readonly SerieService _serieService;

    public DetailController(ApplicationContext dbContext)
    {
        _serieService = new SerieService(dbContext);
    }

    public async Task<IActionResult> Index(int id)
    {
        var detail = await _serieService.GetByIdSerieViewModel(id);
        if (detail == null)
        {
            return NotFound();
        }

        return View(detail); 
    }


}

   




