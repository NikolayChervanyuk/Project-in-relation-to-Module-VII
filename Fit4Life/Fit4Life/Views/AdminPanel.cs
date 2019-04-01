﻿using Fit4Life.Controllers;
using Fit4Life.Extentions;
using Fit4Life.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Fit4Life.Views
{
    internal class AdminPanel
    {
        private Controller controller;
        private static string actions;
        int pickedActionIndex = -1;
        private string adminPassword = "a";

        /// <summary>
        /// Requests for password. If the user enters correct password, then the method returns true, otherwise false.
        /// </summary>
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
                        Supplements supplement = EnterNewSupplement();
                        if (supplement == null)
                        {
                            DisplayInfoMsg("Supplement discarded");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Please wait...");
                            controller.AddProduct(supplement, 0);
                            DisplayInfoMsg("Supplement created successfully!", 1800);
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
        private Supplements EnterNewSupplement()
        {
            Supplements supplement = new Supplements();
            int maxNameLenght = 32;
            int maxBrandLenght = 18;
            int maxPriceValue = 99999;
            int maxWeightGrams = 99999;
            int maxDoseStringLenght = 20;
            bool isEscapeKeyPressed = false;
            //Name check
            do
            {
                Console.Write("Supplemnt name(not empty and no more than 32 symbols):");
                List<string> sName = EnterField(out isEscapeKeyPressed);
                if (isEscapeKeyPressed) return null;
                if (sName.Count > 0 && sName.Count <= maxNameLenght)
                {
                    supplement.Name = string.Join("", sName);
                    break;
                }
                DisplayErrorMsg("Name too long or blank!", 2000);
            } while (true);
            Console.Clear();

            //Brand check
            do
            {
                Console.Write("Supplement brand(no more than 20 symbols):");
                List<string> sBrand = EnterField(out isEscapeKeyPressed);
                if (isEscapeKeyPressed) return null;
                if (sBrand.Count > 0 && sBrand.Count <= maxBrandLenght)
                {
                    supplement.Brand = string.Join("", sBrand);
                    break;
                }
                DisplayErrorMsg("Brand name too long or blank!", 2200);

            } while (true);
            Console.Clear();

            //Price check 
            do
            {
                Console.Write($"Supplement price(max value is {maxPriceValue}):");
                List<string> PriceString = EnterField(out isEscapeKeyPressed);
                if (isEscapeKeyPressed) return null;
                string priceString = "";
                foreach (string character in PriceString) priceString += character;
                decimal price;
                bool isDecimal = decimal.TryParse(priceString, out price);
                if (isDecimal)
                {
                    if (price < 0) DisplayErrorMsg("Price cannot be negative!", 2000);
                    if (price >= 0 && price <= maxPriceValue)
                    {
                        supplement.Price = price;
                        break;
                    }
                    if (price > maxPriceValue) DisplayErrorMsg("Price value overreached max value!", 2000);
                }
                else DisplayErrorMsg("Invalid price format or blank!", 2000);
            } while (true);
            Console.Clear();
            
            //Weight check 
            do
            {
                Console.Write($"Enter weight in grams(max weight is {maxWeightGrams}):");
                List<string> WeightString = EnterField(out isEscapeKeyPressed);
                if (isEscapeKeyPressed)
                {
                    supplement = null;
                    return null;
                }
                string weightString = "";
                foreach (string character in WeightString)
                {
                    weightString += character;
                }
                int weight;
                bool isInt = int.TryParse(weightString, out weight);
                if (isInt)
                {
                    if (weight <= 0) DisplayErrorMsg("Weight should be positive");
                    if (weight > 0 && weight <= maxWeightGrams)
                    {
                        supplement.Weight = weightString + "g";
                        Console.Clear();
                        do
                        {
                            Console.Write($"Additional dosage info(optional,max lenght {maxDoseStringLenght}):");
                            List<string> DosageString = EnterField(out isEscapeKeyPressed);
                            if (isEscapeKeyPressed) return null;
                            if (DosageString.Count == 0) break;
                            supplement.Weight += ", ";
                            string dosageString = "";
                            foreach (var character in DosageString)
                            {
                                dosageString += character;
                            }
                            if (dosageString.Length > maxDoseStringLenght)
                            {
                                DisplayErrorMsg("Dosage info too long!", 1800);
                                continue;
                            }
                            supplement.Weight += dosageString;
                            break;
                        } while (true);
                        break;
                    }
                    if (weight > maxWeightGrams) DisplayErrorMsg("Weight value overreached max value!", 2500);
                }
                else DisplayErrorMsg("Invalid weight format or blank!", 1800);
            } while (true);
            supplement.Quantity = 0;
            supplement.Category_id = 1;
            return supplement;
        }


        /// <summary>
        /// Returns a list of user-entered characters. If Escape key is pressed, then isEscapeKeyPressed equals true.
        /// </summary>
        private List<string> EnterField(out bool isEscapeKeyPressed)
        {

            int index = 0;
            string symbolEntered;
            List<string> fieldName = new List<string>();
            isEscapeKeyPressed = false;
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            do
            {
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    isEscapeKeyPressed = true;
                    return null;
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
                    continue;
                }
                if (key.Key != ConsoleKey.Enter)
                {
                    symbolEntered = Convert.ToString(key.KeyChar);
                    fieldName.Add(Convert.ToString(symbolEntered));
                    index++;
                }
            } while (key.Key != ConsoleKey.Enter);
            return fieldName;
        }

        internal void PrintAdminPageHeadder()
        {
            Console.Clear();
            Console.WriteLine("+" + GInterface.HorizontalLine('-', '-', 17) + "+");
            Console.WriteLine("|-<Welcome admin>-|");
            Console.WriteLine("+" + GInterface.HorizontalLine('~', '~', 17) + "+");
        }
        private void DisplayInfoMsg(string infoMessage, int timeoutInMiliseconds = 1000)
        {
            Console.Clear();
            Console.WriteLine(GInterface.HorizontalLine('x', 'x', infoMessage.Length));
            InformingConsoleColor();
            Console.Write(infoMessage);
            System.Threading.Thread.Sleep(timeoutInMiliseconds);
            Console.ResetColor();
            Console.CursorLeft = 0;
            GInterface.DeleteRow();
            Console.Clear();
        }
        private void DisplayErrorMsg(string errorMessage, int timeoutInMiliseconds = 1000)
        {
            Console.Clear();
            Console.WriteLine(GInterface.HorizontalLine('x', 'x', errorMessage.Length));
            WarningConsoleColor();
            Console.Write(errorMessage);
            System.Threading.Thread.Sleep(timeoutInMiliseconds);
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
