using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fit4Life.Views;
using Fit4Life.Models;

namespace Fit4Life.Controllers
{
    public class Controller
    {
        private Display display;
        private DataManagement dataManagement;
        internal void Start()
        {
            display = new Display();
            dataManagement = new DataManagement();
            //dataManagement.EstablishDataBaseConnection("fit4life", "connectionTester", "1234");

            while (true)
            {
                dataManagement.FetchDataBasedOn(Display.pickedOptionIndex);
                display.OpenProductsView(dataManagement.CurrentOptionIndex);
            }
            //dataManagement.connection.Close();
        }
    }
}