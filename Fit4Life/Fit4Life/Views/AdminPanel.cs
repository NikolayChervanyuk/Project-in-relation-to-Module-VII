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
using System.IO;

namespace Fit4Life.Views
{
    internal class AdminPanel
    {
        private Controller controller;
        private static string actions;
        int pickedActionIndex = -1;
        internal static string adminPassword;
        private const int SupplementIndex = Display.SupplementIndex;
        private const int drinksIndex = Display.drinksIndex;
        private const int equipmentsIndex = Display.equipmentsIndex;
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
                "Delete supplement;Delete drink;Delete Equipment;" +
                "Change password";
            return true;
        }
        private void ChangeAdminPassword()
        {
            string description = "Enter new password(min lenght 4 characters):";
            string newPassword = StringInput(4, 64, true, description);
            if (newPassword == null) return;
            Console.Clear();
            string retypedPassword = null;
            do
            {
                if(retypedPassword != null) DisplayErrorMsg("Passwords do not match!", 1200);
                retypedPassword = StringInput(1, 64, true, "Reneter your new password:");
                if (retypedPassword == null) return;
            } while (retypedPassword != newPassword);
            // create new password
            using (StreamWriter writer = new StreamWriter("adminpwrd.txt", false))
            {
                writer.Write(retypedPassword);
                writer.Close();
            }
            adminPassword = retypedPassword;
            DisplayInfoMsg("Password saved!", 1500);
            return;
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
                GInterface.SetWindowSize(GInterface.productPageWindowSize[0], GInterface.productPageWindowSize[1]);
                switch (pickedActionIndex)
                {
                    case SupplementIndex://supplement
                        PrintRestockPageHeadder(GInterface.SupplementList[0].GetType().Name.ToString(), GInterface.SupplementList.Count);
                        SelectSupplementForRestock();
                        break;
                    case drinksIndex://drink
                        PrintRestockPageHeadder(GInterface.DrinksList[0].GetType().Name.ToString(), GInterface.DrinksList.Count);
                        SelectDrinkForRestock();
                        break;
                    case equipmentsIndex://equipment
                        PrintRestockPageHeadder(GInterface.EquipmentsList[0].GetType().Name.ToString(), GInterface.EquipmentsList.Count);
                        SelectEquipmentForRestock();
                        break;
                }
            }
            //Add action
            if (pickedActionIndex >= 3 && pickedActionIndex <= 5)
            {
                switch (pickedActionIndex)
                {
                    case 3://supplement
                        Supplement supplement = EnterNewSupplement();
                        if (supplement == null)
                        {
                            DisplayInfoMsg("Supplement discarded");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Please wait...");
                            controller.AddProduct(supplement, 0);
                            GInterface.SupplementList = GInterface.GetCategorizedList(categoryIndex, controller);
                            DisplayInfoMsg("Supplement created successfully!", 1800);
                        }
                        break;
                    case 4://drink
                        Drink drink = EnterNewDrink();
                        if (drink == null) DisplayInfoMsg("Supplement discarded");
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Please wait...");
                            controller.AddProduct(drink, 1);
                            GInterface.DrinksList = GInterface.GetCategorizedList(categoryIndex, controller);
                            DisplayInfoMsg("Drink created successfully!", 1800);
                        }
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
                        PrintDeletionPageHeadder(GInterface.SupplementList[0].GetType().Name.ToString(), GInterface.SupplementList.Count);
                        SelectSupplementForDeletion();
                        break;
                    case 7:
                        PrintDeletionPageHeadder(GInterface.DrinksList[0].GetType().Name.ToString(), GInterface.DrinksList.Count);
                        SelectDrinkForDeletion();
                        break;
                    case 8:
                        PrintDeletionPageHeadder(GInterface.EquipmentsList[0].GetType().Name.ToString(), GInterface.EquipmentsList.Count);
                        SelectEquipmentForDeletion();
                        break;
                }
                GInterface.SetWindowSize(GInterface.mainPageWindowSize[0], GInterface.mainPageWindowSize[1]);
            }
            if (pickedActionIndex == 9)
            {
                ChangeAdminPassword();
            }
            Console.Clear();
            return;
        }

        /// <summary>
        /// Returns true if the key pressed is Y, else false.
        /// </summary>
        private bool IsConfirmedRestockAction()
        {
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                switch (key.KeyChar.ToString().ToLower())
                {
                    case "y": return true;
                    case "n": return false;
                }
            } while (key.Key != ConsoleKey.Escape);
            return false;
        }
        /// <summary>
        /// Prints the Supplement and enables their selection. 
        /// If enter is pressed on the selected supplement, restock process is initiated for it.
        /// </summary>
        private void SelectSupplementForRestock()
        {
            GInterface.SetWindowSize(GInterface.productPageWindowSize[0], GInterface.productPageWindowSize[1]);
            int categoryIndex = 0;
            ObjectSelections.TopOffset = Console.CursorTop;
            GInterface.PrintProductsFormated(categoryIndex);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int supplementIndex = 0;
            int listLenght = GInterface.SupplementList.Count;
            ObjectSelections.SelectCurrentProductAt(supplementIndex, categoryIndex);
            //Console.CursorVisible = false;
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
                        RestockSelectedProduct(GInterface.SupplementList[supplementIndex], supplementIndex, categoryIndex);
                        ObjectSelections.SelectCurrentProductAt(supplementIndex, categoryIndex);
                        break;
                }
            } while (key.Key != ConsoleKey.Escape);
            return;
        }
        /// <summary>
        /// Prints the drinks and enables their selection. 
        /// If enter is pressed on the selected drink, restock process is initiated for it.
        /// </summary>
        private void SelectDrinkForRestock()
        {
            GInterface.SetWindowSize(GInterface.productPageWindowSize[0], GInterface.productPageWindowSize[1]);
            int categoryIndex = 1;
            ObjectSelections.TopOffset = Console.CursorTop;
            GInterface.PrintProductsFormated(categoryIndex);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int drinksIndex = 1;
            int listLenght = GInterface.DrinksList.Count;
            ObjectSelections.SelectCurrentProductAt(drinksIndex, categoryIndex);
            //Console.CursorVisible = false;
            do
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow://select next product
                        ObjectSelections.DeselectCurrentProductAt(drinksIndex, categoryIndex);
                        if (drinksIndex + 1 >= listLenght)
                        {
                            drinksIndex = (drinksIndex % (listLenght - 1)) - 1;
                        }
                        ObjectSelections.SelectCurrentProductAt(++drinksIndex, categoryIndex);
                        break;
                    case ConsoleKey.UpArrow://select previous product
                        ObjectSelections.DeselectCurrentProductAt(drinksIndex, categoryIndex);
                        if (drinksIndex - 1 < 0)
                        {
                            drinksIndex = listLenght;
                        }
                        ObjectSelections.SelectCurrentProductAt(--drinksIndex, categoryIndex);
                        break;
                    case ConsoleKey.Enter:
                        RestockSelectedProduct(GInterface.DrinksList[drinksIndex], drinksIndex, categoryIndex);
                        ObjectSelections.SelectCurrentProductAt(drinksIndex, categoryIndex);
                        break;
                }
            } while (key.Key != ConsoleKey.Escape);
            return;
        }
        /// <summary>
        /// Prints the equipments and enables their selection.
        /// If enter is pressed on the selected equipmenet, restock process is initiated for it.
        /// </summary>
        private void SelectEquipmentForRestock()
        {
            GInterface.SetWindowSize(GInterface.productPageWindowSize[0], GInterface.productPageWindowSize[1]);
            int categoryIndex = 2;
            ObjectSelections.TopOffset = Console.CursorTop;
            GInterface.PrintProductsFormated(categoryIndex);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int equipmentIndex = 0;
            int listLenght = GInterface.EquipmentsList.Count;
            ObjectSelections.SelectCurrentProductAt(equipmentIndex, categoryIndex);
            //Console.CursorVisible = false;
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
                        RestockSelectedProduct(GInterface.EquipmentsList[equipmentIndex], equipmentIndex, categoryIndex);
                        ObjectSelections.SelectCurrentProductAt(equipmentIndex, categoryIndex);
                        break;
                }
            } while (key.Key != ConsoleKey.Escape);
            return;
        }
        /// <summary>
        /// Restocks selected product by increasing it's quantity.
        /// </summary>
        /// <param name="product">Product to be restocked</param>
        private void RestockSelectedProduct(object product, int productIndex, int categoryIndex)
        {
            int maxQuantityValue = 99999999;
            AwaitInputForQuantityBox(categoryIndex);
            Console.ResetColor();
            //After executing the program lines above, the cursor is set below the product in a box
            Console.CursorVisible = true;
            int quantityToAdd;
            switch (categoryIndex)
            {
                case SupplementIndex:
                    Supplement supplement = (Supplement)product;
                    Shapes.WriteBottomRestockInfo(product, categoryIndex);
                    quantityToAdd = int.Parse(NumberInput(1, maxQuantityValue, "int", false, "", 8).ToString());
                    if (quantityToAdd <= 0)
                    {
                        Console.ResetColor();
                        DisplayInfoMsg("Restock action cancelled", 1200);

                    }
                    else
                    {
                        Shapes.ShowFinalRestockInfo(product, categoryIndex, quantityToAdd);
                        if (IsConfirmedRestockAction())
                        {
                            controller.IncreaseQuantityOf(supplement, categoryIndex, quantityToAdd);
                            GInterface.SupplementList = (List<Supplement>)GInterface.GetCategorizedList(categoryIndex, controller);
                            DisplayInfoMsg("Supplement restocked successfully!", 1500);
                        }
                        else DisplayInfoMsg("Restock action cancelled", 1200);
                    }
                    PrintRestockPageHeadder(supplement.GetType().Name.ToString(), GInterface.SupplementList.Count);

                    break;
                case drinksIndex:
                    Drink drink = (Drink)product;
                    Shapes.WriteBottomRestockInfo(product, categoryIndex);
                    quantityToAdd = int.Parse(NumberInput(1, maxQuantityValue, "int", false, "", 8).ToString());
                    if (quantityToAdd <= 0)
                    {
                        Console.ResetColor();
                        DisplayInfoMsg("Restock action cancelled", 1200);
                    }
                    else
                    {
                        Shapes.ShowFinalRestockInfo(product, categoryIndex, quantityToAdd);
                        if (IsConfirmedRestockAction())
                        {
                            controller.IncreaseQuantityOf(drink, categoryIndex, quantityToAdd);
                            GInterface.DrinksList = (List<Drink>)GInterface.GetCategorizedList(categoryIndex, controller);
                            DisplayInfoMsg("Drink restocked successfully!", 1500);
                        }
                        else DisplayInfoMsg("Restock action cancelled", 1200);

                    }
                    PrintRestockPageHeadder(drink.GetType().Name.ToString(), GInterface.DrinksList.Count);
                    break;
                case equipmentsIndex:
                    Equipment equipment = (Equipment)product;
                    Shapes.WriteBottomRestockInfo(product, categoryIndex);
                    quantityToAdd = int.Parse(NumberInput(1, maxQuantityValue, "int", false, "", 8).ToString());
                    if (quantityToAdd <= 0)
                    {
                        Console.ResetColor();
                        DisplayInfoMsg("Restock action cancelled", 1200);
                    }
                    else
                    {
                        Shapes.ShowFinalRestockInfo(product, categoryIndex, quantityToAdd);
                        if (IsConfirmedRestockAction())
                        {
                            controller.IncreaseQuantityOf(equipment, categoryIndex, quantityToAdd);
                            GInterface.EquipmentsList = (List<Equipment>)GInterface.GetCategorizedList(categoryIndex, controller);
                            DisplayInfoMsg("Equipment restocked successfully!", 1500);
                        }
                        else DisplayInfoMsg("Restock action cancelled", 1200);
                    }
                    PrintRestockPageHeadder(equipment.GetType().Name.ToString(), GInterface.SupplementList.Count);
                    break;
            }
            GInterface.PrintProductsFormated(categoryIndex);
            ObjectSelections.SelectCurrentProductAt(productIndex, categoryIndex);
            Console.CursorVisible = false;
            Console.ResetColor();
            return;
        }

        /// <summary>
        /// Creates new supplement by entering the properties of supplement.
        /// If the method is terminated via pressing the Escape key,then the supplement equals null.
        /// </summary>
        private Supplement EnterNewSupplement()
        {
            Supplement supplement = new Supplement();
            int minNameLenght = 1;
            int maxNameLenght = 28;
            int minBrandLenght = 1;
            int maxBrandLenght = 18;
            decimal minPriceValue = 0.001m;
            decimal maxPriceValue = 99999m;
            int minWeightGrams = 1;
            int maxWeightGrams = 99999;
            int minDoseStringLenght = 0;
            int maxDoseStringLenght = 16;
            string description = $"Enter supplement name(max lenght {maxNameLenght}):";
            supplement.Name = StringInput(minNameLenght, maxNameLenght, true, description);
            if (supplement.Name == null) return null;
            Console.Clear();

            description = $"Enter supplement brand(max lenght {maxBrandLenght}):";
            supplement.Brand = StringInput(minBrandLenght, maxBrandLenght, true, description);

            if (supplement.Brand == null) return null;
            Console.Clear();

            description = $"Enter price(max value {maxPriceValue}):";
            supplement.Price = decimal.Parse(NumberInput(minPriceValue, maxPriceValue, "decimal", true, description).ToString());
            if (supplement.Price < 0) return null;
            Console.Clear();

            description = $"Enter weight in grams(max weight {maxWeightGrams}):";
            supplement.Weight = NumberInput(minWeightGrams, maxWeightGrams, "int", true, description).ToString();
            if (supplement.Weight == "-1") return null;
            supplement.Weight += "g";
            Console.Clear();

            description = $"Enter dose info (max lenght {maxDoseStringLenght}, optional):";
            string doseInfo = StringInput(minDoseStringLenght, maxDoseStringLenght, true, description);
            if (doseInfo == null) return null;
            supplement.Quantity = 0;
            if (doseInfo.Equals("")) return supplement;
            supplement.Weight += ", " + doseInfo;
            return supplement;
        }
        /// <summary>
        /// Creates new Drink by entering the properties of supplement.
        /// If the method is terminated via pressing the Escape key,then the drink equals null.
        /// </summary>
        private Drink EnterNewDrink()
        {
            Drink drink = new Drink();
            int minNameLenght = 1;
            int maxNameLenght = 32;
            int minMlValue = 1;
            int maxMlValue = 99999;
            decimal minPriceValue = 0.001m;
            decimal maxPriceValue = 99999m;
            string description = $"Enter drink name(max lenght {maxNameLenght}):";
            drink.Name = StringInput(minNameLenght, maxNameLenght, true, description);
            if (drink.Name == null) return null;
            Console.Clear();

            description = $"Enter mililiters(max value {maxMlValue}):";
            drink.Mililiters = NumberInput(minMlValue, maxMlValue, "int", true, description, maxMlValue.ToString().Length).ToString();
            if (int.Parse(drink.Mililiters) < 0) return null;
            drink.Mililiters += "mL";
            Console.Clear();

            description = $"Enter price(max value {maxPriceValue}):";
            drink.Price = decimal.Parse(NumberInput(minPriceValue, maxPriceValue, "decimal", true, description).ToString());
            if (drink.Price < 0) return null;
            Console.Clear();

            drink.Quantity = 0;
            return drink;
        }
        /// <summary>
        /// Creates new equipment by entering the properties of supplement.
        /// If the method is terminated via pressing the Escape key,then the equipment equals null.
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
            string description = $"Enter equipment name(max lenght {maxNameLenght}):";
            equipment.Name = StringInput(minNameLenght, maxNameLenght, true, description);
            if (equipment.Name == null) return null;
            Console.Clear();

            description = $"Enter equipment brand(max lenght {maxBrandLenght}):";
            equipment.Brand = StringInput(minBrandLenght, maxBrandLenght, true, description);
            if (equipment.Brand == null) return null;
            Console.Clear();

            description = $"Enter price(max value {maxPriceValue}):";
            equipment.Price = decimal.Parse(NumberInput(minPriceValue, maxPriceValue, "decimal", true, description).ToString());
            if (equipment.Price < 0) return null;
            Console.Clear();

            equipment.Quantity = 0;
            return equipment;
        }

        /// <summary>
        /// Prints the Supplement and enables their selection. 
        /// If enter is pressed on the selected supplement, deletion process is initiated for it.
        /// </summary>
        private void SelectSupplementForDeletion()
        {
            GInterface.SetWindowSize(GInterface.productPageWindowSize[0], GInterface.productPageWindowSize[1]);
            int categoryIndex = 0;
            ObjectSelections.TopOffset = Console.CursorTop;
            GInterface.PrintProductsFormated(categoryIndex);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int supplementIndex = 0;
            int listLenght = GInterface.SupplementList.Count;
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
                            controller.DeleteProduct(GInterface.SupplementList[supplementIndex], categoryIndex);
                            DisplayInfoMsg("Deletion completed successfully!", 2200);
                            GInterface.SupplementList = GInterface.GetCategorizedList(categoryIndex, controller);
                            if (supplementIndex >= --listLenght) supplementIndex--;
                            PrintDeletionPageHeadder(GInterface.SupplementList[0].GetType().Name.ToString(), listLenght);
                            GInterface.PrintProductsFormated(categoryIndex);
                        }
                        ObjectSelections.SelectCurrentProductAt(supplementIndex, categoryIndex);
                        key = new ConsoleKeyInfo();
                        break;
                }
            } while (key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Enter);
            return;
        }
        /// <summary>
        /// Prints the drinks and enables their selection. 
        /// If enter is pressed on the selected drink, deletion process is initiated for it.
        /// </summary>
        private void SelectDrinkForDeletion()
        {
            GInterface.SetWindowSize(GInterface.productPageWindowSize[0], GInterface.productPageWindowSize[1]);
            int categoryIndex = 1;
            ObjectSelections.TopOffset = Console.CursorTop;
            GInterface.PrintProductsFormated(categoryIndex);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int drinkIndex = 0;
            int listLenght = GInterface.DrinksList.Count;
            ObjectSelections.SelectCurrentProductAt(drinkIndex, categoryIndex);
            Console.CursorVisible = false;
            do
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow://select next product
                        ObjectSelections.DeselectCurrentProductAt(drinkIndex, categoryIndex);
                        if (drinkIndex + 1 >= listLenght)
                        {
                            drinkIndex = (drinkIndex % (listLenght - 1)) - 1;
                        }
                        ObjectSelections.SelectCurrentProductAt(++drinkIndex, categoryIndex);
                        break;
                    case ConsoleKey.UpArrow://select previous product
                        ObjectSelections.DeselectCurrentProductAt(drinkIndex, categoryIndex);
                        if (drinkIndex - 1 < 0)
                        {
                            drinkIndex = listLenght;
                        }
                        ObjectSelections.SelectCurrentProductAt(--drinkIndex, categoryIndex);
                        break;
                    case ConsoleKey.Enter:
                        ShowDeletionConfirmationMsg(drinkIndex);
                        key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                        {
                            Console.Clear();
                            Console.WriteLine("Please wait...");
                            controller.DeleteProduct(GInterface.DrinksList[drinkIndex], categoryIndex);
                            DisplayInfoMsg("Deletion completed successfully!", 2200);
                            GInterface.DrinksList = GInterface.GetCategorizedList(categoryIndex, controller);
                            if (drinkIndex >= --listLenght) drinkIndex--;
                            PrintDeletionPageHeadder(GInterface.DrinksList[0].GetType().Name.ToString(), listLenght);
                            GInterface.PrintProductsFormated(categoryIndex);
                        }
                        ObjectSelections.SelectCurrentProductAt(drinkIndex, categoryIndex);
                        key = new ConsoleKeyInfo();
                        break;
                }
            } while (key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Enter);
            return;
        }
        /// <summary>
        /// Prints the equipment and enables their selection. 
        /// If enter is pressed on the selected equipment, deletion process is initiated for it.
        /// </summary>
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
                            DisplayInfoMsg("Deletion completed successfully!", 2200);
                            GInterface.EquipmentsList = GInterface.GetCategorizedList(categoryIndex, controller);
                            if (equipmentIndex >= --listLenght) equipmentIndex--;
                            PrintDeletionPageHeadder(GInterface.EquipmentsList[0].GetType().Name.ToString(), listLenght);
                            GInterface.PrintProductsFormated(categoryIndex);
                        }
                        ObjectSelections.SelectCurrentProductAt(equipmentIndex, categoryIndex);
                        key = new ConsoleKeyInfo();
                        break;
                }
            } while (key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Enter);
            return;
        }

        private void AwaitInputForQuantityBox(int categoryIndex, int flashTimes = 2)
        {
            int[] initialCursorPos = { Console.CursorLeft, Console.CursorTop };
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorLeft = 0;
            Console.Write(GInterface.HorizontalLine(' ', ' ', 100));
            Console.CursorLeft = 45;
            Console.Write($"<<<Selected>>>");
            int topOffset = ObjectSelections.TopOffset + 2;
            switch (categoryIndex)
            {
                case SupplementIndex:
                    topOffset += GInterface.SupplementList.Count;
                    break;
                case drinksIndex:
                    topOffset += GInterface.DrinksList.Count;
                    break;
                case equipmentsIndex:
                    topOffset += GInterface.EquipmentsList.Count;
                    break;
            }
            int leftOffset = Console.CursorLeft = 0;
            Console.SetCursorPosition(leftOffset, topOffset - 1);
            Console.WriteLine(GInterface.HorizontalLine('=', '=', 101));
            for (int i = 0; i < flashTimes; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                DrawRestockProductFrame(leftOffset, topOffset);
                Thread.Sleep(200);
                Console.ResetColor();
                DrawRestockProductFrame(leftOffset, topOffset);
                if (Console.KeyAvailable) break;
                Thread.Sleep(200);
            }
            Console.CursorLeft++;
            Console.CursorTop++;
            return;
        }
        private void DrawRestockProductFrame(int leftOffset, int topOffset)
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
        private void PrintRestockPageHeadder(string nameOfCategory, int height)
        {
            Console.Clear();
            Console.WriteLine(GInterface.HorizontalLine('-', '*', 100));
            GInterface.ShiftText(37);
            Console.WriteLine($"{nameOfCategory}s to restock:");
            Console.WriteLine(GInterface.HorizontalLine('-', '*', 100));
            Shapes.DrawFrame(101, height + 2, true, '#', '+', "blue");
        }
        private void PrintDeletionPageHeadder(string nameOfCategory, int height)
        {
            Console.WriteLine(GInterface.HorizontalLine('-', '*', 100));
            GInterface.ShiftText(37);
            Console.WriteLine($"{nameOfCategory}s for deletion:");
            Console.WriteLine(GInterface.HorizontalLine('-', '*', 100));
            Shapes.DrawFrame(101, height + 2, true, 'X', 'x', "red");
        }

        /// <summary>
        /// Returns number from type typeOfNumber within the range of minNumberValue and maxNumberValue.
        /// Input is requested until the criterias in the signature are met. If Escape key is pressed,
        /// the method returns -1.
        /// </summary>
        /// <param name="minNumberValue">Sets lowest value for the number</param>
        /// <param name="maxNumberValue">Sets highest value for the number</param>
        /// <param name="typeOfnumber">Defines the number type</param>
        /// <param name="descriptionInfo">Prints a string before number</param>
        /// <param name="errorMsg">If set to true, then, when the input is incorrect, the console clears and shows appropriate message</param>
        /// <returns></returns>
        private object NumberInput(dynamic minNumberValue, dynamic maxNumberValue, string typeOfnumber, bool errorMsg = true, string descriptionInfo = "", int stringLenght = 255)
        {

            bool isEscapeKeyPressed = false;
            int[] initialCursorPos = { Console.CursorLeft, Console.CursorTop };
            do
            {
                Console.Write($"{descriptionInfo}");
                List<string> PriceString = EnterField(out isEscapeKeyPressed, stringLenght);
                if (isEscapeKeyPressed) return -1;
                string priceString = "";
                foreach (string character in PriceString) priceString += character;
                if (IsTypeOfNumberRequested(typeOfnumber, priceString))
                {
                    dynamic price = GetNumberRequested(typeOfnumber, priceString);
                    if (price < minNumberValue && errorMsg) DisplayErrorMsg($"Value cannot be less than {minNumberValue}!", 2200);
                    if (price > 0 && price <= maxNumberValue) return price;
                    if (price > maxNumberValue && errorMsg) DisplayErrorMsg($"Max value overreached!", 2000);
                }
                else if (errorMsg) DisplayErrorMsg($"Invalid number format or blank!", 2000);
                if (errorMsg == false)
                {
                    while (Console.CursorLeft > initialCursorPos[0])
                    {
                        Console.CursorLeft--;
                        Console.Write(' ');
                        Console.CursorLeft--;
                    }
                }
            } while (true);
        }
        /// <summary>
        /// Returns a string  with lenght between minStringLneght and maxStringLneght.
        /// Input is requested until the criterias in the signature are met. If Escape key is pressed,
        /// the method returns null.
        /// </summary>
        /// <param name="errorMsg">If string lenght not in boundaries, clears console and shows approporiate message if true</param>
        /// <param name="descriptionInfo">Prints a string before number</param>
        /// <returns></returns>
        private string StringInput(int minStringLenght, int maxStringLenght, bool errorMsg = true, string descriptionInfo = "")
        {
            bool isEscapeKeyPressed = false;
            do
            {
                Console.Write($"{descriptionInfo}");
                List<string> fieldName = EnterField(out isEscapeKeyPressed);
                if (isEscapeKeyPressed) return null;
                if (fieldName.Count >= minStringLenght && fieldName.Count <= maxStringLenght)
                {
                    return string.Join("", fieldName);
                }
                if (errorMsg) DisplayErrorMsg($"String too long, too short or blank!", 2200);
            } while (true);
        }

        /// <summary>
        /// Returns numberString in representation type, defined by typeOfnumber.
        /// Throws exceptions if conversation is not possible.
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
        /// Returns a list of user-entered characters. If Escape key is pressed, then isEscapeKeyPressed equals true.
        /// </summary>
        private List<string> EnterField(out bool isEscapeKeyPressed, int stringLenght = 255)
        {

            int index = 0;
            string symbolEntered;
            List<string> fieldName = new List<string>();
            isEscapeKeyPressed = false;
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    isEscapeKeyPressed = true;
                    return null;
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (index >= 1)
                    {
                        Console.CursorLeft--;
                        Console.Write(' ');
                        Console.CursorLeft--;
                        fieldName.RemoveAt(--index);
                    }
                    continue;
                }
                if (!char.IsControl(key.KeyChar))
                {
                    if (index < stringLenght)
                    {
                        symbolEntered = Convert.ToString(key.KeyChar);
                        fieldName.Add(Convert.ToString(symbolEntered));
                        Console.Write(key.KeyChar);
                        index++;
                    }
                    else
                    {
                        string fieldNameInString = "";
                        foreach (var symbol in fieldName) fieldNameInString += symbol;
                        if (IsAnyNumber(fieldNameInString))
                        {
                            Console.CursorLeft -= index;
                            for (int i = 0; i < fieldName.Count; i++)
                            {
                                fieldName[i] = "9";
                                Console.Write("9");
                            }
                        }
                    }
                }
            } while (key.Key != ConsoleKey.Enter);
            return fieldName;
        }
        /// <summary>
        /// Returns true if stringName can be parsed to any number.
        /// </summary>
        private bool IsAnyNumber(string stringName)
        {
            if (IsTypeOfNumberRequested("int", stringName)
                                || IsTypeOfNumberRequested("double", stringName)
                                || IsTypeOfNumberRequested("decimal", stringName))
            {
                return true;
            }
            return false;

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
        internal void PrintAdminPageHeadder()
        {
            Console.Clear();
            Console.WriteLine("+" + GInterface.HorizontalLine('-', '-', 17) + "+");
            Console.WriteLine("|-<Welcome admin>-|");
            Console.WriteLine("+" + GInterface.HorizontalLine('~', '~', 17) + "+");
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
        /// Clears the console and displays text in blue box for a certain amount of time.
        /// </summary>
        /// <param name="infoMessage">Text to be displayed</param>
        /// <param name="timeoutInMiliseconds">The time, in which the message stays open</param>
        private void DisplayInfoMsg(string infoMessage, int timeoutInMiliseconds = 1000)
        {
            Console.Clear();
            Console.WriteLine(GInterface.HorizontalLine('x', 'x', infoMessage.Length));
            Shapes.InformingConsoleColor();
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
            Shapes.WarningConsoleColor();
            Console.Write(errorMessage);
            System.Threading.Thread.Sleep(timeoutInMiliseconds);
            Console.ResetColor();
            Console.CursorLeft = 0;
            GInterface.DeleteRow();
            Console.Clear();
        }

        public AdminPanel()
        {
            controller = new Controller();
            using (StreamReader reader = new StreamReader("adminpwrd.txt"))
            {
                if (File.ReadAllText("adminpwrd.txt") == string.Empty)
                {
                    adminPassword = "admin";
                }
                else adminPassword = reader.ReadToEnd();
            }
            Console.CursorVisible = false;
        }
    }
}
