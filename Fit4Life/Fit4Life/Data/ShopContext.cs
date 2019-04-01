﻿using Fit4Life.Data.Models;
using Fit4Life.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit4Life.Data
{
    class ShopContext : DbContext
    {
        public ShopContext()
            :base("name = ShopContext") { }

        
        public DbSet<Supplements> Supplements { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Cart> Cart { get; set; }

    }
}
