using Fit4Life.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit4Life.Data.Models
{
    class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Supplement> Supplements{ get; set; }

        public Category()
        {
            Supplements = new List<Supplement>();
        }
    }
}
