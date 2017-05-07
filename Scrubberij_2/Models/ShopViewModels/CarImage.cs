using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scrubberij_2.Models.ShopViewModels
{
    public class CarImage
    {
        [Key]
        public int ID { get; set; }

        public int CarID { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [FileExtensions(Extensions = "jpg, png")]
        public string URL { get; set; }

        public bool IsFirst { get; set; }
    }
}
