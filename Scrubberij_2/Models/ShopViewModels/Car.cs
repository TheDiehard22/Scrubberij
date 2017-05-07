using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scrubberij_2.Models.ShopViewModels
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        public string Merk { get; set; }

        public string Type { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Bouwjaar { get; set; }

        public decimal Prijs { get; set; }

        public int Kilometerstand { get; set; }

        public string Brandstof { get; set; }

        public string Transmissie { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("APK verloopdatum")]
        public DateTime ApkVerloopDatum { get; set; }

        public bool BTW { get; set; }

        public List<CarImage> Fotos { get; set; }

        [DisplayName("Extra informatie")]
        [DataType(DataType.MultilineText)]
        public string ExtraInformatie { get; set; }

        public ICollection<Comment> Comments { get; set; }

    }
}
