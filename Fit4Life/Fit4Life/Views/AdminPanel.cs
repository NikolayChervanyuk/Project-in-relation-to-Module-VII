using Fit4Life.Controllers;
using Fit4Life.Extentions;
using Fit4Life.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit4Life.Views
{
    internal class AdminPanel
    {
        private Controller controller;
        private static string actions;
        int pickedActionIndex = -1;
        private string adminPassword = "a";

        internal bool IsAccessGained()
        {
            List<string> passwordEntered = null;
            string symbolEntered;
            ConsoleKeyInfo key;
            int index;
            //checks if entered password matches with admin's password
            do
            {
                if (passwordEntered != null)
                {
                    DisplayErrorMsg("Wrong password!");
                }
                passwordEntered = new List<string>();
                Console.WriteLine("Esc - back");
                Console.Write("Enter password:");
                key = Console.ReadKey(false);
                index = 0;
                while (key.Key != ConsoleKey.Enter)
                {
                    if (key.Key == ConsoleKey.Escape)
                    {
                        return false;
                    }
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if (index >= 1)
                        {
                            Console.Write(' ');
                            Console.CursorLeft--;
                            passwordEntered.RemoveAt(--index);
                        }
                        else Console.Write(':');
                    }
                    else
                    {
                        symbolEntered = Convert.ToString(key.KeyChar);
                        passwordEntered.Add(Convert.ToString(symbolEntered));
                        Console.CursorLeft--;
                        Console.Write('*');
                        index++;
                    }
                    key = Console.ReadKey(false);
                }
            } while (string.Join("", passwordEntered.ToArray()) != adminPassword);

            actions =
                "Restock supplement;Restock drink;Restock equipment;" +
                "Add new supplement;Add new drink;Add new Equipment;" +
                "Delete new supplement;Delete new drink;Delete new Equipment";
            return true;
        }

        internal int SelectAction()
        {
            ObjectSelections.TopOffset = Console.CursorTop;
            ObjectSelections.LeftOffset = Console.CursorLeft;
            ObjectSelections.OptionsList = GInterface.GetStringListFromString(actions, true);
            int actionsCount = ObjectSelections.OptionsList.Count;
            int actionIndex = 0;
            var key = new ConsoleKeyInfo();
            ObjectSelections.SelectCurrentOptionAt(actionIndex);
            do
            {
                key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        ObjectSelections.DeselectCurrentOptionAt(actionIndex);
                        if (actionIndex + 1 >= actionsCount)
                        {
                            actionIndex = (actionIndex % (actionsCount - 1)) - 1;
                        }
                        ObjectSelections.SelectCurrentOptionAt(++actionIndex);
                        break;
                    case ConsoleKey.UpArrow:
                        ObjectSelections.DeselectCurrentOptionAt(actionIndex);
                        if (actionIndex - 1 < 0)
                        {
                            actionIndex += actionsCount;
                        }
                        ObjectSelections.SelectCurrentOptionAt(--actionIndex);
                        break;
                    case ConsoleKey.Escape:
                        return -1;
                }
            } while (key.Key != ConsoleKey.Enter);
            return pickedActionIndex = actionIndex;
        }

        internal void TakeAction(int pickedActionIndex)
        {
            Console.Clear();
            if (pickedActionIndex < 0) return;
            //Restock action
            if (pickedActionIndex >= 0 && pickedActionIndex <= 2)
            {
                switch (pickedActionIndex)
                {
                    case 0://supplement
                        break;
                    case 1://drink
                        break;
                    case 2://equipment
                        break;
                }
            }
            //Add action
            if (pickedActionIndex >= 3 && pickedActionIndex <= 5)
            {
                switch (pickedActionIndex)
                {
                    case 3://supplement
                        Supplements supplement;
                        EnterNewSupplement(out supplement);
                        if (supplement == null)
                        {
                            Console.Clear();
                            DisplayInfoMsg("Supplement discarded");
                        }
                        else
                        {
                            controller.AddProduct(supplement, 0);
                        }
                        break;
                    case 4://drink
                        break;
                    case 5://equipment
                        break;
                }
            }
            Console.Clear();
            return;
        }

        /// <summary>
        /// Creates new supplement by entering the properties of supplement.
        /// If the method is terminated via pressing the Escape key,then the supplement equals null.
        /// </summary>
        /// <param name="supplement"></param>
        private void EnterNewSupplement(out Supplements supplement)
        {
            supplement = new Supplements();
            int maxNameLenght = 32;
            int maxBrandLenght = 18;
            int maxPriceValue = 99999;
            int maxWeightGrams = 99999;
            int doseFromWeight = 20;
            int maxQuantity = 9999;
            bool isEscapeKeyPressed = false;
            bool isValid = true;
            //Name check
            do
            {
                List<string> sName = new List<string>();
                if (!isValid)
                {
                    DisplayErrorMsg("Name too long or blank!");
                }
                Console.Write("Supplemnt name(not empty and no more than 32 symbols):");
                EnterField(out sName, out isEscapeKeyPressed);
                if (isEscapeKeyPressed)
                {
                    supplement = null;
                    return;
                }
                if (sName.Count <= 0 || sName.Count > maxNameLenght)
                {
                    isValid = false;
                }
                else
                {
                    supplement.Name = string.Join("", sName);
                    break;
                }
            } while (true);
            Console.Clear();

            //Brand check
            isValid = true;
            do
            {
                if (!isValid)
                {
                    DisplayErrorMsg("Brand name too long or blank!", 2200);
                }
                Console.Write("Supplement brand(no more than 20 symbols):");
                List<string> sBrand = new List<string>();
                EnterField(out sBrand, out isEscapeKeyPressed);
                if (isEscapeKeyPressed)
                {
                    supplement = null;
                    return;
                }
                if (sBrand.Count <= 0 || sBrand.Count > maxBrandLenght)
                {
                    isValid = false;
                }
                else
                {
                    supplement.Brand = string.Join("", sBrand);
                    break;
                }
            } while (true);
            Console.Clear();
            
            //Price check
            isValid = true;
            do
            {
                Console.Write($"Supplement price(max value is {maxPriceValue}):");
                List<string> PriceString;
                EnterField(out PriceString, out isEscapeKeyPressed);
                if(isEscapeKeyPressed)
                {
                    supplement = null;
                    return;
                }
                string priceString = "";
                foreach (string item in PriceString)
                {
                    priceString += item;
                }
                decimal price;
                bool isDecimal = decimal.TryParse(priceString, out price);
                if (!(isValid && isDecimal))
                {
                    DisplayErrorMsg("Invalid price format or !", 2500);
                }
                else
                {
                    supplement.Price = price;
                    break;
                }
            } while (true);

            //Weight check
            isValid = true;
            do
            {

            } while (true);

        }

        private void EnterField(out List<string> fieldName, out bool isEscapeKeyPressed)
        {
            int index = 0;
            string symbolEntered;
            fieldName = new List<string>();
            isEscapeKeyPressed = false;
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            do
            {
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    isEscapeKeyPressed = true;
                    return;
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (index >= 1)
                    {
                        Console.Write(' ');
                        Console.CursorLeft--;
                        fieldName.RemoveAt(--index);
                    }
                    else Console.CursorLeft++;
                }
                else if(key.Key != ConsoleKey.Enter)
                {
                    symbolEntered = Convert.ToString(key.KeyChar);
                    fieldName.Add(Convert.ToString(symbolEntered));
                    index++;
                }
            } while (key.Key != ConsoleKey.Enter);
        }

        internal void PrintAdminPageHeadder()
        {
            Console.Clear();
            Console.WriteLine("+" + GInterface.HorizontalLine('-', 17) + "+");
            Console.WriteLine("|-<Welcome admin>-|");
            Console.WriteLine("+" + GInterface.HorizontalLine('~', 17) + "+");
        }
        private void DisplayInfoMsg(string infoMessage, int timeout = 1000)
        {
            Console.Clear();
            Console.WriteLine(GInterface.HorizontalLine('x', infoMessage.Length));
            InformingConsoleColor();
            Console.Write(infoMessage);
            System.Threading.Thread.Sleep(timeout);
            Console.ResetColor();
            Console.CursorLeft = 0;
            GInterface.DeleteRow();
            Console.Clear();
        }
        private void DisplayErrorMsg(string errorMessage, int timeout = 1000)
        {
            Console.Clear();
            Console.WriteLine(GInterface.HorizontalLine('x', errorMessage.Length));
            WarningConsoleColor();
            Console.Write(errorMessage);
            System.Threading.Thread.Sleep(timeout);
            Console.ResetColor();
            Console.CursorLeft = 0;
            GInterface.DeleteRow();
            Console.Clear();
        }
        private void WarningConsoleColor()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
        }
        private void InformingConsoleColor()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public AdminPanel()
        {
            controller = new Controller();
        }

    }
}
