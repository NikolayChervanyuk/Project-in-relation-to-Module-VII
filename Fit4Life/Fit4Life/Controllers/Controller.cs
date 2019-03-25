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

        public void TransferFromShopToCart(int id, int quantity,int index)
        {
            using(shopContext = new ShopContext())
            {
                switch (index)
                {
                    case 0:
                        Supplements supplement = shopContext.Supplements.Find(id);
                        supplement.Quantity -= quantity;

                        foreach (var item in shopContext.Cart)
                        {
                            if (item.Name == supplement.Name)
                            {
                                item.Quantity++;
                                shopContext.SaveChanges();
                                break;
                            }
                        }

                        shopContext.Cart.Add(new Cart(supplement.Name, supplement.Price, quantity));
                        shopContext.SaveChanges();
                        break;

                    case 1:
                        break;

                    case 2:
                        Equipment equipment = shopContext.Equipment.Find(id);
                        equipment.Quantity -= quantity;

                        foreach (var item in shopContext.Cart)
                        {
                            if (item.Name == equipment.Name)
                            {
                                item.Quantity++;
                                shopContext.SaveChanges();
                                break;
                            }
                        }

                        shopContext.Cart.Add(new Cart(equipment.Name, equipment.Price, quantity));
                        shopContext.SaveChanges();
                        break;


                    default:
                        break;
                }
            }
        }

        void AddProduct(Product product, int index)
        {
            using (shopContext = new ShopContext())
            {
                switch (index)
                {
                    case 0:
                        shopContext.Supplements.Add(new Supplements());
                        break;
                    case 1:
                        break;
                    case 2:
                        shopContext.Equipment.Add(new Equipment());
                        break;
                    default:
                        break;
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