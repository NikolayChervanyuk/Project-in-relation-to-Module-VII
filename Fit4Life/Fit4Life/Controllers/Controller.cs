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
    internal class Controller
    {
        private ShopContext shopContext { get; set; }

        internal object GetAllBasedOnCategory(int categoryIndex)
        {
            using (shopContext = new ShopContext())
            {
                return GetCategoryByIndex(categoryIndex);
            }
        }

        internal void DecreaseQuantityOf(object product, int quantity = 1)
        {
            using (var shopContext = new ShopContext())
            {
                dynamic updater;
                switch (product.GetType().Name.ToString())
                {
                    case "Supplements":
                        Supplements supplement = (Supplements)product;
                        updater = shopContext.Supplements.FirstOrDefault(s => s.Id == supplement.Id);
                        updater.Quantity -= quantity;
                        break;
                    /*case "Drink":
                        Supplements supplement = (Supplements)product;
                        var updater = shopContext.Supplements.FirstOrDefault(s => s.Id == supplement.Id);
                        updater.Quantity -= quantity;
                        break;*/
                    case "Equipment":
                        Equipment equipment = (Equipment)product;
                        updater = shopContext.Equipment.FirstOrDefault(s => s.Id == equipment.Id);
                        updater.Quantity -= quantity;
                        break;
                }
                shopContext.SaveChanges();
            }
        }

        private object GetCategoryByIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return shopContext.Supplements.ToList();
                case 1:
                    break;
                case 2:
                    return shopContext.Equipment.ToList();
            }
            return null;
        }

        internal void AddProduct(object product, int categoryIndex)
        {
            using (shopContext = new ShopContext())
            {
                switch (categoryIndex)
                {
                    case 0:
                        Supplements supplement = (Supplements)product;
                        shopContext.Supplements.Add(supplement);
                        break;
                    case 1:
                        break;
                    case 2:
                        Equipment equipment = (Equipment)product;
                        shopContext.Equipment.Add(new Equipment());
                        break;
                }
            }
        }

        /*public void TransferFromShopToCart(int id, int quantity, int index)
        {
            using (shopContext = new ShopContext())
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
        }*/
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