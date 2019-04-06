using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Threading;
using Fit4Life.Data.Models;

namespace Fit4Life.Models
{
    class Supplements
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
        public string Weight { get; set; }
        public decimal Price { get; set; }
    }
}
