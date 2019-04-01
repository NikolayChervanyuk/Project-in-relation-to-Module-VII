﻿using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Returns the index option selected by the user
        /// </summary>
        /// <returns></returns>
        private int SelectOption(int optionIndex = 0)
        {
            //int pickedOptionIndex = -1;
            //the following code enables option selection with arrow keys and enter
            int optIndex = 0;
            int optionsCount = ObjectSelections.OptionsList.Count;
            Console.CursorVisible = false;
            ObjectSelections.SelectCurrentOptionAt(optionIndex);
            ConsoleKeyInfo key = Console.ReadKey(true);
            //Loop, while option is not selected
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

        //Displays the products view according to optionIndex
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
                    case 0:
                        GInterface.SupplementsList = (List<Supplements>)productsList;
                        break;
                    case 1:
                        //GInterface.DrinksList = (List<Drink>)productsList;
                        break;
                    case 2:
                        GInterface.EquipmentsList = (List<Equipment>)productsList;
                        break;
                    case 3:
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
                    //System.Threading.Thread.Sleep(1000);
                    pickedAction = admin.SelectAction();
                    admin.TakeAction(pickedAction);
                } while (pickedAction != -1);
            }
            //OpenHomeView();
            return;
        }

        private void PrintProductsBasedOnCategory(int optionIndex)
        {
            switch (optionIndex)
            {
                case 0://supplements
                    GInterface.PrintProductsFormated(optionIndex);
                    break;
                case 1://drinks
                    //GInterface.PrintProductFormated(GInterface.DrinksList, optionIndex);
                    break;
                case 2://equipment
                    GInterface.PrintProductsFormated(optionIndex);
                    break;
                default:
                    break;
            }
        }
        private void SelectProduct(int optionIndex)
        {
            int productIndex = 0;
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
                        //int quantity = GInterface.ShoppingCartProductCounter[GInterface.IndexOfProductInCart(currentProduct, optionIndex)];
                        switch (currentProduct.GetType().Name.ToString())
                        {
                            case "Supplements":
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
                                    GInterface.SupplementsList = (List<Supplements>)controller.GetAllBasedOnCategory(0);
                                    ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
                                    shoppingCartTotal += supplement.Price;
                                    GInterface.RefreshCartTotal(shoppingCartTotal);
                                }
                                break;
                            case "Drink":
                                /*Drink drink = (Drink)product;
                                  
                                 */
                                break;
                            case "Equipment":
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
                            }
                            //if product does not exist in cart, create it
                            else
                            {

                                GInterface.ShoppingCartProductCounter.Add(1);
                                GInterface.ShoppingCartList.Add(currentProduct);
                            }
                            Console.WriteLine();
                        }
                        break;
                    case ConsoleKey.Tab://show/hide cart
                        GInterface.ShowCart();
                        do
                        {
                            Console.CursorLeft = 0;
                            Console.Write(' ');
                            key = Console.ReadKey(true);
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
            OpenHomeView();
        }

        private void ManageViews()
        {
            do
            {
                if (pickedOptionIndex == -1)
                {
                    OpenHomeView();
                }
                else if (pickedOptionIndex == 3)
                {
                    OpenAdminView();
                    pickedOptionIndex = -1;
                }
                else
                {
                    OpenProductsView(pickedOptionIndex);
                }
            } while (true);
        }
        public Display()
        {
            Console.WriteLine("Initializing, please wait...");
            GInterface.ShoppingCartList = new List<object>();
            GInterface.ShoppingCartProductCounter = new List<int>();
            controller = new Controller();
            GInterface.SupplementsList = (List<Supplements>)GInterface.GetCategorizedList(0,controller);
            //GInterface.DrinkssList = (List<Equipment>)GInterface.GetCategorizedList(1, controller);
            GInterface.EquipmentsList = (List<Equipment>)GInterface.GetCategorizedList(2, controller);
            ObjectSelections.OptionsList = GInterface.GetStringListFromString(optionsString);
            adminOptionIndex = ObjectSelections.OptionsList.Count - 1;
            ManageViews();
        }
    }
}