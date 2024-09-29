using Application.Repository;
using Application.ViewModels;
using Database.Contexts;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProducerService
    {
        private readonly ProducerRepository _producerRepository;

        public ProducerService(ApplicationContext dbContext)
        {
            _producerRepository =  new (dbContext);
        }

        public async Task<List<ProducerViewModel>> GetAllViewModel()
        {
            var producers = await _producerRepository.GetAllProducersAsync();
            return producers.Select(producer => new ProducerViewModel
            {
                Id = producer.Id,
                Name = producer.Name
            }).ToList();
        }

        public async Task<SaveProducerViewModel> GetByIdSaveViewModel(int id)
        {
            var producer = await _producerRepository.GetByIdAsync(id);
            if (producer == null)
                return null;

            return new SaveProducerViewModel
            {

                Id = producer.Id,
                Name = producer.Name
            };
        }

        public async Task Add(SaveProducerViewModel vm)
        {
            var producer = new Producer
            {
                Name = vm.Name
            };

            await _producerRepository.AddAsync(producer);
        }

        public async Task Update(SaveProducerViewModel vm)
        {
            Producer producer = new()
            {
                Id = vm.Id,
                Name = vm.Name
            };

            await _producerRepository.UpdateAsync(producer);
        }

        public async Task Delete(int id)
        {
            await _producerRepository.DeleteAsync(id);
        }
    }
}
