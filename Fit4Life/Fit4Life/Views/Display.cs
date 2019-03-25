using System;
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
        Controller controller = new Controller();
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
            ObjectSelections.OptionsList = GInterface.GetMainPageOptionsList(optionsString, printOptions: true);
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
            int optionsCount = ObjectSelections.OptionsList.Count;
            Console.CursorVisible = false;
            ObjectSelections.SelectCurrentOptionAt(optIndex);
            ConsoleKeyInfo key = Console.ReadKey();
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
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Console.WriteLine("Visit us again :)");
                        //Console.ReadKey();
                        Environment.Exit(0);
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
                    default:
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
            var key = Console.ReadKey();
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
                        GInterface.ShoppingCartList.Add(
                            GInterface.GetCategorizedList(optionIndex, controller)[productIndex]);
                        Console.WriteLine();
                        break;
                    case ConsoleKey.Tab://show/hide cart
                        Console.Clear();
                        foreach (object productInCart in GInterface.ShoppingCartList)
                        {
                            switch (productInCart.GetType().Name.ToString())
                            {
                                case "Supplements":
                                    ObjectSelections.PrintProduct(productInCart, 0);
                                    break;
                                case "Drinks":
                                    //ObjectSelections.PrintProduct(productInCart, 1);
                                    break;
                                case "Equipment":
                                    ObjectSelections.PrintProduct(productInCart, 2);
                                    break;
                            }
                        }
                        if (key.Key == ConsoleKey.Tab)
                        {
                            GInterface.PrintProductsPageHeadder(shoppingCartTotal, optionIndex);
                            GInterface.PrintProductsFormated(optionIndex);
                            ObjectSelections.SelectCurrentProductAt(productIndex, optionIndex);
                        }
                        break;
                }
                key = Console.ReadKey();
            }
            Console.Clear();
            OpenHomeView();
        }

        public Display()
        {
            OpenHomeView();
            do
            {
                OpenProductsView(pickedOptionIndex);
            } while (true);
        }
    }
}