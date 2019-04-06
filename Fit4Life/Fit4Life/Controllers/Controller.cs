using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fit4Life.Views;
using Fit4Life.Data.Models;
using Fit4Life.Data;
using Fit4Life.Models;
using System.Data.Entity;

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
                    return shopContext.Supplement.ToList();
                case 1:
                    return shopContext.Drinks.ToList();
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
        public decimal GetThePriceOfAllProductsInCart()
        {
            using (shopContext = new ShopContext())
            {
                decimal price = 0;
                foreach (var product in shopContext.Cart.ToList())
                {
                    price += product.Price * product.Quantity;
                }
                return price;
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
                        Supplement supplement = (Supplement)product;
                        shopContext.Supplement.Add(supplement);
                        break;
                    case 1:
                        Drink drink = (Drink)product;
                        shopContext.Drinks.Add(drink);
                        break;
                    case 2:
                        Equipment equipment = (Equipment)product;
                        shopContext.Equipment.Add(equipment);
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
                        Supplement supplement = (Supplement)product;
                        shopContext.Cart.Add(new Cart(supplement.Name, supplement.Price, 1));
                        break;
                    case 1:
                        Drink drink = (Drink)product;
                        shopContext.Cart.Add(new Cart(drink.Name, drink.Price, 1));
                        break;
                    case 2:
                        Equipment equipment = (Equipment)product;
                        shopContext.Cart.Add(new Cart(equipment.Name, equipment.Price, 1));
                        break;
                }
                shopContext.SaveChanges();
            }
            /*using (shopContext = new ShopContext())
            {
                switch (categoryIndex)
                {
                    case 0:
                        Supplement supplement = (Supplement)product;
                        shopContext.Entry(supplement).State = EntityState.Deleted;
                        break;
                    case 1:
                        Supplement supplement = (Supplement)product;
                        shopContext.Entry(supplement).State = EntityState.Deleted;
                        break;
                    case 2:
                        Equipment equipment = (Equipment)product;
                        shopContext.Entry(equipment).State = EntityState.Deleted;
                        break;
                }
                shopContext.SaveChanges();
            }*/
        }
        //update
        internal void DecreaseQuantityOf(object product, int quantity = 1)
        {
            using (var shopContext = new ShopContext())
            {
                dynamic updater;
                switch (product.GetType().Name.ToString())
                {
                    case "Supplement":
                        Supplement supplement = (Supplement)product;
                        updater = shopContext.Supplement.FirstOrDefault(s => s.Id == supplement.Id);
                        updater.Quantity -= quantity;
                        break;
                    case "Drink":
                        Drink drink = (Drink)product;
                        updater = shopContext.Drinks.FirstOrDefault(s => s.Id == drink.Id);
                        updater.Quantity -= quantity;
                        break;
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
                        Supplement supplement = (Supplement)product;
                        Supplement supplementToReplace = shopContext.Supplement.Find(supplement.Id);
                        supplement.Quantity += quantity;
                        shopContext.Entry(supplementToReplace).Entity.Quantity = supplement.Quantity;
                        break;
                    case 1:
                        Drink drink = (Drink)product;
                        Drink drinkToReplace = shopContext.Drinks.Find(drink.Id);
                        drink.Quantity += quantity;
                        shopContext.Entry(drinkToReplace).Entity.Quantity = drink.Quantity;
                        break;
                    case 2:
                        Equipment equipment = (Equipment)product;
                        Equipment equipmentToReplace = shopContext.Equipment.Find(equipment.Id);
                        equipment.Quantity += quantity;
                        shopContext.Entry(equipmentToReplace).Entity.Quantity = equipment.Quantity;
                        break;
                }
                shopContext.SaveChanges();
            }
        }
        /// <summary>
        /// Increases the quantity of a product in shopping cart if exists by name. 
        /// Returns true if quantity increasement succeeded, else false.
        /// </summary>
        /// <param name="product">Product to be found</param>
        /// <param name="quantity">The quantity, which would be added</param>
        internal bool IncreaseQuantityOfCartProductIfExists(object product, int categoryIndex, int quantity = 1)
        {
            using (shopContext = new ShopContext())
            {
                bool isProductInCart = false;
                switch (categoryIndex)
                {
                    case 0:
                        Supplement supplement = (Supplement)product;
                        foreach (var productInCart in shopContext.Cart.ToList())
                        {
                            if (productInCart.Name == supplement.Name)
                            {
                                productInCart.Quantity += quantity;
                                isProductInCart = true;
                                break;
                            }
                        }
                        break;
                    case 1:
                        Drink drink = (Drink)product;
                        foreach (var productInCart in shopContext.Cart.ToList())
                        {
                            if (productInCart.Name == drink.Name)
                            {
                                productInCart.Quantity += quantity;
                                isProductInCart = true;
                                break;
                            }
                        }
                        break;
                    case 2:
                        Equipment equipment = (Equipment)product;
                        foreach (var productInCart in shopContext.Cart)
                        {
                            if (productInCart.Name == equipment.Name)
                            {
                                productInCart.Quantity += quantity;
                                isProductInCart = true;
                                break;
                            }
                        }
                        break;
                }
                if (isProductInCart)
                {
                    shopContext.SaveChanges();
                    return true;
                }
                return false;
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
                        shopContext.Supplement.Attach((Supplement)product);
                        shopContext.Supplement.Remove((Supplement)product);
                        break;
                    case 1:
                        shopContext.Drinks.Attach((Drink)product);
                        shopContext.Drinks.Remove((Drink)product);
                        break;
                    case 2:
                        shopContext.Equipment.Attach((Equipment)product);
                        shopContext.Equipment.Remove((Equipment)product);
                        break;
                }
                shopContext.SaveChanges();
            }
        }
    }
}