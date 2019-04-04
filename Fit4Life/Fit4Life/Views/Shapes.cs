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
    internal static class Shapes
    {
        private const int supplementsIndex = Display.supplementsIndex;
        private const int drinksIndex = Display.drinksIndex;
        private const int equipmentsIndex = Display.equipmentsIndex;

        internal static  void WriteBottomRestockInfo(object product, int categoryIndex)
        {
            string explanationMsg = "Quantity to add:";
            int[] quantityFieldPos = new int[2];
            switch (categoryIndex)
            {
                case supplementsIndex:
                    Supplements supplement = (Supplements)product;
                    Console.Write($"Supplement: {supplement.Name} / {supplement.Brand}");
                    GInterface.ShiftText(5);
                    Console.Write($"Price: {supplement.Price:f2}");
                    GInterface.ShiftText(5);
                    Console.Write($"Q:{supplement.Quantity}");
                    GInterface.ShiftText(2);
                    Console.CursorLeft = 100 - 25;
                    Console.Write(explanationMsg);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    quantityFieldPos = new int[] { Console.CursorLeft, Console.CursorTop }; //Beginning of the quanitity number field
                    Console.Write(GInterface.HorizontalLine(' ', ' ', 8));
                    Console.CursorLeft = quantityFieldPos[0];
                    break;
                case drinksIndex:
                   Drink drink = (Drink)product;
                    Console.Write($"Equipment: {drink.Name}");
                    GInterface.ShiftText(5);
                    Console.Write($"Price: {drink.Price:f2}");
                    GInterface.ShiftText(5);
                    Console.Write($"Q:{drink.Quantity}");
                    GInterface.ShiftText(2);
                    Console.CursorLeft = 100 - 25;
                    explanationMsg = "Quantity to add:";
                    Console.Write(explanationMsg);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    quantityFieldPos = new int[] { Console.CursorLeft, Console.CursorTop }; //Beginning of the quanitity number field
                    Console.Write(GInterface.HorizontalLine(' ', ' ', 8));
                    Console.CursorLeft = quantityFieldPos[0];
                    break;
                case equipmentsIndex:
                    Equipment equipment = (Equipment)product;
                    Console.Write($"Equipment: {equipment.Name} / {equipment.Brand}");
                    GInterface.ShiftText(5);
                    Console.Write($"Price: {equipment.Price:f2}");
                    GInterface.ShiftText(5);
                    Console.Write($"Q:{equipment.Quantity}");
                    GInterface.ShiftText(2);
                    Console.CursorLeft = 100 - 25;
                    explanationMsg = "Quantity to add:";
                    Console.Write(explanationMsg);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    quantityFieldPos = new int[] { Console.CursorLeft, Console.CursorTop }; //Beginning of the quanitity number field
                    Console.Write(GInterface.HorizontalLine(' ', ' ', 8));
                    Console.CursorLeft = quantityFieldPos[0];
                    break;
            }
        }

        internal static void ShowFinalRestockInfo(object product, int categoryIndex, int quantityToAdd)
        {
            int[] boxPos = { 67, Console.CursorTop + 2 };
            Console.SetCursorPosition(boxPos[0], boxPos[1]);
            Console.ResetColor();
            DrawBox(boxPos[0], boxPos[1], 34, 4, '+', '#'); //width=24
            Console.CursorTop++;
            switch (categoryIndex)
            {
                case supplementsIndex:
                    var supplement = (Supplements)product;
                    //int[] quantityFieldPos = { Console.CursorLeft, Console.CursorTop }; //Beginning of the quanitity number field
                    Console.Write($"Expenses:{supplement.Price:f2}bgn x {quantityToAdd}");
                    Console.CursorTop++;
                    Console.CursorLeft = boxPos[0] + 1;
                    Console.Write($" => {supplement.Price * quantityToAdd}bgn");
                    break;
                case drinksIndex:
                   Drink drink = (Drink)product;
                    Console.Write($"Expenses:{drink.Price:f2}bgn x {quantityToAdd}");
                    Console.CursorTop++;
                    Console.CursorLeft = boxPos[0] + 1;
                    Console.Write($" => {drink.Price * quantityToAdd}bgn");
                    break;
                case equipmentsIndex:
                    Equipment equipment = (Equipment)product;
                    Console.Write($"Expenses:{equipment.Price:f2}bgn x {quantityToAdd}");
                    Console.CursorTop++;
                    Console.CursorLeft = boxPos[0] + 1;
                    Console.Write($" => {equipment.Price * quantityToAdd}bgn");
                    break;
            }
            DrawBox(boxPos[0], boxPos[1] + 4, 34, 3, '+', '#');
            Console.CursorTop++;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Confirm the restock action?(Y/N)");
            Console.ResetColor();
        }
        
        /// <summary>
        /// Draws a frame. If resetCursor is true, then cursor is set to it's initial position - else it stays to it's last position.
        /// Available values for frameColour are: 'red', 'green', 'blue', 'white' and vanila.
        /// </summary>
        internal static void DrawFrame(int width, int height, bool resetCursos = false, char edges = ' ', char character = ' ', string frameColour = "vanila")
        {
            frameColour = frameColour.ToLower();
            Console.ResetColor();
            if (frameColour == "blue") InformingConsoleColor();
            if (frameColour == "red") WarningConsoleColor();
            if (frameColour == "green")
            { Console.BackgroundColor = ConsoleColor.Green; Console.ForegroundColor = ConsoleColor.White; }
            if (frameColour == "white")
            { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            int[] cursorPosition = { Console.CursorLeft, Console.CursorTop + 1 };
            Console.Write(GInterface.HorizontalLine(character, edges, width));
            Console.CursorLeft--;
            GInterface.DrawVerticalLine(character, edges, height);
            Console.CursorLeft = 0;
            Console.WriteLine(GInterface.HorizontalLine(character, edges, width));
            if (resetCursos) Console.SetCursorPosition(cursorPosition[0], cursorPosition[1]);
            Console.ResetColor();
        }

        internal static void DrawRestockProductFrame(int leftOffset, int topOffset)
        {
            Console.Write(GInterface.HorizontalLine('-', '#', 101));
            Console.CursorLeft--;
            Console.CursorTop++;
            Console.WriteLine('<');
            Console.Write(GInterface.HorizontalLine('-', '#', 101));
            Console.CursorTop--;
            Console.CursorLeft = leftOffset;
            Console.Write('>');
            Console.SetCursorPosition(leftOffset, topOffset);
        }

        internal static void DrawBox(int xPos, int yPos, int width, int height, char character = ' ', char edegs = ' ')
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(GInterface.HorizontalLine(character, edegs, width));
            Console.CursorLeft--;
            GInterface.DrawVerticalLine(character, edegs, height);
            Console.CursorLeft -= width;
            Console.Write(GInterface.HorizontalLine(character, edegs, width));
            Console.CursorLeft -= width;
            Console.CursorTop -= height - 1;
            GInterface.DrawVerticalLine(character, edegs, height);
            Console.CursorTop -= height - 1;
        }
        internal static void WarningConsoleColor()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
        }
        internal static void InformingConsoleColor()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
        }
         
    }
}
