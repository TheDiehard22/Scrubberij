using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scrubberij_2.Models.ShopViewModels
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Voornaam { get; set; }

        [Required]
        public string Achternaam { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        public string Tekst { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy - HH:mm}")]
        public DateTime Datum { get; set; }

        public int CarId { get; set; }

        public virtual Car Car { get; set; }

        public Comment()
        {
            this.Datum = DateTime.Now;
        }
    }
}
