﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Threading;
namespace Fit4Life.Models
{
    internal class DataManagement
    {
        public int CurrentOptionIndex { get; set; } = -1;
        internal MySqlConnection connection;
        internal /*return type*/void FetchDataBasedOn(int optionIndexSelected)
        {
            CurrentOptionIndex = optionIndexSelected;
            if (optionIndexSelected == -1)//If no option was chosen
            {

            }
            else
            {
                switch (optionIndexSelected)
                {
                    //Spacebar to add to cart.
                    case 0://Supplements
                           //FORMAT
                           //Name - Brand - Weight - Price  (Press D for description)
                        break;
                    case 1://Drinks

                        break;
                    case 2://Equipment

                        break;
                    case 3://Fitness world news

                        break;
                    case 4://About

                        break;
                    default:
                        break;
                }
            }
            //return sqlCommand
        }

        internal void EstablishDataBaseConnection(string databaseName, string userId, string userPassword, string server = "localhost")
        {
            string connectionString =
                $"server = {server}; database = {databaseName}; uid = {userId}; pwd = {userPassword};";
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }
    }
}
