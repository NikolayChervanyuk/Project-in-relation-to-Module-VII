using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit4Life.Data.Models
{
    abstract class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        Category supplement { get; set; }
        public int Quantity { get; set; }
        public string Weight { get; set; }
        public decimal Price { get; set; }
    }
}
