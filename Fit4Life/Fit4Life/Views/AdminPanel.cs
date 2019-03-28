using Fit4Life.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit4Life.Views
{
    internal class AdminPanel
    {
        private static string actions;
        int actionIndex = -1;
        private List<string> actionList = GInterface.GetStringListByString(actions, true);
        private string adminPassword = "admin";


        internal bool IsAccessGained()
        {
            List<string> passwordEntered = null;
            string symbolEntered;
            ConsoleKeyInfo key;
            int index;
            do
            {
                if (passwordEntered != null)
                {
                    DisplayWrongPassMsg();
                }
                passwordEntered = new List<string>();
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
            return true;
        }
        private void DisplayWrongPassMsg()
        {
            Console.Clear();
            Console.WriteLine(GInterface.HorizontalLine('x', 15));
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Wrong password!");
            System.Threading.Thread.Sleep(600);
            Console.ResetColor();
            Console.CursorLeft = 0;
            Console.WriteLine(GInterface.HorizontalLine(' ', 20));
            Console.Clear();
        }

        internal void SelectAction()
        {
            Console.WriteLine(actionList[0]);
            System.Threading.Thread.Sleep(1000);
        }

        public AdminPanel()
        {
            actions = "Add new supplement;Add new Drink;Add new Equipment;" +
            "Restock supplement;Restock drink;Restock equipment";
        }
    }
}
