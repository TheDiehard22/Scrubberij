using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrubberij_2.Models.ShopViewModels
{
    public class ShopIndex
    {
        public Car Car { get; set; }

        public Comment Comment { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
