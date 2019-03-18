using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fit4Life.Extentions;
namespace Fit4Life.Views
{
    internal class Display
    {
        internal static int pickedOptionIndex;
        internal static double shoppingCartTotal = 0;
        /// <summary>
        /// Initializes main page headder and sets optionIndex 
        /// to the option the user has selected
        /// </summary>
        internal void OpenHomeView()
        {
            Console.Clear();
            GInterface.SetWindowSize(50, 60);
            Console.WriteLine($"Total: {shoppingCartTotal:f2}bgn");
            GInterface.PrintMainPageHeadder();
            string optionsString = "Supplements;Drinks;Equipment;Fitness world news;About";
            GInterface.OptionsList = GInterface.GetMainPageOptionsList(optionsString, printOptions: true);
            pickedOptionIndex = SelectOption();
        }
        /// <summary>
        /// Returns the index option selected by the user
        /// </summary>
        /// <returns></returns>
        private int SelectOption()
        {
            int pickedOptionIndex = -1;
            //the following code enables option selection with arrow keys and enter
            int optIndex = 0;
            int optionsCount = GInterface.OptionsList.Count;
            Console.CursorVisible = false;
            GInterface.SelectCurrentOptionAt(optIndex);
            ConsoleKeyInfo key = Console.ReadKey();
            while (key.Key != ConsoleKey.Enter)
            {
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        GInterface.DeselectCurrentOptionAt(optIndex);
                        if (optIndex + 1 >= optionsCount)
                        {
                            optIndex = (optIndex % (optionsCount - 1)) - 1;
                        }
                        GInterface.SelectCurrentOptionAt(++optIndex);
                        break;

                    case ConsoleKey.UpArrow:
                        GInterface.DeselectCurrentOptionAt(optIndex);
                        if (optIndex - 1 < 0)
                        {
                            optIndex += optionsCount;
                        }
                        GInterface.SelectCurrentOptionAt(--optIndex);
                        break;
                    default:

                        break;
                }
                key = Console.ReadKey();
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

            if (optionIndex != -1)
            {
                GInterface.PrintProductsPageHeadder(shoppingCartTotal, optionIndex);
                SelectProduct();
            }
            else
            {
                throw new IndexOutOfRangeException("No option was chosen");
            }
        }

        private void SelectProduct()
        {
            var key = Console.ReadKey();
            while (!(key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.Backspace))
            {
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:

                        break;
                    case ConsoleKey.UpArrow:

                        break;
                    case ConsoleKey.Spacebar://show description

                        break;
                    case ConsoleKey.Enter://add to cart

                        break;
                    case ConsoleKey.Tab://show/hide cart

                        break;
                }
            }
            Console.Clear();
            OpenHomeView();
        }

        public Display()
        {
            OpenHomeView();
        }
    }
}