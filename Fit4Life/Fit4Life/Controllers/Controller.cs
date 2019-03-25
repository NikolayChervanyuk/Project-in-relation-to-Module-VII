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

        public object GetAllBasedOnCategory(int categoryIndex)
        {
            using(shopContext = new ShopContext())
            {

                return shopContext.GetCategoryByIndex(categoryIndex);
            }
        }

        public void TransferFromShopToCart(int id, int quantity)
        {
            using(shopContext = new ShopContext())
            {
                Supplements supplement = shopContext.Supplements.Find(id);
                supplement.Quantity -= quantity;

                foreach (var item in shopContext.Cart)
                {
                    if(item.Name == supplement.Name)
                    {
                        item.Quantity++;
                        shopContext.SaveChanges();
                        break;
                    }
                }

                shopContext.Cart.Add(new Cart(supplement.Name, supplement.Price, quantity));
                shopContext.SaveChanges();
            }
        }

        void AddProduct(Supplements supplement)
        {
            using (shopContext = new ShopContext())
            {
                shopContext.Supplements.Add(supplement);
            }
        }

        void AddEquipment(Equipment equipment)
        {
            using (shopContext = new ShopContext())
            {
                shopContext.Equipment.Add(equipment);
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