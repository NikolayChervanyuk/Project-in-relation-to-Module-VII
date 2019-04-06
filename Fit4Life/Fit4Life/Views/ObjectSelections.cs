using Fit4Life.Data;
using Fit4Life.Data.Models;
using Fit4Life.Extentions;
using Fit4Life.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit4Life.Views
{
    /// <summary>
    /// Containing methods for selecting options and products and printing them.
    /// </summary>
    internal static class ObjectSelections
    {
        internal static List<string> OptionsList { get; set; }
        internal static int TopOffset;
        internal static int LeftOffset;
        private const int supplementsIndex = Display.supplementsIndex;
        private const int drinksIndex = Display.drinksIndex;
        private const int equipmentsIndex = Display.equipmentsIndex;

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

        internal static void SelectCurrentProductAt(int productIndex, int categoryIndex)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, TopOffset + productIndex);
            Console.Write(new string(' ', 100));
            Console.CursorLeft = 0;
            PrintProductByIndex(productIndex, categoryIndex);

            //Console.CursorTop--;
            Console.ResetColor();
        }
        internal static void DeselectCurrentProductAt(int productIndex, int categoryIndex)
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, TopOffset + productIndex);
            Console.Write(new string(' ', 100));
            Console.CursorLeft = 0;
            PrintProductByIndex(productIndex, categoryIndex);
        }

        /// <summary>
        /// Prints a product from already fetched lists (product and category indexes required)
        /// </summary>
        internal static void PrintProductByIndex(int productIndex, int categoryIndex, bool printForCart = false)
        {
            Console.Write($" {productIndex + 1}.");
            switch (categoryIndex)
            {
                case supplementsIndex:
                    PrintSupplement(GInterface.SupplementsList[productIndex]);
                    break;
                case drinksIndex:
                    PrintDrink(GInterface.DrinksList[productIndex]);
                    break;
                case equipmentsIndex:
                    PrintEquipment(GInterface.EquipmentsList[productIndex]);
                    break;
            }
        }

        /// <summary>
        /// Prints a product whose type is defined by category index (independent from the fetched Lists)
        /// </summary>
        internal static void PrintProduct(object product, int categoryIndex, bool printForCart = false)
        {
            switch (categoryIndex)
            {
                case supplementsIndex:
                    Supplements supplement = (Supplements)product;
                    PrintSupplement(supplement, printForCart);
                    break;
                case drinksIndex:
                    Drink drink = (Drink)product;
                    PrintDrink(drink, printForCart);
                    break;
                case equipmentsIndex:
                    Equipment equipment = (Equipment)product;
                    PrintEquipment(equipment, printForCart);
                    break;
            }
        }

        internal static void PrintSupplement(Supplements supplement, bool printForCart = false)
        {
            int offset = 0;
            Console.Write($" {supplement.Name}");
            Console.CursorLeft = offset += 34;
            Console.Write($"{supplement.Brand}");
            Console.CursorLeft = offset += 19;
            Console.Write($"{supplement.Weight}");
            Console.CursorLeft = offset += 26;
            Console.Write($"{supplement.Price:#.00}bgn");
            Console.CursorLeft = offset += 12;
            if (!printForCart)
            {
                Console.Write($"Q:{supplement.Quantity}");
            }
        }
        internal static void PrintDrink(Drink drink, bool printForCart = false)
        {
            int offset = 0;
            Console.Write($" {drink.Name}");
            Console.CursorLeft = offset += 34;
            Console.Write($"{drink.Mililiters}");
            Console.CursorLeft = offset += 19;
            Console.Write($"{drink.Price:#.00}bgn");
            Console.CursorLeft = 90;
            if (!printForCart)
            {
                Console.Write($"Q:{drink.Quantity}");
            }
        }
        internal static void PrintEquipment(Equipment equipment, bool printForCart = false)
        {
            int offset = 0;
            Console.Write($" {equipment.Name}");
            Console.CursorLeft = offset += 38;
            Console.Write($"{equipment.Brand}");
            Console.CursorLeft = offset += 27;
            Console.Write($"{equipment.Price:#.00}bgn");
            Console.CursorLeft = offset += 20;
            if (!printForCart)
            {
                Console.Write($"Q:{equipment.Quantity}");
            }

        }

        internal static void PrintProductInCart(Cart cart)
        {
            int offset = 0;
            Console.Write($" {cart.Name}");
            Console.CursorLeft = offset += 35;
           
            Console.Write($"{cart.Price:#.00}bgn");
            Console.CursorLeft = offset += 20;

            Console.WriteLine($"Q:{cart.Quantity}");
        }
    }
}