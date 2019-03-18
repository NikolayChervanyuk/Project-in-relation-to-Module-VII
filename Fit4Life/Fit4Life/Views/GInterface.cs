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
        private static int TopOffset;
        private static int LeftOffset;
        internal static List<string> OptionsList { get; set; }
        internal static List<Type> ShoppingCartList { get; set; }

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
                                  (int)(windowHeight * heightCoeff));
        }

        internal static void PrintMainPageHeadder()
        {
            Console.WriteLine(HorizontalLine('-', 23));
            ShiftText(5);
            Console.WriteLine("<|Fit 4 Life|>"); //14 spaces to center
            Console.WriteLine("Welcome to our shop!");
            Console.WriteLine(HorizontalLine('-', 23));
            TopOffset = Console.CursorTop;
            LeftOffset = Console.CursorLeft;
        }
        internal static void PrintProductsPageHeadder(double shoppingCartTotal, int optionIndex)
        {
            Console.WriteLine("Navigation:\nArrows up/down - scroll   Space - show description \nEnter - add to cart\t  Esc/Backspace - return to main menu");
            Console.WriteLine($"\nShopping cart: {shoppingCartTotal:f2}bgn");
            Console.WriteLine(HorizontalLine('-', 100));
            GInterface.ShiftText(45);
            Console.WriteLine(OptionsList[optionIndex]);
            Console.WriteLine(HorizontalLine('-', 100));
        }

        internal static void SelectCurrentOptionAt(int optionIndex)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, TopOffset + optionIndex);
            Console.Write($"{optionIndex + 1}." + OptionsList.ElementAt(optionIndex) + "<");
            Console.ResetColor();
        }
        internal static void DeselectCurrentOptionAt(int optionIndex)
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, TopOffset + optionIndex);
            Console.Write($"{optionIndex + 1}." + OptionsList.ElementAt(optionIndex) + new string(' ', Console.BufferWidth / 2));
        }

        /// <summary>
        /// Generates the categories to choose from.
        /// </summary>
        /// <param name="optionsString">Separate each option with semicolon(;)</param>
        /// <param name="printOptions">If set true, prints the list</param>
        internal static List<string> GetMainPageOptionsList(string optionsString, bool printOptions = false)
        {
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