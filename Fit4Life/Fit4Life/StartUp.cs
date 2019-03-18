using Fit4Life.Controllers;
using Fit4Life.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fit4Life
{
    class StartUp
    {
        static void Main(string[] args)
        {
            var controller = new Controller();
            //initiating the program
            controller.Start();    
        }
    }
}
