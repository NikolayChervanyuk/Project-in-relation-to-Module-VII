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
    /// <summary>
    /// Helper class for Display.
    /// </summary>
    internal static class GInterface
    {
        //internal static List<Type> ProductsList { get; set; }
        internal static List<object> ShoppingCartList { get; set; }
        internal static List<int> ShoppingCartProductCounter { get; set; }
        internal static List<Supplement> SupplementList { get; set; }
        internal static List<Drink> DrinksList { get; set; }
        internal static List<Equipment> EquipmentsList { get; set; }
        internal static List<Cart> CartList { get; set; }
        internal static int indexerOfProductsCounter = 0;
        private const int SupplementIndex = Display.SupplementIndex;
        private const int drinksIndex = Display.drinksIndex;
        private const int equipmentsIndex = Display.equipmentsIndex;

        //Resolution ratios respectively width to height, height to width
        private static readonly int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        private static readonly int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        private static double widthCoeff = (double)screenWidth / screenHeight;
        private static double heightCoeff = (double)screenHeight / screenWidth;
        internal static int[] mainPageWindowSize = { 50, 60 };
        internal static int[] productPageWindowSize = { 60, 80 };
        /// <summary>
        /// Sets the console's window size. Adaptable to different resolutions.
        /// </summary>
        internal static void SetWindowSize(int windowWidth, int windowHeight)
        {
            Console.SetWindowSize((int)(windowWidth * widthCoeff),
                                  (int)(windowHeight * heightCoeff));

            Console.SetBufferSize((int)(windowWidth * widthCoeff),
                                  (int)(windowHeight * heightCoeff * 10));
        }

        internal static void PrintMainPageHeadder()
        {
            DrawVerticalLine('|', '+', 4, true);
            Console.Write(HorizontalLine('-', '+', 24));
            Console.CursorLeft--;
            DrawVerticalLine('|', '+', 4, true);
            Console.WriteLine();
            ShiftText(5);
            Console.WriteLine("<|Fit 4 Life|>");
            ShiftText(1);
            Console.WriteLine("Welcome to our shop!");
            Console.WriteLine(HorizontalLine('-', '+', 24));
            ObjectSelections.TopOffset = Console.CursorTop;
            ObjectSelections.LeftOffset = Console.CursorLeft;
        }
        internal static void PrintProductsPageHeadder(decimal shoppingCartTotal, int optionIndex)
        {
            Console.WriteLine("Navigation:\nArrows up/down - scroll \nEnter - add to cart\t  Esc - return to main menu");
            Console.WriteLine($"\nShopping cart: {shoppingCartTotal:f2}bgn");
            Console.WriteLine(HorizontalLine('-', '+', 100));
            ShiftText(45);
            Console.WriteLine(ObjectSelections.OptionsList[optionIndex]);
            Console.WriteLine(HorizontalLine('-', '+', 100));
            ObjectSelections.TopOffset = Console.CursorTop;
            ObjectSelections.LeftOffset = Console.CursorLeft;
        }

        /// <summary>
        /// Generates list of type string from provided string. Each word, separated by semicolon(;) gets added to the list
        /// </summary>
        /// <param name="optionsString">Separate each option with semicolon(;)</param>
        /// <param name="printOptions">If set true, prints the list</param>
        internal static List<string> GetStringListFromString(string optionsString, bool printOptions = false)
        {
            var OptionsList = new List<string>();
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
            if (printOptions) PrintOptions(OptionsList);
            return OptionsList;
        }

        /// <summary>
        /// Gets the options from optionsList and prints them.
        /// </summary>
        internal static void PrintOptions(List<string> optionsList)
        {
            for (int i = 0; i < optionsList.Count(); i++)
            {
                Console.Write($"{i + 1}.");
                Console.WriteLine(optionsList[i]);
            }

        }

        /// <summary>
        /// Returns the lenght of list (list type must be defined by categoryIndex)
        /// </summary>
        internal static int GetListLenghtByCategory(int categoryIndex)
        {
            switch (categoryIndex)
            {
                case SupplementIndex:
                    return SupplementList.Count;
                case drinksIndex:
                    return DrinksList.Count;
                case equipmentsIndex:
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
        /// <summary>
        /// Returns  a list, fetched from the database from type, defined by optionIndex.
        /// </summary>
        /// <param name="optionIndex">Category of the list</param>
        /// <param name="productList_Undefined">Object, without defined type</param>
        private static object ReturnDefinedTypeList(int optionIndex, dynamic productList_Undefined)
        {
            switch (optionIndex)
            {
                case SupplementIndex:
                    return new List<Supplement>(productList_Undefined);
                case drinksIndex:
                    return new List<Drink>(productList_Undefined);
                case equipmentsIndex:
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
                case SupplementIndex://Supplement
                    for (int i = 0; i < SupplementList.Count; i++)
                    {
                        ObjectSelections.PrintProductByIndex(i, categoryIndex);
                        Console.WriteLine();
                    }
                    break;
                case drinksIndex://drinks
                    for (int i = 0; i < DrinksList.Count; i++)
                    {
                        ObjectSelections.PrintProductByIndex(i, categoryIndex);
                        Console.WriteLine();
                    }
                    break;
                case equipmentsIndex://equipments
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
        
        /// <summary>
        /// Gets the index of the element in ShoppingCartList where product's id equals element's id.
        /// </summary>
        /// <param name="product">Product to be searched for.</param>
        internal static int IndexOfProductInCart(object product, int categoryIndex)
        {
            switch (categoryIndex)
            {
                case SupplementIndex:
                    Supplement supplement = (Supplement)product;
                    for (int i = 0; i < ShoppingCartList.Count; i++)
                    {
                        try
                        {
                            if (((Supplement)ShoppingCartList[i]).Id == supplement.Id) return i;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    break;
                case drinksIndex:
                    Drink drink = (Drink)product;
                    for (int i = 0; i < ShoppingCartList.Count; i++)
                    {
                        try
                        {
                            if (((Drink)ShoppingCartList[i]).Id == drink.Id) return i;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    break;
                case equipmentsIndex:
                    Equipment equipment = (Equipment)product;
                    for (int i = 0; i < ShoppingCartList.Count; i++)
                    {
                        try
                        {
                            if (((Equipment)ShoppingCartList[i]).Id == equipment.Id) return i;
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

        /// <summary>
        /// Prints out of stock message in red text.
        /// </summary>
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
        /// Determines wheater a list contains a product from the defined type or not.
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
                    case "Supplement":
                        if (shoppingCartProduct.GetType().Name.ToString() == productType)
                        {
                            var supplement = (Supplement)shoppingCartProduct;
                            var supplementForComparison = (Supplement)product;
                            if (supplement.Id == supplementForComparison.Id)
                            {
                                indexerOfProductsCounter = index;
                                flag = true;
                            }

                        }
                        break;
                    case "Drink":
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
                          break;
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

        /// <summary>
        /// Prints the cart in raw form
        /// </summary>
        internal static void ShowCart()
        {
            Console.Clear();
            Console.WriteLine("Shopping cart:");
            if (GInterface.ShoppingCartList.Count > 0)
            {
                int index = 0;
                foreach (object productInCart in ShoppingCartList)
                {
                    Console.CursorLeft = 92;
                    Console.Write($"x{ShoppingCartProductCounter[index++]}");
                    Console.CursorLeft = 0;
                    Console.Write($" {index}.");
                    switch (productInCart.GetType().Name.ToString())
                    {
                        case "Supplement":

                            ObjectSelections.PrintProduct(productInCart, SupplementIndex, true);
                            break;
                        case "Drink":
                            ObjectSelections.PrintProduct(productInCart, drinksIndex, true);
                            break;
                        case "Equipment":
                            ObjectSelections.PrintProduct(productInCart, equipmentsIndex, true);
                            break;
                    }
                    Console.WriteLine();
                }

            }
            else
            {
                Console.WriteLine("Your shopping cart is empty :/");
            }
        }

        internal static void ShowCartInTableForm(List<Cart> cartList, decimal shoppingCartTotal)
        {
            Console.Clear();
            Console.WriteLine($"\nShopping cart: {shoppingCartTotal:f2}bgn");
            Console.WriteLine(HorizontalLine('-', '+', 100));
            Console.CursorLeft += Console.WindowLeft / 2;
            Console.WriteLine("All products in your cart");
            Console.WriteLine(HorizontalLine('-', '+', 100));
            foreach (var productInCart in cartList)
            {
                ObjectSelections.PrintProductInCart(productInCart);
            }
            var key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Spacebar && key.Key != ConsoleKey.Escape) key = Console.ReadKey(true);
        }

        internal static dynamic GetCategorizedProduct(object product, int categoryIndex)
        {
            switch (categoryIndex)
            {
                case SupplementIndex:
                    return (Supplement)product;
                case drinksIndex:
                    return (Drink)product;
                case equipmentsIndex:
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
        /// Deletes a row without changing cursor's position.
        /// </summary>
        internal static void DeleteRow()
        {
            int initialCursorPos_X = Console.CursorLeft;
            int initialCursorPos_Y = Console.CursorTop;
            Console.CursorLeft = 0;
            while (true)
            {
                Console.Write(' ');
                if (Console.CursorTop != initialCursorPos_Y)
                {
                    Console.SetCursorPosition(initialCursorPos_X, initialCursorPos_Y);
                    break;
                }
            }
        }
        /// <summary>
        /// Returns a horizontal line.
        /// </summary>
        internal static string HorizontalLine(char character, char endingsChar, int lineLenght)
        {
            char[] charactersLine = new char[lineLenght];
            charactersLine[0] = endingsChar;
            for (int i = 1; i < lineLenght - 1; i++)
            {
                charactersLine[i] = character;
            }
            charactersLine[lineLenght - 1] = endingsChar;
            return string.Concat(charactersLine);
        }
        internal static void DrawVerticalLine(char character, char endingsChar, int lineLenght, bool returnCursor = false)
        {
            int cursorPos_X = Console.CursorLeft;
            int cursorPos_Y = Console.CursorTop;
            Console.Write(endingsChar);
            Console.CursorLeft--;
            for (int i = 1; i < lineLenght; i++)
            {
                Console.SetCursorPosition(cursorPos_X, cursorPos_Y + i);
                Console.Write(character);
            }
            Console.CursorLeft--;
            Console.Write(endingsChar);
            if (returnCursor) Console.SetCursorPosition(cursorPos_X, cursorPos_Y);
        }
        /// <param name="clearLine">If true, then clears all chars beforehand.</param>
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