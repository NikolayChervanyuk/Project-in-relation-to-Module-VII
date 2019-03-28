using Fit4Life.Controllers;
using Fit4Life.Data;
using Fit4Life.Data.Models;
using Fit4Life.Models;
using Fit4Life.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Fit4Life.Extentions
{
    internal static class GInterface
    {

        //internal static List<Type> ProductsList { get; set; }
        internal static List<object> ShoppingCartList { get; set; }
        internal static List<int> ShoppingCartProductCounter { get; set; }

        internal static List<Supplements> SupplementsList { get; set; }
        //internal static List<Drink> SupplementsList { get; set; }
        internal static List<Equipment> EquipmentsList { get; set; }
        internal static int indexerOfProductsCounter = 0;

        //Resolution ratios respectively width to height, height to width
        private static readonly int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        private static readonly int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        private static double widthCoeff = (double)screenWidth / screenHeight;
        private static double heightCoeff = (double)screenHeight / screenWidth;

        internal static void SetWindowSize(int windowWidth, int windowHeight)
        {
            Console.SetWindowSize((int)(windowWidth * widthCoeff),
                                  (int)(windowHeight * heightCoeff));

            Console.SetBufferSize((int)(windowWidth * widthCoeff),
                                  (int)(windowHeight * heightCoeff * 10));
        }

        internal static void PrintMainPageHeadder()
        {
            Console.WriteLine(HorizontalLine('-', 23));
            ShiftText(5);
            Console.WriteLine("<|Fit 4 Life|>");
            Console.WriteLine("Welcome to our shop!");
            Console.WriteLine(HorizontalLine('-', 23));
            ObjectSelections.TopOffset = Console.CursorTop;
            ObjectSelections.LeftOffset = Console.CursorLeft;
        }
        internal static void PrintProductsPageHeadder(decimal shoppingCartTotal, int optionIndex)
        {
            Console.WriteLine("Navigation:\nArrows up/down - scroll \nEnter - add to cart\t  Esc - return to main menu");
            Console.WriteLine($"\nShopping cart: {shoppingCartTotal:f2}bgn");
            Console.WriteLine(HorizontalLine('-', 100));
            ShiftText(45);
            Console.WriteLine(ObjectSelections.OptionsList[optionIndex]);
            Console.WriteLine(HorizontalLine('-', 100));
            ObjectSelections.TopOffset = Console.CursorTop;
            ObjectSelections.LeftOffset = Console.CursorLeft;
        }

        /// <summary>
        /// Generates list of type string by providing a string. Each word, separated by semicolon gets added to the list
        /// </summary>
        /// <param name="optionsString">Separate each option with semicolon(;)</param>
        /// <param name="printOptions">If set true, prints the list</param>
        internal static List<string> GetStringListByString(string optionsString, bool printOptions = false)
        {
            var OptionsList = ObjectSelections.OptionsList;
            if (OptionsList == null)
            {
                OptionsList = new List<string>();
                //Here we add the options which would be displayed in our main page.
                //! Separate each object with (;)
                int separatorInd = 0;
                int ind = -1;
                while (++ind < optionsString.Length)
                {
                    if (optionsString[ind] == ';')
                    {
                        OptionsList.Add(optionsString.Substring(separatorInd, ind - separatorInd));
                        separatorInd = ind + 1;
                    }
                }
                OptionsList.Add(optionsString.Substring(separatorInd, ind - separatorInd));
            }
            //Print options if printOptions = true;
            if (printOptions)
            {
                for (int i = 0; i < OptionsList.Count(); i++)
                {
                    Console.Write($"{i + 1}.");
                    Console.WriteLine(OptionsList[i]);
                }

            }
            return OptionsList;
        }

        /// <summary>
        /// Returns the lenght of list (list type must be defined by categoryIndex)
        /// </summary>
        internal static int GetListLenghtByCategory(int categoryIndex)
        {
            switch (categoryIndex)
            {
                case 0:
                    return SupplementsList.Count;
                case 1:
                //return DrinksList.Count;
                case 2:
                    return EquipmentsList.Count;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Requests a table from the database and returns it as list of defined type depending on category
        /// </summary>
        internal static dynamic GetCategorizedList(int optionIndex, Controller controller)
        {
            dynamic productsList_Obj = controller.GetAllBasedOnCategory(optionIndex);
            return ReturnDefinedTypeList(optionIndex, productsList_Obj);
        }
        private static object ReturnDefinedTypeList(int optionIndex, dynamic productList_Undefined)
        {
            switch (optionIndex)
            {
                case 0:
                    return new List<Supplements>(productList_Undefined);
                case 1:
                //return new List<Drinks>(productList_Undefined);
                case 2:
                    return new List<Equipment>(productList_Undefined);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Prints all products fetched from the database in a tablelike fashion
        /// </summary>
        internal static void PrintProductsFormated(int categoryIndex)
        {
            Console.SetCursorPosition(0, ObjectSelections.TopOffset);
            switch (categoryIndex)
            {
                case 0://supplements
                    for (int i = 0; i < SupplementsList.Count; i++)
                    {
                        ObjectSelections.PrintProductByIndex(i, categoryIndex);
                        Console.WriteLine();
                    }
                    break;
                case 1://drinks
                    /*for (int i = 0; i < DrinksList.Count; i++)
                    {
                        PrintProductByIndex(i, categoryIndex);
                        Console.WriteLine();
                    }*/
                    break;
                case 2://equipments
                    for (int i = 0; i < EquipmentsList.Count; i++)
                    {
                        ObjectSelections.PrintProductByIndex(i, categoryIndex);
                        Console.WriteLine();
                    }
                    break;
                default:
                    break;
            }
        }

        internal static int IndexOfProductInCart(object product, int categoryIndex)
        {
            switch (categoryIndex)
            {
                case 0:
                    Supplements supplement = (Supplements)product;
                    for (int i = 0; i < ShoppingCartList.Count; i++)
                    {
                        try
                        {
                            if (((Supplements)ShoppingCartList[i]).Id == supplement.Id)
                            {
                                return i;
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    break;
                /*case 1:
                    Drink supplement = (Drink)product;
                    break;*/
                case 2:
                    Equipment equipment = (Equipment)product;
                    for (int i = 0; i < ShoppingCartList.Count; i++)
                    {
                        try
                        {
                            if (((Equipment)ShoppingCartList[i]).Id == equipment.Id)
                            {
                                return i;
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    break;
            }
            return int.MaxValue;
        }

        internal static void ShowOutOfStockMsg(int productIndex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorTop = ObjectSelections.TopOffset + productIndex;
            Console.CursorLeft = 0;
            ShiftText(44, true);
            Console.Write("Out of stock");
            Console.Write(new string(' ', 44));
            System.Threading.Thread.Sleep(800);
            Console.ResetColor();

        }

        /// <summary>
        /// Determines wheater a list contains a product from the defined type or not
        /// </summary>
        internal static bool ObjectListContainsProduct(List<object> shoppingCartList, object product, string productType)
        {
            int index = 0;
            bool flag = false;
            foreach (object shoppingCartProduct in shoppingCartList)
            {
                //the switch statement is used only to attempt casting so to make the comparisons
                switch (productType)
                {
                    case "Supplements":
                        if (shoppingCartProduct.GetType().Name.ToString() == productType)
                        {
                            var supplement = (Supplements)shoppingCartProduct;
                            var supplementForComparison = (Supplements)product;
                            if (supplement.Id == supplementForComparison.Id)
                            {
                                indexerOfProductsCounter = index;
                                flag = true;
                            }

                        }
                        break;
                    /*case "Drink":
                    if (shoppingCartProduct.GetType().Name.ToString() == productType)
                        {      
                            var drink = (Drink)shoppingCartProduct;
                            var drinkForComparison = (Drink)product;
                            if (drink.Id == drinkForComparison.Id)
                            {
                               indexerOfProductsCounter = index;
                               flag = true;
                            }
                        }
                          break;*/
                    case "Equipment":
                        if (shoppingCartProduct.GetType().Name.ToString() == productType)
                        {
                            var equipment = (Equipment)shoppingCartProduct;
                            var equipmentForComparison = (Equipment)product;
                            if (equipment.Id == equipmentForComparison.Id)
                            {
                                indexerOfProductsCounter = index;
                                flag = true;
                            }
                        }
                        break;
                }
                index++;
            }
            return flag;
        }

        internal static void ShowCart()
        {
            Console.Clear();
            Console.WriteLine("Shopping cart:");
            if (GInterface.ShoppingCartList.Count > 0)
            {
                int index = 0;
                foreach (object productInCart in ShoppingCartList)
                {
                    Console.CursorLeft = 99;
                    Console.Write($"x{ShoppingCartProductCounter[index++]}");
                    Console.CursorLeft = 0;
                    Console.Write($" {index}.");
                    switch (productInCart.GetType().Name.ToString())
                    {
                        case "Supplements":

                            ObjectSelections.PrintProduct(productInCart, 0, true);
                            Console.WriteLine();
                            break;
                        case "Drinks":
                            //ObjectSelections.PrintProduct(productInCart, 1, true);
                            //Console.WriteLine();
                            break;
                        case "Equipment":
                            ObjectSelections.PrintProduct(productInCart, 2, true);
                            Console.WriteLine();
                            break;
                    }
                }

            }
            else
            {
                Console.WriteLine("Your shopping cart is empty :/");
            }
        }

        internal static dynamic GetCategorizedProduct(object product, int categoryIndex)
        {
            switch (categoryIndex)
            {
                case 0:
                    return (Supplements)product;
                case 1:
                //return (Drink)product;
                case 2:
                    return (Equipment)product;
            }
            return null;
        }

        internal static void RefreshCartTotal(decimal cartTotal)
        {
            var cursorPositionTop = Console.CursorTop;
            var cursorPositionLeft = Console.CursorLeft;
            Console.CursorTop = 3;
            Console.CursorLeft = 0;
            Console.Write($"\nShopping cart: {cartTotal:f2}bgn");
            Console.SetCursorPosition(cursorPositionLeft, cursorPositionTop);
            //ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
        }
        /// <summary>
        /// Returns a horizontal line
        /// </summary>
        internal static string HorizontalLine(char character, int lineLenght)
        {
            char[] charactersLine = new char[lineLenght];
            for (int i = 0; i < lineLenght; i++)
            {
                charactersLine[i] = character;
            }
            return string.Concat(charactersLine);
        }
        /// <param name="clearLine">If true then clears all chars beforehand</param>
        internal static void ShiftText(int positions, bool clearLine = false)
        {
            if (clearLine)
            {
                int initalCursorPosition = Console.CursorLeft;
                Console.CursorLeft = 0;
                Console.Write(new string('\0', initalCursorPosition + positions));
                Console.CursorLeft = initalCursorPosition;
            }
            Console.CursorLeft += positions;
        }
    }
}