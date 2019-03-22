using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fit4Life.Views;
using Fit4Life.Data.Models;
using Fit4Life.Data;
using Fit4Life.Models;

namespace Fit4Life.Controllers
{
    public class Controller
    {
        private ShopContext shopContext { get; set; }

        List<Supplement> ShowAllSupplements()
        {
            using(shopContext = new ShopContext())
            {
                return shopContext.Supplements.ToList();
            }
        }

        public void TransferFromShopToCart(int id, int quantity)
        {
            using(shopContext = new ShopContext())
            {
                Supplement supplement = shopContext.Supplements.Find(id);
                supplement.Quantity -= quantity;

               // if (shopContext.Cart.Contains() cant contain anything 
                {
                    //still no idea
                }
            }
        }
        /*private Display display;
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
        }*/
    }
}