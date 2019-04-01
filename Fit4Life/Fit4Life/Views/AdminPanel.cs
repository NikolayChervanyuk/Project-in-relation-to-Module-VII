using Fit4Life.Controllers;
using Fit4Life.Extentions;
using Fit4Life.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Fit4Life.Data;

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
                "Delete supplement;Delete drink;Delete Equipment";
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
                key = Console.ReadKey(true);
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
            int categoryIndex = pickedActionIndex % 3;
            if (pickedActionIndex < 0) return;
            //Restock action
            if (pickedActionIndex >= 0 && pickedActionIndex <= 2)
            {
                switch (pickedActionIndex)
                {
                    case 0://supplement
                        PrintRestockPageHeadder();
                        
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
                            GInterface.SupplementsList = GInterface.GetCategorizedList(categoryIndex, controller);
                            DisplayInfoMsg("Supplement created successfully!", 1800);
                        }
                        break;
                    case 4://drink
                        break;
                    case 5://equipment
                        Equipment equipment = EnterNewEquipment();
                        if (equipment == null) DisplayInfoMsg("Supplement discarded");
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Please wait...");
                            controller.AddProduct(equipment, 2);
                            GInterface.EquipmentsList = GInterface.GetCategorizedList(categoryIndex, controller);
                            DisplayInfoMsg("Equipment created successfully!", 1800);
                        }
                        break;
                }
            }
            //Delete action
            if (pickedActionIndex >= 6 && pickedActionIndex <= 8)
            {
                GInterface.SetWindowSize(GInterface.productPageWindowSize[0], GInterface.productPageWindowSize[1]);
                switch (pickedActionIndex)
                {
                    case 6:
                        PrintDeletionPageHeadder(GInterface.SupplementsList[0].GetType().Name.ToString(), GInterface.SupplementsList.Count);
                        SelectSupplementForDeletion();
                        break;
                    case 7:
                        DrawRedFrame(101, GInterface.SupplementsList.Count + 2, true, 'X', 'x');
                        //SelectDrinkForDeletion();
                        break;
                    case 8:
                        PrintDeletionPageHeadder(GInterface.EquipmentsList[0].GetType().Name.ToString(), GInterface.EquipmentsList.Count);
                        SelectEquipmentForDeletion();
                        break;
                }
                GInterface.SetWindowSize(GInterface.mainPageWindowSize[0], GInterface.mainPageWindowSize[1]);
            }
            Console.Clear();
            return;
        }

        private void PrintRestockPageHeadder()
        {
            throw new NotImplementedException();
        }

        private void SelectEquipmentForDeletion()
        {
            GInterface.SetWindowSize(GInterface.productPageWindowSize[0], GInterface.productPageWindowSize[1]);
            int categoryIndex = 2;
            ObjectSelections.TopOffset = Console.CursorTop;
            GInterface.PrintProductsFormated(categoryIndex);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int equipmentIndex = 0;
            int listLenght = GInterface.EquipmentsList.Count;
            ObjectSelections.SelectCurrentProductAt(equipmentIndex, categoryIndex);
            Console.CursorVisible = false;
            do
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow://select next product
                        ObjectSelections.DeselectCurrentProductAt(equipmentIndex, categoryIndex);
                        if (equipmentIndex + 1 >= listLenght)
                        {
                            equipmentIndex = (equipmentIndex % (listLenght - 1)) - 1;
                        }
                        ObjectSelections.SelectCurrentProductAt(++equipmentIndex, categoryIndex);
                        break;
                    case ConsoleKey.UpArrow://select previous product
                        ObjectSelections.DeselectCurrentProductAt(equipmentIndex, categoryIndex);
                        if (equipmentIndex - 1 < 0)
                        {
                            equipmentIndex = listLenght;
                        }
                        ObjectSelections.SelectCurrentProductAt(--equipmentIndex, categoryIndex);
                        break;
                    case ConsoleKey.Enter:
                        ShowDeletionConfirmationMsg(equipmentIndex);
                        key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                        {
                            Console.Clear();
                            Console.WriteLine("Please wait...");
                            controller.DeleteProduct(GInterface.EquipmentsList[equipmentIndex], categoryIndex);
                            DisplayInfoMsg("Deletion complete successfully!", 2200);
                            GInterface.EquipmentsList = GInterface.GetCategorizedList(categoryIndex, controller);
                            listLenght--;
                            PrintDeletionPageHeadder(GInterface.EquipmentsList[0].GetType().Name.ToString(), listLenght);
                            DrawRedFrame(101, GInterface.EquipmentsList.Count + 2, true, 'X', 'x');
                            GInterface.PrintProductsFormated(categoryIndex);
                            if (equipmentIndex >= listLenght) equipmentIndex--;
                            ObjectSelections.SelectCurrentProductAt(equipmentIndex, categoryIndex);
                        }
                        else
                        {
                            GInterface.PrintProductsFormated(categoryIndex);
                            ObjectSelections.SelectCurrentProductAt(equipmentIndex, categoryIndex);
                        }
                        key = new ConsoleKeyInfo();
                        break;
                }
            } while (key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Enter);
            return;
        }

        private void SelectSupplementForDeletion()
        {
            GInterface.SetWindowSize(GInterface.productPageWindowSize[0], GInterface.productPageWindowSize[1]);
            int categoryIndex = 0;
            ObjectSelections.TopOffset = Console.CursorTop;
            GInterface.PrintProductsFormated(categoryIndex);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int supplementIndex = 0;
            int listLenght = GInterface.SupplementsList.Count;
            ObjectSelections.SelectCurrentProductAt(supplementIndex, categoryIndex);
            Console.CursorVisible = false;
            do
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow://select next product
                        ObjectSelections.DeselectCurrentProductAt(supplementIndex, categoryIndex);
                        if (supplementIndex + 1 >= listLenght)
                        {
                            supplementIndex = (supplementIndex % (listLenght - 1)) - 1;
                        }
                        ObjectSelections.SelectCurrentProductAt(++supplementIndex, categoryIndex);
                        break;
                    case ConsoleKey.UpArrow://select previous product
                        ObjectSelections.DeselectCurrentProductAt(supplementIndex, categoryIndex);
                        if (supplementIndex - 1 < 0)
                        {
                            supplementIndex = listLenght;
                        }
                        ObjectSelections.SelectCurrentProductAt(--supplementIndex, categoryIndex);
                        break;
                    case ConsoleKey.Enter:
                        ShowDeletionConfirmationMsg(supplementIndex);
                        key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                        {
                            Console.Clear();
                            Console.WriteLine("Please wait...");
                            controller.DeleteProduct(GInterface.SupplementsList[supplementIndex], categoryIndex);
                            DisplayInfoMsg("Deletion complete successfully!", 2200);
                            GInterface.SupplementsList = GInterface.GetCategorizedList(categoryIndex, controller);
                            listLenght--;
                            PrintDeletionPageHeadder(GInterface.SupplementsList[0].GetType().Name.ToString(), listLenght);
                            GInterface.PrintProductsFormated(categoryIndex);
                            if (supplementIndex >= listLenght) supplementIndex--;
                            ObjectSelections.SelectCurrentProductAt(supplementIndex, categoryIndex);
                        }
                        else
                        {
                            GInterface.PrintProductsFormated(categoryIndex);
                            ObjectSelections.SelectCurrentProductAt(supplementIndex, categoryIndex);

                        }
                        key = new ConsoleKeyInfo();
                        break;
                }
            } while (key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Enter);
            return;
        }

        private void PrintDeletionPageHeadder(string nameOfCategory, int height)
        {
            Console.WriteLine(GInterface.HorizontalLine('-', '*', 100));
            GInterface.ShiftText(37);
            Console.WriteLine($"{nameOfCategory}s for deletion:");
            Console.WriteLine(GInterface.HorizontalLine('-', '*', 100));
            DrawRedFrame(101, height + 2, true, 'X', 'x');
        }

        /// <summary>
        /// Draws a frame. If resetCursor is true, then cursor is set to it's initial position, 
        /// else it stays to it's last position.
        /// </summary>
        private void DrawRedFrame(int width, int height, bool resetCursos = false, char edges = ' ', char character = ' ')
        {
            WarningConsoleColor();
            int[] cursorPosition = { Console.CursorLeft, Console.CursorTop + 1 };
            Console.Write(GInterface.HorizontalLine(character, edges, width));
            Console.CursorLeft--;
            GInterface.DrawVerticalLine(character, edges, height);
            Console.CursorLeft = 0;
            Console.WriteLine(GInterface.HorizontalLine(character, edges, width));
            if (resetCursos) Console.SetCursorPosition(cursorPosition[0], cursorPosition[1]);
            Console.ResetColor();
        }

        private void ShowDeletionConfirmationMsg(int productIndex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorTop = ObjectSelections.TopOffset + productIndex;
            Console.CursorLeft = 0;
            GInterface.ShiftText(16, true);
            Console.Write("Are you sure you want to delete this product?(Press Enter to confirm)");
            Console.Write(new string(' ', 15));
            Console.ResetColor();
        }

        /// <summary>
        /// Creates new supplement by entering the properties of supplement.
        /// If the method is terminated via pressing the Escape key,then the supplement equals null.
        /// </summary>
        private Supplements EnterNewSupplement()
        {
            Supplements supplement = new Supplements();
            int minNameLenght = 1;
            int maxNameLenght = 28;
            int minBrandLenght = 1;
            int maxBrandLenght = 18;
            decimal minPriceValue = 0.001m;
            decimal maxPriceValue = 99999m;
            int minWeightGrams = 1;
            int maxWeightGrams = 99999;
            int minDoseStringLenght = 0;
            int maxDoseStringLenght = 20;
            string description = $"Enter supplement name(max lenght {maxNameLenght})";
            supplement.Name = StringInput(minNameLenght, maxNameLenght, description);
            if (supplement.Name == null) return null;
            Console.Clear();

            description = $"Enter supplement brand(max lenght {maxBrandLenght})";
            supplement.Brand = StringInput(minBrandLenght, maxBrandLenght, description);

            if (supplement.Brand == null) return null;
            Console.Clear();

            description = $"Enter price(max value {maxPriceValue})";
            supplement.Price = decimal.Parse(NumberInput(minPriceValue, maxPriceValue, "decimal", description).ToString());
            if (supplement.Price < 0) return null;
            Console.Clear();

            description = $"Enter weight (max weight {maxWeightGrams})";
            supplement.Weight = NumberInput(minWeightGrams, maxWeightGrams, "int", description).ToString();
            if (supplement.Weight == null) return null;
            supplement.Weight += "g";

            description = $"Enter dose info (max lenght {maxDoseStringLenght}, optional)";
            string doseInfo = StringInput(minDoseStringLenght, maxDoseStringLenght, description);
            supplement.Weight += ", " + doseInfo;
            supplement.Quantity = 0;
            supplement.Category_id = 1; //<<< remove?
            return supplement;
        }
        /// <summary>
        /// Creates new Drink by entering the properties of supplement.
        /// If the method is terminated via pressing the Escape key,then the supplement equals null.
        /// </summary>
        /*private Drink EnterNewEquipment()
        {

        }*/
        /// <summary>
        /// Creates new equipment by entering the properties of supplement.
        /// If the method is terminated via pressing the Escape key,then the supplement equals null.
        /// </summary>
        private Equipment EnterNewEquipment()
        {
            Equipment equipment = new Equipment();
            int minNameLenght = 1;
            int maxNameLenght = 32;
            int minBrandLenght = 1;
            int maxBrandLenght = 24;
            decimal minPriceValue = 0.001m;
            decimal maxPriceValue = 99999m;
            string description = $"Enter equipment name(max lenght {maxNameLenght})";
            equipment.Name = StringInput(minNameLenght, maxNameLenght, description);
            if (equipment.Name == null) return null;
            Console.Clear();

            description = $"Enter equipment brand(max lenght {maxBrandLenght})";
            equipment.Brand = StringInput(minBrandLenght, maxBrandLenght, description);
            if (equipment.Brand == null) return null;
            Console.Clear();

            description = $"Enter price(max value {maxPriceValue})";
            equipment.Price = decimal.Parse(NumberInput(minPriceValue, maxPriceValue, "decimal", description).ToString());
            if (equipment.Price < 0) return null;
            Console.Clear();

            equipment.Quantity = 0;
            equipment.Category_id = 1; //<<< remove?
            return equipment;
        }


        ///<summary>
        ///   do
        ///   {
        ///       Console.Write("Supplemnt name(not empty and no more than 32 symbols):");
        ///       List<string> sName = EnterField(out isEscapeKeyPressed);
        ///       if (isEscapeKeyPressed) return supplement = null;
        ///       if (sName.Count > 0 && sName.Count <= maxNameLenght)
        ///       {
        ///           supplement.Name = string.Join("", sName);
        ///           break;
        ///       }
        ///       DisplayErrorMsg("Name too long or blank!", 2000);
        ///   } while (true);
        ///   Console.Clear();
        ///   //Brand check
        ///   do
        ///   {
        ///       Console.Write("Supplement brand(no more than 20 symbols):");
        ///       List<string> sBrand = EnterField(out isEscapeKeyPressed);
        ///       if (isEscapeKeyPressed) return null;
        ///       if (sBrand.Count > 0 && sBrand.Count <= maxBrandLenght)
        ///       {
        ///           supplement.Brand = string.Join("", sBrand);
        ///           break;
        ///       }
        ///       DisplayErrorMsg("Brand name too long or blank!", 2200);
        ///
        ///   } while (true);
        ///   Console.Clear();
        ///
        ///   //Price check 
        ///   do
        ///   {
        ///       Console.Write($"Supplement price(max value is {maxPriceValue}):");
        ///       List<string> PriceString = EnterField(out isEscapeKeyPressed);
        ///       if (isEscapeKeyPressed) return null;
        ///       string priceString = "";
        ///       foreach (string character in PriceString) priceString += character;
        ///       decimal price;
        ///       bool isDecimal = decimal.TryParse(priceString, out price);
        ///       if (isDecimal)
        ///       {
        ///           if (price < 0) DisplayErrorMsg("Price cannot be negative!", 2000);
        ///           if (price >= 0 && price <= maxPriceValue)
        ///           {
        ///               supplement.Price = price;
        ///               break;
        ///           }
        ///           if (price > maxPriceValue) DisplayErrorMsg("Price value overreached max value!", 2000);
        ///       }
        ///       else DisplayErrorMsg("Invalid price format or blank!", 2000);
        ///   } while (true);
        ///   Console.Clear();
        ///
        ///   //Weight check 
        ///   do
        ///   {
        ///       Console.Write($"Enter weight in grams(max weight is {maxWeightGrams}):");
        ///       List<string> WeightString = EnterField(out isEscapeKeyPressed);
        ///       if (isEscapeKeyPressed)
        ///       {
        ///           supplement = null;
        ///           return null;
        ///       }
        ///       string weightString = "";
        ///       foreach (string character in WeightString)
        ///       {
        ///           weightString += character;
        ///       }
        ///       int weight;
        ///       bool isInt = int.TryParse(weightString, out weight);
        ///       if (isInt)
        ///       {
        ///           if (weight <= 0) DisplayErrorMsg("Weight should be positive");
        ///           if (weight > 0 && weight <= maxWeightGrams)
        ///           {
        ///               supplement.Weight = weightString + "g";
        ///               Console.Clear();
        ///               do
        ///               {
        ///                   Console.Write($"Additional dosage info(optional,max lenght {maxDoseStringLenght}):");
        ///                   List<string> DosageString = EnterField(out isEscapeKeyPressed);
        ///                   if (isEscapeKeyPressed) return null;
        ///                   if (DosageString.Count == 0) break;
        ///                   supplement.Weight += ", ";
        ///                   string dosageString = "";
        ///                   foreach (var character in DosageString)
        ///                   {
        ///                       dosageString += character;
        ///                   }
        ///                   if (dosageString.Length > maxDoseStringLenght)
        ///                   {
        ///                       DisplayErrorMsg("Dosage info too long!", 1800);
        ///                       continue;
        ///                   }
        ///                   supplement.Weight += dosageString;
        ///                   break;
        ///               } while (true);
        ///               break;
        ///           }
        ///           if (weight > maxWeightGrams) DisplayErrorMsg("Weight value overreached max value!", 2500);
        ///       }
        ///       else DisplayErrorMsg("Invalid weight format or blank!", 1800);
        ///   } while (true);
        ///<summary/>

        private object NumberInput(dynamic minNumberValue, dynamic maxNumberValue, string typeOfnumber, string descriptionInfo = "Number:")
        {

            bool isEscapeKeyPressed = false;
            do
            {
                Console.Write($"{descriptionInfo}:");
                List<string> PriceString = EnterField(out isEscapeKeyPressed);
                if (isEscapeKeyPressed) return -1;
                string priceString = "";
                foreach (string character in PriceString) priceString += character;
                if (IsTypeOfNumberRequested(typeOfnumber, priceString))
                {
                    dynamic price = GetNumberRequested(typeOfnumber, priceString);
                    if (price < minNumberValue) DisplayErrorMsg($"Value cannot be less than {minNumberValue}!", 2200);
                    if (price >= 0 && price <= maxNumberValue) return price;
                    if (price > maxNumberValue) DisplayErrorMsg($"Max value overreached!", 2000);
                }
                else DisplayErrorMsg($"Invalid number format or blank!", 2000);
            } while (true);
        }

        private string StringInput(int minStringLenght, int maxStringLenght, string descriptionInfo = "String")
        {
            bool isEscapeKeyPressed = false;
            do
            {
                Console.Write($"{descriptionInfo}:");
                List<string> fieldName = EnterField(out isEscapeKeyPressed);
                if (isEscapeKeyPressed) return null;
                if (fieldName.Count >= minStringLenght && fieldName.Count <= maxStringLenght)
                {
                    return string.Join("", fieldName);
                }
                DisplayErrorMsg($"String too long or blank!", 2000);
            } while (true);
        }

        internal void PrintAdminPageHeadder()
        {
            Console.Clear();
            Console.WriteLine("+" + GInterface.HorizontalLine('-', '-', 17) + "+");
            Console.WriteLine("|-<Welcome admin>-|");
            Console.WriteLine("+" + GInterface.HorizontalLine('~', '~', 17) + "+");
        }

        /// <summary>
        /// Returns priceString from type, dependent from typeOfnumber.
        /// </summary>
        private dynamic GetNumberRequested(string typeOfnumber, string numberString)
        {
            switch (typeOfnumber.ToLower())
            {
                case "int":
                    return int.Parse(numberString);
                case "double":
                    return double.Parse(numberString);
                case "decimal":
                    return decimal.Parse(numberString);
            }
            return null;
        }
        /// <summary>
        ///  Determines wheather numberString can be parsed to typeOfnumber type and reterns true if possible, otherwise false.
        /// </summary>
        private bool IsTypeOfNumberRequested(string typeOfnumber, string numberInString)
        {
            switch (typeOfnumber.ToLower())
            {
                case "int":
                    int intNum;
                    bool isInt = int.TryParse(numberInString, out intNum);
                    if (isInt) return true;
                    break;
                case "double":
                    double doubleNum;
                    bool isDouble = double.TryParse(numberInString, out doubleNum);
                    if (isDouble) return true;
                    break;
                case "decimal":
                    decimal decimalNum;
                    bool isDecimal = decimal.TryParse(numberInString, out decimalNum);
                    if (isDecimal) return true;
                    break;
                default: throw new InvalidOperationException("Unrecognized number type");
            }
            return false;
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

