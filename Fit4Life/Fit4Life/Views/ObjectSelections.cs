﻿using Fit4Life.Data;
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
            PrintProductByIndex(productIndex, categoryIndex);
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

        internal static void PrintProductByIndex(int productIndex, int categoryIndex, bool printForCart = false)
        {
            Console.Write($" {productIndex + 1}.");
            switch (categoryIndex)
            {
                case 0:
                    PrintSupplement(GInterface.SupplementsList[productIndex]);
                    break;
                case 1:

                    break;
                case 2:
                    PrintEquipment(GInterface.EquipmentsList[productIndex]);
                    break;
            }
        }
        internal static void PrintProduct(object product, int categoryIndex, bool printForCart = false)
        {
            switch (categoryIndex)
            {
                case 0:
                    Supplements supplement = (Supplements)product;
                    PrintSupplement(supplement, printForCart);
                    break;
                case 1:
                    //Drink drink = (Drink)product;
                    //PrintDrink(drink, printForCart);
                    break;
                case 2:
                    Equipment equipment = (Equipment)product;
                    PrintEquipment(equipment, printForCart);
                    break;
            }
        }
        internal static void PrintSupplement(Supplements supplement, bool printForCart = false)
        {
            int offset = 0;
            Console.Write($" {supplement.Name}");
            Console.CursorLeft = offset += 35;
            Console.Write($"{supplement.Brand}");
            Console.CursorLeft = offset += 20;
            Console.Write($"{supplement.Weight}");
            Console.CursorLeft = offset += 26;
            Console.Write($"{supplement.Price:#.00}bgn");
            Console.CursorLeft = offset += 12;
            if (!printForCart)
            {
                Console.Write($"Q:{supplement.Quantity}");
            }
            Console.WriteLine();
        }
        internal static void PrintEquipment(Equipment equipment, bool printForCart = false)
        {
            int offset = 0;
            Console.Write($" {equipment.Name}");
            Console.CursorLeft = offset += 35;
            Console.Write($"{equipment.Brand}");
            Console.CursorLeft = offset += 23;
            Console.Write($"{equipment.Price:#.00}bgn");
            Console.CursorLeft = offset += 20;
            if (!printForCart)
            {
                Console.Write($"Q:{equipment.Quantity}");
            }
            Console.WriteLine();
        }
        /*internal static void PrintDrink(Drink drink)
        {
         
        }*/
    }
}