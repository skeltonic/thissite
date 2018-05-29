using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thissite.Models
{
    public class SiteReviews
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Rating { get; set; }
        public string Image { get; set; }
    }
}
