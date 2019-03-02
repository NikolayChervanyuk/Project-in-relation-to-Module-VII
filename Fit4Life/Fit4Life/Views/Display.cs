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
        internal void InitializeView()
        {
            int headderRows = 4;
            Console.WriteLine(GInterface.HorizontalLine('-', 23));
            Console.WriteLine($"{GInterface.ShiftText(5)}<|Fit 4 Life|>"); //14 spaces to center
            Console.WriteLine("Welcome to our shop!");
            Console.WriteLine(GInterface.HorizontalLine('-', 23));
            GInterface.GenerateOptionsList(printOptions: true);

            int currLineSelected = headderRows; //when = to headderRows, 1st option is selected
            int optIndex = 0;
            int optionsCount = GInterface.optionsList.Length;

            //the following code enables option selection with arrow keys and enter
            GInterface.SelectCurrentOptionAt(currLineSelected, optIndex);
            ConsoleKeyInfo key;
            int pickedOptionInd = -1;
            while ((key = Console.ReadKey()).Key != ConsoleKey.Enter)
            {
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        GInterface.DeselectCurrentOptionAt(currLineSelected, optIndex);
                        if (optIndex + 1 >= optionsCount)
                        {
                            currLineSelected -= optIndex + 1;
                            optIndex = (optIndex % (optionsCount - 1)) - 1;
                        }
                        GInterface.SelectCurrentOptionAt(++currLineSelected, ++optIndex);


                        break;

                    case ConsoleKey.UpArrow:
                        GInterface.DeselectCurrentOptionAt(currLineSelected, optIndex);
                        if (optIndex - 1 < 0)
                        {
                            currLineSelected += (optionsCount - 1) + 1;
                            optIndex += (optionsCount - 1) + 1;
                        }
                        GInterface.SelectCurrentOptionAt(--currLineSelected, --optIndex);
                        break;

                    case ConsoleKey.Enter:
                        pickedOptionInd = optIndex;
                        //и сега Митак, тук се взима решение какво да се прави според избрания индекс
                        break;
                }
            }

        }
    }
}
