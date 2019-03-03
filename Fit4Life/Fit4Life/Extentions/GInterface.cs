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

        internal static List<string> optionsList;

        /// <summary>
        /// Generates the options you can select from on the main page. If printOptions is true, then we print list of options
        /// </summary>
        /// <param name="optionsCount"></param>
        /// <param name="printOptions"></param>
        internal static void GenerateMainPageOptionsList(bool printOptions = false)
        {
            if (optionsList == null)
            {
                optionsList = new List<string>();
                //Here we add the options which would be displayed in our main page.
                //! Separate each object with (;)
                string options = "Supplements;Drinks;Equipment;Fitness world news;About";
                int separatorInd = 0;
                int ind = -1;
                while (++ind < options.Length)
                {
                    if (options[ind] == ';')
                    {
                        optionsList.Add(options.Substring(separatorInd, ind - separatorInd));
                        separatorInd = ind+1;
                    }
                }
                optionsList.Add(options.Substring(separatorInd, ind - separatorInd));
            }
            //Print options if printOptions = true;
            if (printOptions)
            {
                for (int i = 0; i < optionsList.Count(); i++)
                {
                    Console.WriteLine($"{i + 1}.");
                }
                for (int i = 0; i < optionsList.Count(); i++)
                {
                    Console.SetCursorPosition(2, GetHomePageHeadderRowsCount + i);
                    Console.Write(optionsList[i]);
                }
            }
        }

        internal static void DeselectCurrentOptionAt(int optionIndex)
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, GetHomePageHeadderRowsCount + optionIndex);
            Console.Write($"{optionIndex + 1}." + optionsList.ElementAt(optionIndex) + new string(' ', Console.BufferWidth/2));
        }

        internal static void SelectCurrentOptionAt(int optionIndex)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, GetHomePageHeadderRowsCount + optionIndex);
            Console.Write($"{optionIndex + 1}." + optionsList.ElementAt(optionIndex) + "<");
            Console.ResetColor();
        }
    }
}