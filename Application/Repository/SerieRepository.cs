using Database.Contexts;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class SerieRepository
    {
        private readonly ApplicationContext _dbContext;

        public SerieRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<Serie> Series => _dbContext.Series;


        public async Task AddAsync(Serie Series)
        {
            await _dbContext.Series.AddAsync(Series);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Serie Series)
        {

            _dbContext.Entry(Series).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var serie = await _dbContext.Set<Serie>().FindAsync(id);

            if (serie != null)
            {
                var generoSeries = _dbContext.GenreSeries.Where(gs => gs.SerieId == serie.Id);
                _dbContext.GenreSeries.RemoveRange(generoSeries);

                _dbContext.Set<Serie>().Remove(serie);
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<List<Serie>> GetAllAsync()
        {
            return await _dbContext.Series.ToListAsync() ?? new List<Serie>();
        }

        public async Task<Serie> GetByIdAsyn(int id)
        {
            return await _dbContext.Set<Serie>().FindAsync(id);
        }

        public IQueryable<Serie> GetAllSeriesWithRelations()
        {
            return _dbContext.Series
                .Include(s => s.Genre)              
                .Include(s => s.Producers)          
                .Include(s => s.SecondaryGenres)    
                    .ThenInclude(gs => gs.Genre);    
        }

        

        public async Task<Serie> GetByIdAsync(int id)
        {
            return await _dbContext.Series
                .Include(s => s.Genre)              
                .Include(s => s.SecondaryGenres)     
                .ThenInclude(gs => gs.Genre)         
                .FirstOrDefaultAsync(s => s.Id == id); 
        }




    }
}