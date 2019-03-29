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

            actions = "Restock supplement;Restock drink;Restock equipment;" +
            "Add new supplement;Add new Drink;Add new Equipment";
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
                            Console.ResetColor();

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
            PrintAdminPageHeadder();
            SelectAction();
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
            bool isValid = true;
            bool isEscapeKeyPressed = false;
            //Name check
            do
            {
                List<string> sName = new List<string>();
                if (!isValid)
                {
                    DisplayErrorMsg("Symbol limit overreached!");
                }
                Console.Write("Supplemnt name(no more than 32 symbols):");
                EnterField(out sName, out isEscapeKeyPressed);
                if (sName.Count > maxNameLenght)
                {
                    isValid = false;
                }
                else
                {
                    supplement.Name = string.Join("", sName);
                    break;
                }
            } while (true);
            if (isEscapeKeyPressed)
            {
                supplement = null;
                return;
            }

            //Brand check
            isValid = true;
            do
            {
                if (!isValid)
                {
                    DisplayErrorMsg("Symbol limit overreached!");
                }
                Console.Write("Supplement brand(no more than 20 symbols):");
                string sBrand = Console.ReadLine();
                if (sBrand.Length > maxBrandLenght)
                {
                    isValid = false;
                }
                else
                {
                    supplement.Brand = sBrand;
                    break;
                }
            } while (true);

            //Price check
            do
            {
                isValid = true;
                Console.Write($"Supplement price(max value is {maxPriceValue}):");
                decimal number;
                isValid = decimal.TryParse(Console.ReadLine(), out number);
                if (!isValid)
                {
                    DisplayErrorMsg("Invalid price format!");
                }
                else
                {
                    supplement.Price = number;
                    break;
                }
            } while (true);

            //Weight check
            isValid = true;
            do
            {

            } while (true);

            supplement = null;
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
                else
                {
                    symbolEntered = Convert.ToString(key.KeyChar);
                    fieldName.Add(Convert.ToString(symbolEntered));
                    Console.CursorLeft--;
                    Console.Write(key.KeyChar);
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
        private void DisplayInfoMsg(string infoMessage)
        {
            Console.Clear();
            Console.WriteLine(GInterface.HorizontalLine('x', infoMessage.Length));
            InformingConsoleColor();
            Console.Write(infoMessage);
            System.Threading.Thread.Sleep(1000);
            Console.ResetColor();
            Console.CursorLeft = 0;
            GInterface.DeleteRow();
            Console.Clear();
        }
        private void DisplayErrorMsg(string errorMessage)
        {
            Console.Clear();
            Console.WriteLine(GInterface.HorizontalLine('x', errorMessage.Length));
            WarningConsoleColor();
            Console.Write(errorMessage);
            System.Threading.Thread.Sleep(900);
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
