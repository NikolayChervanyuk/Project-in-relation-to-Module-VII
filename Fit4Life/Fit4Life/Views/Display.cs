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
        internal int optionIndex = -1;
        internal void InitializeHomeView()
        {
            
            double shoppingCartTotal = 0;
            Console.WriteLine($"Total :{shoppingCartTotal}");
            GInterface.PrintMainPageHeadder();
            optionIndex = SelectOption();
        }
        internal int SelectOption()
        {
            //the following code enables option selection with arrow keys and enter
            int currLineSelected = GInterface.GetHomePageHeadderRowsCount; //when = to HeadderRowsCount, 1st option is selected
            int optIndex = 0;
            int optionsCount = GInterface.optionsList.Length;
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
                            currLineSelected += optionsCount ;
                            optIndex += optionsCount ;
                        }
                        GInterface.SelectCurrentOptionAt(--currLineSelected, --optIndex);
                        break;

                    case ConsoleKey.Enter:
                        pickedOptionInd = optIndex;
                        //и сега Митак, тук се взима решение какво да се прави според избрания индекс
                        break;
                }
            }
            return pickedOptionInd;
        }
    }
}