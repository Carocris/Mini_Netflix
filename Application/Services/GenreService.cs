using Application.Repository;
using Application.ViewModels;
using Database.Contexts;
using Database.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GenreService
    {
        private readonly GenreRepository _genreRepository;

        public GenreService(ApplicationContext dbContext)
        {
            _genreRepository = new GenreRepository(dbContext);
        }

        public async Task<List<GenreViewModel>> GetAllViewModel()
        {
            var genres = await _genreRepository.GetAllGenresAsync();
            return genres.Select(genre => new GenreViewModel
            {
                Id = genre.Id,
                Name = genre.Name
            }).ToList();
        }

        public async Task<SaveGenreViewModel> GetByIdSaveViewModel(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
                return null;

            return new SaveGenreViewModel
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public async Task Add(SaveGenreViewModel vm)
        {
            var genre = new Genre
            {
                Name = vm.Name
            };

            await _genreRepository.AddAsync(genre);
        }

        public async Task Update(SaveGenreViewModel vm)
        {
            Genre genre = new Genre
            {
                Id = vm.Id,
                Name = vm.Name
            };

            await _genreRepository.UpdateAsync(genre);
        }

        public async Task Delete(int id)
        {
            await _genreRepository.DeleteAsync(id);
        }
    }
}
