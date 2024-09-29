using Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class SerieViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string VideoUrl { get; set; }

        public string ImageUrl { get; set; }
        public string Description { get; set; }

        public int GenreId { get; set; }  
        public string GenreTitle { get; set; }  

        public int ProducerId { get; set; }
        public string ProducerTitle { get; set; }

        public List<int> SecondaryGenreIds { get; set; } 

        public List<string> SecondaryGenres { get; set; } 
    }

}