using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fit4Life.Controllers;
using Fit4Life.Data;
using Fit4Life.Extentions;
using Fit4Life.Models;

namespace Fit4Life.Views
{
    internal class Display
    {
        Controller controller;
        internal static int pickedOptionIndex = -1;
        internal static decimal shoppingCartTotal = 0;
        private static string optionsString = "Supplements;Drinks;Equipment;Admin login";
        private static int adminOptionIndex = -1;
        internal const int supplementsIndex = 0;
        internal const int drinksIndex = 1;
        internal const int equipmentsIndex = 2;
        private const int adminIndex = 3;
        /// <summary>
        /// Initializes main page headder and sets optionIndex 
        /// to the option the user has selected
        /// </summary>
        internal void OpenHomeView()
        {
            Console.Clear();
            GInterface.SetWindowSize(GInterface.mainPageWindowSize[0], GInterface.mainPageWindowSize[1]);
            Console.WriteLine($"Total: {shoppingCartTotal:f2}bgn");
            GInterface.PrintMainPageHeadder();
            ObjectSelections.OptionsList = GInterface.GetStringListFromString(optionsString, printOptions: true);
            pickedOptionIndex = SelectOption();
        }
        internal void OpenProductsView(int optionIndex)
        {
            Console.ResetColor();
            Console.Clear();
            GInterface.SetWindowSize(60, 80);
            GInterface.PrintProductsPageHeadder(shoppingCartTotal, optionIndex);
            if (optionIndex != -1)
            {
                var productsList = GInterface.GetCategorizedList(optionIndex, controller);
                switch (optionIndex)
                {
                    case supplementsIndex:
                        GInterface.SupplementsList = (List<Supplements>)productsList;
                        break;
                    case drinksIndex:
                        GInterface.DrinksList = (List<Drink>)productsList;
                        break;
                    case equipmentsIndex:
                        GInterface.EquipmentsList = (List<Equipment>)productsList;
                        break;
                    case adminIndex:
                        OpenAdminView();
                        break;
                }
                PrintProductsBasedOnCategory(optionIndex);
                SelectProduct(optionIndex);
            }
            else
            {
                throw new IndexOutOfRangeException("No option was chosen");
            }
        }
        private void OpenAdminView(bool trustedUser = false)
        {
            AdminPanel admin = new AdminPanel();
            int pickedAction = -1;
            Console.Clear();
            if (admin.IsAccessGained() || trustedUser)
            {
                do
                {

                    admin.PrintAdminPageHeadder();
                    pickedAction = admin.SelectAction();
                    admin.TakeAction(pickedAction);
                } while (pickedAction != -1);
            }
            return;
        }
        /// <summary>
        /// Returns the index option selected by the user
        /// </summary>
        private int SelectOption(int optionIndex = 0)
        {
            //the following code enables option selection with arrow keys and enter
            int optIndex = 0;
            int optionsCount = ObjectSelections.OptionsList.Count;
            Console.CursorVisible = false;
            ObjectSelections.SelectCurrentOptionAt(optionIndex);
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter)
            {
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        ObjectSelections.DeselectCurrentOptionAt(optIndex);
                        if (optIndex + 1 >= optionsCount)
                        {
                            optIndex = (optIndex % (optionsCount - 1)) - 1;
                        }
                        ObjectSelections.SelectCurrentOptionAt(++optIndex);

                        break;

                    case ConsoleKey.UpArrow:
                        ObjectSelections.DeselectCurrentOptionAt(optIndex);
                        if (optIndex - 1 < 0)
                        {
                            optIndex += optionsCount;
                        }
                        ObjectSelections.SelectCurrentOptionAt(--optIndex);
                        break;
                    case ConsoleKey.Tab:
                        GInterface.SetWindowSize(GInterface.productPageWindowSize[0], GInterface.productPageWindowSize[1]);
                        GInterface.ShowCart();
                        do
                        {
                            Console.CursorLeft = 0;
                            Console.Write(' ');
                            key = Console.ReadKey(true);
                        } while (key.Key != ConsoleKey.Tab && key.Key != ConsoleKey.Escape);
                        Console.Clear();
                        GInterface.SetWindowSize(GInterface.mainPageWindowSize[0], GInterface.mainPageWindowSize[1]);
                        Console.WriteLine($"Total: {shoppingCartTotal:f2}bgn");
                        GInterface.PrintMainPageHeadder();
                        GInterface.PrintOptions(ObjectSelections.OptionsList);
                        ObjectSelections.SelectCurrentOptionAt(optionIndex);
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Console.WriteLine("Visit us again :)");
                        //Console.ReadKey();
                        Environment.Exit(0);
                        break;
                }
                key = Console.ReadKey(true);
            }
            Console.CursorVisible = true;
            return pickedOptionIndex = optIndex;
        }
        /// <summary>
        /// Enables option selection
        /// </summary>
        /// <param name="optionIndex"></param>
        private void SelectProduct(int optionIndex)
        {
            int productIndex = 0;
            int categoryIndex = -1;
            int listLenght = GInterface.GetListLenghtByCategory(optionIndex);
            ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
            var key = Console.ReadKey(true);
            while (!(key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.Backspace))
            {
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow://select next product
                        ObjectSelections.DeselectCurrentProductAt(productIndex, optionIndex);
                        if (productIndex + 1 >= listLenght)
                        {
                            productIndex = (productIndex % (listLenght - 1)) - 1;
                        }
                        ObjectSelections.SelectCurrentProductAt(++productIndex, optionIndex);
                        break;
                    case ConsoleKey.UpArrow://select previous product
                        ObjectSelections.DeselectCurrentProductAt(productIndex, optionIndex);
                        if (productIndex - 1 < 0)
                        {
                            productIndex = listLenght;
                        }
                        ObjectSelections.SelectCurrentProductAt(--productIndex, optionIndex);
                        break;
                    case ConsoleKey.Enter://add to cart
                        bool isOutOfStock = false;
                        dynamic categorizedList = GInterface.GetCategorizedList(optionIndex, controller);
                        string productType = categorizedList[productIndex].GetType().Name.ToString();
                        object currentProduct = categorizedList[productIndex];
                        switch (currentProduct.GetType().Name.ToString())
                        {
                            case "Supplements":
                                categoryIndex = 0;
                                Supplements supplement = (Supplements)currentProduct;
                                if (supplement.Quantity <= 0)
                                {
                                    isOutOfStock = true;
                                    GInterface.ShowOutOfStockMsg(productIndex);
                                    Console.CursorLeft = 0;
                                    ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
                                }
                                else//add to cart and update database
                                {
                                    controller.DecreaseQuantityOf(currentProduct, 1);
                                    //controller.AddToCart(supplement, categoryIndex);
                                    GInterface.SupplementsList = (List<Supplements>)controller.GetAllBasedOnCategory(supplementsIndex);
                                    ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
                                    shoppingCartTotal += supplement.Price;
                                    GInterface.RefreshCartTotal(shoppingCartTotal);
                                }
                                break;
                            case "Drink":
                                categoryIndex = 1;
                                Drink drink = (Drink)currentProduct;
                                if (drink.Quantity <= 0)
                                {
                                    isOutOfStock = true;
                                    GInterface.ShowOutOfStockMsg(productIndex);
                                    Console.CursorLeft = 0;
                                    ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
                                }
                                else//add to cart and update database
                                {
                                    controller.DecreaseQuantityOf(currentProduct, 1);
                                    //controller.AddToCart(supplement, categoryIndex);
                                    GInterface.DrinksList = (List<Drink>)controller.GetAllBasedOnCategory(drinksIndex);
                                    ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
                                    shoppingCartTotal += drink.Price;
                                    GInterface.RefreshCartTotal(shoppingCartTotal);
                                }
                                break;
                            case "Equipment":
                                categoryIndex = 2;
                                Equipment equipment = (Equipment)currentProduct;
                                if (equipment.Quantity <= 0)
                                {
                                    isOutOfStock = true;
                                    GInterface.ShowOutOfStockMsg(productIndex);
                                    Console.CursorLeft = 0;
                                    ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
                                }
                                else
                                {
                                    controller.DecreaseQuantityOf(currentProduct);
                                    GInterface.EquipmentsList = (List<Equipment>)controller.GetAllBasedOnCategory(2);
                                    ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
                                    GInterface.RefreshCartTotal(shoppingCartTotal);
                                    shoppingCartTotal += equipment.Price;
                                    GInterface.RefreshCartTotal(shoppingCartTotal);
                                }
                                break;
                        }

                        if (!isOutOfStock)
                        {
                            //if product exists in cart, increase ShoppingCartProductCounter by 1;
                            if (GInterface.ObjectListContainsProduct(GInterface.ShoppingCartList, currentProduct, productType))
                            {
                                GInterface.ShoppingCartProductCounter[GInterface.indexerOfProductsCounter]++;
                                controller.IncreaseQuantityOfCartProductIfExists(currentProduct, categoryIndex);
                                GInterface.CartList = controller.GetCart();
                            }
                            //if product does not exist in cart, create it
                            else
                            {
                                //integrated logic prohibits quantity increasing if product alreadt exists
                                if (controller.IncreaseQuantityOfCartProductIfExists(currentProduct, categoryIndex))
                                {
                                    controller.AddToCart(currentProduct, categoryIndex);
                                    GInterface.CartList = controller.GetCart();
                                }
                                GInterface.ShoppingCartProductCounter.Add(1);
                                GInterface.ShoppingCartList.Add(currentProduct);
                            }
                            Console.WriteLine();
                        }
                        break;
                    case ConsoleKey.Tab://show/hide cart
                        do
                        {
                            GInterface.ShowCart();
                            Console.CursorLeft = 0;
                            key = Console.ReadKey(true);

                            /*
                            if (key.Key == ConsoleKey.Spacebar)
                            {
                                GInterface.ShowCartInTableForm(GInterface.CartList, controller.GetThePriceOfAllProductsInCart());
                            }*/
                        } while (key.Key != ConsoleKey.Tab && key.Key != ConsoleKey.Escape);
                        Console.Clear();
                        GInterface.PrintProductsPageHeadder(shoppingCartTotal, optionIndex);
                        GInterface.PrintProductsFormated(optionIndex);
                        ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
                        break;
                }
                key = Console.ReadKey(true);
            }
            Console.Clear();
            return;
        }

        /// <summary>
        /// Displays the products view according to optionIndex
        /// </summary>
        private void PrintProductsBasedOnCategory(int optionIndex)
        {
            switch (optionIndex)
            {
                case supplementsIndex://supplements
                    GInterface.PrintProductsFormated(optionIndex);
                    break;
                case drinksIndex://drinks
                    GInterface.PrintProductsFormated(optionIndex);
                    break;
                case equipmentsIndex://equipment
                    GInterface.PrintProductsFormated(optionIndex);
                    break;
                default:
                    break;
            }
        }
        private void ViewsManager()
        {
            do
            {
                if (pickedOptionIndex == -1)
                {
                    OpenHomeView();
                }
                else if (pickedOptionIndex == adminIndex)
                {
                    OpenAdminView();
                    pickedOptionIndex = -1;
                }
                else
                {
                    OpenProductsView(pickedOptionIndex);
                    pickedOptionIndex = -1;
                }
            } while (true);
        }
        public Display()
        {
            PrintRandomLoadingText();
            GInterface.ShoppingCartList = new List<object>();
            GInterface.ShoppingCartProductCounter = new List<int>();
            controller = new Controller();
            if (((List<Supplements>)controller.GetAllBasedOnCategory(supplementsIndex)).Count <= 1) CreateSampleSupplements();
            if (((List<Drink>)controller.GetAllBasedOnCategory(drinksIndex)).Count <= 1) CreateSampleDrinks();
            if (((List<Equipment>)controller.GetAllBasedOnCategory(equipmentsIndex)).Count <= 1) CreateSampleEquipments();
            GInterface.SupplementsList = (List<Supplements>)GInterface.GetCategorizedList(0, controller);
            GInterface.DrinksList = (List<Drink>)GInterface.GetCategorizedList(1, controller);
            GInterface.EquipmentsList = (List<Equipment>)GInterface.GetCategorizedList(2, controller);
            GInterface.CartList = controller.GetCart();
            ObjectSelections.OptionsList = GInterface.GetStringListFromString(optionsString);
            adminOptionIndex = ObjectSelections.OptionsList.Count - 1;
            ViewsManager();
        }
        private void PrintRandomLoadingText()
        {
            string randomText = "Loading your gainzzz;Abs on the way;Loading the magic pills;" +
                "Pumping your muscles;Burning ze fett;Yo Sexyyy ^o^;Unleashing the GAINZ";
            List<string> randomLoadingMsgs = GInterface.GetStringListFromString(randomText);
            Random randomIndex = new Random();
            Console.Write(randomLoadingMsgs[randomIndex.Next(0, randomLoadingMsgs.Count - 1)]);
            Console.WriteLine("...");
        }
        private void CreateSampleSupplements()
        {
            Supplements supplement;
            for (int i = 1; i <= 2; i++)
            {
                supplement = new Supplements();
                supplement.Name = $"supplement{i}";
                supplement.Brand = $"Brand{i}";
                supplement.Price = i;
                supplement.Weight = i.ToString();
                supplement.Quantity = 0;
                controller.AddProduct(supplement, supplementsIndex);
            }
        }
        private void CreateSampleDrinks()
        {
            Drink drink;
            for (int i = 1; i <= 2; i++)
            {
                drink = new Drink();
                drink.Name = $"drink{i}";
                drink.Mililiters = $"{i * 100}mL";
                drink.Price = i;
                drink.Quantity = 0;
                controller.AddProduct(drink, drinksIndex);
            }
        }
        private void CreateSampleEquipments()
        {
            Equipment equipment;
            for (int i = 1; i <= 2; i++)
            {
                equipment = new Equipment();
                equipment.Name = $"equipment{i}";
                equipment.Brand = $"Brand{i}";
                equipment.Price = i;
                equipment.Quantity = 0;
                controller.AddProduct(equipment, equipmentsIndex);
            }
        }

    }
}