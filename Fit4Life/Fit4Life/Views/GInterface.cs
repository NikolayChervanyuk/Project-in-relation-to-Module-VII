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
        internal static List<Type> ShoppingCartList { get; set; }

        internal static List<Supplements> SupplementsList { get; set; }
        //internal static List<Drink> SupplementsList { get; set; }
        internal static List<Equipment> EquipmentsList { get; set; }

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
            Console.WriteLine("<|Fit 4 Life|>"); //14 spaces to center
            Console.WriteLine("Welcome to our shop!");
            Console.WriteLine(HorizontalLine('-', 23));
            ObjectSelections.TopOffset = Console.CursorTop;
            ObjectSelections.LeftOffset = Console.CursorLeft;
        }
        internal static void PrintProductsPageHeadder(double shoppingCartTotal, int optionIndex)
        {
            Console.WriteLine("Navigation:\nArrows up/down - scroll \nEnter - add to cart\t  Esc/Backspace - return to main menu");
            Console.WriteLine($"\nShopping cart: {shoppingCartTotal:f2}bgn");
            Console.WriteLine(HorizontalLine('-', 100));
            ShiftText(45);
            Console.WriteLine(ObjectSelections.OptionsList[optionIndex]);
            Console.WriteLine(HorizontalLine('-', 100));
            ObjectSelections.TopOffset = Console.CursorTop;
            ObjectSelections.LeftOffset = Console.CursorLeft;
        }

        /// <summary>
        /// Generates the categories to choose from.
        /// </summary>
        /// <param name="optionsString">Separate each option with semicolon(;)</param>
        /// <param name="printOptions">If set true, prints the list</param>
        internal static List<string> GetMainPageOptionsList(string optionsString, bool printOptions = false)
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

        internal static void PrintProductsFormated(int categoryIndex)
        {
            Console.SetCursorPosition(0, ObjectSelections.TopOffset);
            switch (categoryIndex)
            {
                case 0://supplements
                    for (int i = 0; i < SupplementsList.Count; i++)
                    {
                        ObjectSelections.PrintProduct(i, categoryIndex);
                    }
                    break;
                case 1://drinks
                    /*for (int i = 0; i < DrinksList.Count; i++)
                    {
                        PrintProduct(i, categoryIndex);
                    }*/
                    break;
                case 2://equipments
                    for (int i = 0; i < EquipmentsList.Count; i++)
                    {
                        ObjectSelections.PrintProduct(i, categoryIndex);
                    }
                    break;
                default:
                    break;
            }
        }
        //control print method (sholud be removed in released version)

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