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
        internal int optionIndex;
        internal void InitializeHomeView()
        {
            //Console.SetWindowSize(50, 30); //max size (170, 62). Depends on resolution
            //Console.SetBufferSize(50, 30);
            double shoppingCartTotal = 0;
            Console.WriteLine($"Total: {shoppingCartTotal}");
            GInterface.PrintMainPageHeadder();
            GInterface.GenerateMainPageOptionsList(printOptions: true);
            //if = -1, no option was selected
            optionIndex = SelectOption();
        }
        internal int SelectOption()
        {
            int pickedOptionIndex = -1;
            //the following code enables option selection with arrow keys and enter
            int optIndex = 0;
            int optionsCount = GInterface.optionsList.Count;
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
                }
                key = Console.ReadKey();
            }
            Console.CursorVisible = true;
            return pickedOptionIndex = optIndex;
        }
    }
}