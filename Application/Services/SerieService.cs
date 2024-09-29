using Application.Repository;
using Application.ViewModels;
using Database.Contexts;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SerieService
    {
        private readonly SerieRepository _SeriesRepository;
        private readonly ApplicationContext _dbContext;

        public SerieService(ApplicationContext dbContext)
        {
            _SeriesRepository = new SerieRepository(dbContext);
            _dbContext = dbContext; 
        }


        public async Task<List<SerieViewModel>> GetAllViewModel()
        {
            var seriesList = await _SeriesRepository.GetAllSeriesWithRelations().ToListAsync();

            return seriesList.Select(series => new SerieViewModel
            {
                Id = series.Id,
                Title = series.Title,
                VideoUrl = series.VideoUrl,
                ImageUrl = series.ImageUrl,
                Description = series.Description,
                GenreId = series.GenreId ?? 0, 
                GenreTitle = series.Genre?.Name ?? "Sin género",
                ProducerId = series.ProducerId ?? 0,
                ProducerTitle = series.Producers?.Name ?? "Sin productor",
                SecondaryGenreIds = series.SecondaryGenres
                        .Select(g => g.GenreId ?? 0) 
                        .ToList(),
                SecondaryGenres = series.SecondaryGenres?.Select(gs => gs.Genre?.Name).ToList() ?? new List<string>()
            }).ToList();
        }



        public async Task<List<SerieViewModel>> SearchSeriesByTitleOrProducer(string searchTerm)
        {
            var seriesList = await _SeriesRepository.GetAllSeriesWithRelations()
                .Where(s => s.Title.Contains(searchTerm) ||
                            (s.Producers != null && s.Producers.Name.Contains(searchTerm)))
                .ToListAsync();

            return seriesList.Select(series => new SerieViewModel
            {
                Id = series.Id,
                Title = series.Title,
                VideoUrl = series.VideoUrl,
                ImageUrl = !string.IsNullOrEmpty(series.ImageUrl) ? series.ImageUrl : "URL de imagen por defecto",
                Description = series.Description,
                GenreId = series.GenreId ?? 0, 
                GenreTitle = series.Genre?.Name ?? "Sin género", 
                ProducerId = series.ProducerId ?? 0,
                ProducerTitle = series.Producers?.Name ?? "Sin productor", 
                SecondaryGenres = series.SecondaryGenres?.Select(gs => gs.Genre?.Name).ToList() ?? new List<string>()
            }).ToList();
        }


        public async Task<List<SerieViewModel>> GetAllSeriesWithGenresAndProducerAsync()
        {
            var seriesWithGenresAndProducer = await _SeriesRepository.GetAllSeriesWithRelations()
                .Select(s => new SerieViewModel
                {
                    Id = s.Id,
                    Title = s.Title,
                    ImageUrl = s.ImageUrl,
                    GenreId = s.GenreId ?? 0, 
                    GenreTitle = s.Genre.Name,
                    ProducerId = s.ProducerId ?? 0,
                    ProducerTitle = s.Producers.Name,
                    SecondaryGenres = s.SecondaryGenres.Select(gs => gs.Genre.Name).ToList()
                })
                .ToListAsync();

            return seriesWithGenresAndProducer;
        }


        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _dbContext.Genres.ToListAsync();
        }

        public async Task<List<Producer>> GetAllProducersAsync()
        {
            return await _dbContext.Producers.ToListAsync();
        }

        public async Task<Serie> GetByIdAsync(int id)
        {
            return await _SeriesRepository.GetByIdAsync(id);  
        }
        public async Task<SaveSeriesViewModel> GetByIdSaveViewModel(int id)
        {
            var serie = await _SeriesRepository.Series

                .Include(s => s.Genre) 
                .Include(s => s.Producers) 
                .Include(s => s.SecondaryGenres) 
                    .ThenInclude(gs => gs.Genre) 
                .Where(s => s.Id == id) 
                .FirstOrDefaultAsync(); 

            if (serie == null)
            {
                return null; 
            }

            var saveSerieViewModel = new SaveSeriesViewModel
            {
                Id = serie.Id,
                Title = serie.Title,
                Description = serie.Description,
                VideoUrl = serie.VideoUrl,
                ImageUrl = serie.ImageUrl,
                GenreId = serie.GenreId ?? 0, 
                ProducerId = serie.ProducerId ?? 0,
                SecondaryGenreIds = serie.SecondaryGenres
                        .Select(g => g.GenreId ?? 0) 
                        .ToList() 
            };

            return saveSerieViewModel;
        }

        public async Task<SerieViewModel> GetByIdSerieViewModel(int id)
        {
            var serie = await _SeriesRepository.Series

                .Include(s => s.Genre)
                .Include(s => s.Producers) 
                .Include(s => s.SecondaryGenres) 
                    .ThenInclude(gs => gs.Genre) 
                .Where(s => s.Id == id) 
                .FirstOrDefaultAsync(); 

            if (serie == null)
            {
                return null; 
            }

            var SerieViewModel = new SerieViewModel
            {
                Id = serie.Id,
                Title = serie.Title,
                Description = serie.Description,
                VideoUrl = serie.VideoUrl,
                ImageUrl = serie.ImageUrl,
                GenreId = serie.GenreId ?? 0,
                ProducerId = serie.ProducerId ?? 0,
                SecondaryGenreIds = serie.SecondaryGenres
                        .Select(g => g.GenreId ?? 0) 
                        .ToList()  
            };

            return SerieViewModel;
        }
        public async Task Update(SaveSeriesViewModel vm)
        {
            var existingSerie = await _SeriesRepository.GetByIdAsync(vm.Id);

            if (existingSerie != null)
            {
                existingSerie.Title = vm.Title;
                existingSerie.ImageUrl = vm.ImageUrl;
                existingSerie.Description = vm.Description;
                existingSerie.VideoUrl = vm.VideoUrl;
                existingSerie.GenreId = vm.GenreId;
                existingSerie.ProducerId = vm.ProducerId;

                existingSerie.SecondaryGenres = vm.SecondaryGenreIds?.Select(id => new GenreSerie { GenreId = id }).ToList();

                await _SeriesRepository.UpdateAsync(existingSerie);
            }
        }
        public async Task Add(SaveSeriesViewModel vm)
        {
            Serie serie = new Serie
            {
                Title = vm.Title,
                ImageUrl = vm.ImageUrl,
                Description = vm.Description,
                VideoUrl = vm.VideoUrl,
                GenreId = vm.GenreId,
                ProducerId = vm.ProducerId,
                SecondaryGenres = vm.SecondaryGenreIds?.Select(id => new GenreSerie { GenreId = id }).ToList()
            };

            await _SeriesRepository.AddAsync(serie);
        }

        public async Task Delete(int id)
        {
            await _SeriesRepository.DeleteAsync(id); 
        }









    }


}
