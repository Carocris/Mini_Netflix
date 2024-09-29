using Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class SaveSeriesViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The video URL is required.")]
        public string VideoUrl { get; set; }

        [Required(ErrorMessage = "The image URL is required.")]
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "The genre is required.")]
        public int GenreId { get; set; }  

        [Required(ErrorMessage = "The producer is required.")]
        public int ProducerId { get; set; } 

        public List<int> SecondaryGenreIds { get; set; }  




    }
}