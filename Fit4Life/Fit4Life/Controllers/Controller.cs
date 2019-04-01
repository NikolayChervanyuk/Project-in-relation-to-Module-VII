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

        //read
        internal object GetAllBasedOnCategory(int categoryIndex)
        {
            using (shopContext = new ShopContext())
            {
                return GetCategoryByIndex(categoryIndex);
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

        internal List<Cart> GetCart()
        {
            using (shopContext = new ShopContext())
            {
                return shopContext.Cart.ToList();
            }
        }
        
        //create
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
                        shopContext.Equipment.Add(equipment);
                        break;
                }
                shopContext.SaveChanges();
            }
        }

        //update
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
        internal void IncreaseQuantityOf(object product, int categoryIndex, int quantity)
        {
            using (shopContext = new ShopContext())
            {
                switch (categoryIndex)
                {
                    case 0:
                        Supplements supplement = (Supplements)product;
                        Supplements supplementToReplace = shopContext.Supplements.Find(supplement.Id);
                        supplement.Quantity += quantity;
                        shopContext.Entry(supplementToReplace).Entity.Quantity = supplement.Quantity;
                        break;
                    case 1:
                        break;
                    case 2:
                        Equipment equipment = (Equipment)product;
                        Equipment equipmentToReplace = shopContext.Equipment.Find(equipment.Id);
                        equipment.Quantity += quantity;
                        shopContext.Entry(equipmentToReplace).Entity.Quantity = equipment.Quantity;
                        break;
                }
            }
        }

        //delete
        internal void DeleteProduct(object product, int categoryIndex)
        {
            using (shopContext = new ShopContext())
            {
                switch (categoryIndex)
                {
                    case 0:
                        // Supplements supplement = shopContext.Supplements.Find(id) ;
                        shopContext.Supplements.Remove((Supplements)product);
                        break;
                    case 1:
                        break;
                    case 2:
                        //   Equipment equipment = shopContext.Equipment.Find(id);
                        //    shopContext.Equipment.Remove(equipment);
                        break;
                }
                shopContext.SaveChanges();
            }
        }

        internal void AddToCart(object product, int categoryIndex)
        {
            using (shopContext = new ShopContext())
            {
                switch (categoryIndex)
                {
                    case 0:
                        Supplements supplement = (Supplements)product ;
                        shopContext.Cart.Add(new Cart(supplement.Name, supplement.Price, 1));
                        break;
                    case 1:
                        break;
                    case 2:
                        Equipment equipment = (Equipment)product;
                        shopContext.Cart.Add(new Cart(equipment.Name, equipment.Price, 1));
                        break;
                }
                shopContext.SaveChanges();
            }
        }

        internal void IncreaseQuantityOfCartProduct(object product, int categoryIndex, int quantity = 1)
        {
            using (shopContext = new ShopContext())
            {
                if(shopContext.Cart.Count() == 0)
                {
                    return;
                }
                switch (categoryIndex)
                {
                    case 0:
                        Supplements supplement = (Supplements)product;
                        foreach (var productInCart in shopContext.Cart.ToList())
                        {
                            if(productInCart.Name == supplement.Name)
                            {
                                productInCart.Quantity += quantity;
                                break;
                            }
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        Equipment equipment = (Equipment)product;
                        foreach (var productInCart in shopContext.Cart)
                        {
                            if (productInCart.Name == equipment.Name)
                            {
                                productInCart.Quantity += quantity;
                                break;
                            }
                        }
                        break;
                }
                shopContext.SaveChanges();
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