using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit4Life.Extentions
{
    internal static class GInterface
    {
        internal static int GetHomePageHeadderRowsCount { get; private set; }
        internal static string DrawHorizontalLine(char character, int lineLenght)
        {
            char[] charactersLine = new char[lineLenght];
            for (int i = 0; i < lineLenght; i++)
            {
                charactersLine[i] = character;
            }
            return string.Concat(charactersLine);
        }

        internal static void PrintMainPageHeadder()
        {
            Console.WriteLine(DrawHorizontalLine('-', 23));
            Console.WriteLine($"{ShiftText(5)}<|Fit 4 Life|>"); //14 spaces to center
            Console.WriteLine("Welcome to our shop!");
            Console.WriteLine(DrawHorizontalLine('-', 23));
            GetHomePageHeadderRowsCount = 5;
            GenerateOptionsList(printOptions: true);
        }

        internal static string ShiftText(int positions)
        {
            char[] shiftingChars = new char[positions];
            for (int i = 0; i < positions; i++)
            {
                shiftingChars[i] = ' ';
            }
            return string.Concat(shiftingChars);
        }
        internal static string[] optionsList;
        /// <summary>
        /// Generates the options you can select from on the main page
        /// </summary>
        /// <param name="cursorOffset"></param>
        private static void GenerateOptionsList(int optionsCount = 5, bool printOptions = false)
        {
            if (optionsList == null)
            {
                optionsList = new string[optionsCount];
                for (int i = 0; i < optionsList.Count(); i++)
                {
                    optionsList[i] = $"{i + 1}.";
                }
                //ето тук ръчно въвеждаме какво да се показва на менюто.
                //Общо 5 избора съм направил (виж сигнатурата)
                optionsList[0] += "Supplements";
                optionsList[1] += "Drinks";
                optionsList[2] += "Equipment";
                optionsList[3] += "Fitness world news";
                optionsList[4] += "About";
            }
            //print options if printOptions = true;
            if (printOptions)
            {
                foreach (string option in optionsList)
                {
                    Console.WriteLine(option);
                }
            }
        }

        internal static void DeselectCurrentOptionAt(int currentLineSelected, int optionIndex)
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, currentLineSelected);
            Console.Write("\r" + optionsList[optionIndex] + new string(' ', 50));
        }

        internal static void SelectCurrentOptionAt(int currentLineSelected, int optionIndex)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, currentLineSelected);
            Console.Write("\r" + optionsList[optionIndex] + "<");
            Console.ResetColor();
        }
    }
}
