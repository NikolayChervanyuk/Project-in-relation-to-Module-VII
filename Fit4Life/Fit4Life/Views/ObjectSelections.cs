using Fit4Life.Data;
using Fit4Life.Extentions;
using Fit4Life.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit4Life.Views
{
    internal static class ObjectSelections
    {
        internal static List<string> OptionsList { get; set; }
        internal static int TopOffset;
        internal static int LeftOffset;

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
            PrintProduct(productIndex, categoryIndex);
            Console.ResetColor();
        }
        internal static void DeselectCurrentProductAt(int productIndex, int categoryIndex)
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, TopOffset + productIndex);
            Console.Write(new string(' ', 100));
            Console.CursorLeft = 0;
            PrintProduct(productIndex, categoryIndex);
        }

        internal static void PrintProduct(int productIndex, int categoryIndex)
        {
            int offset = 0;
            switch (categoryIndex)
            {
                case 0:
                    Console.Write($" {productIndex + 1}. {GInterface.SupplementsList[productIndex].Name}");
                    Console.CursorLeft = offset += 35;
                    Console.Write($"{GInterface.SupplementsList[productIndex].Brand}");
                    Console.CursorLeft = offset += 20;
                    Console.Write($"{GInterface.SupplementsList[productIndex].Weight}");
                    Console.CursorLeft = offset += 28;
                    Console.Write($"{GInterface.SupplementsList[productIndex].Price}");
                    Console.CursorLeft = offset += 10;
                    Console.WriteLine($"{GInterface.SupplementsList[productIndex].Quantity}");
                    break;
                case 1:

                    break;
                case 2:
                    Console.Write($" {productIndex + 1}. {GInterface.EquipmentsList[productIndex].Name}");
                    Console.CursorLeft = offset += 35;
                    Console.Write($"{GInterface.EquipmentsList[productIndex].Brand}");
                    Console.CursorLeft = offset += 20;
                    Console.WriteLine($"{GInterface.EquipmentsList[productIndex].Price}");
                    break;
                default:
                    break;
            }
        }
    }
}
