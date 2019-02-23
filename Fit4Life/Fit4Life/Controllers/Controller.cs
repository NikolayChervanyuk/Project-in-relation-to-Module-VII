using Fit4Life.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit4Life.Controllers
{
    public class Controller
    {
        private Display display = new Display();
        //private Model model = new Model(); //models logic is pending...
        internal void Start()
        {
            display = new Display();
        }
    }
}
