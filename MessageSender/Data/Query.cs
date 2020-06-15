using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessageSender
{
    public class Query
    {
        //Initialize connection
        private DBConnection connection { get; set; }

        //Sql to database
        private string commandText { get; set; }

        //Catch sql result
        private SqlDataReader reader { get; set; }

        //List of people with parameters: name,surname,phonenumber
        public List<Person> GetEmployeeData()
        {
            connection = new DBConnection();
            List<Person> people = new List<Person>();

            commandText = "SELECT Name, Surname, PhoneNumber, IDemployee FROM Employees";

            reader = connection.Select(commandText);

            while (reader.Read())
            {
                people.Add(new Person
                {
                    Name = String.Concat(reader["Name"], " ", reader["Surname"]),
                    Idu = Convert.ToInt32(reader["IDemployee"]),
                    PhoneNumber = reader["PhoneNumber"].ToString()
                });
            }
            return people;
        }

        //Return signed person id
        public int GetIdEmployee(string login, string password)
        {
            int idu = 0;

            connection = new DBConnection();

            commandText = $"SELECT IDemployee FROM Users WHERE Login='{login}' AND Password='{password}';";

            reader = connection.Select(commandText);

            while (reader.Read())
            {
                idu = Convert.ToInt32(reader["IDemployee"]);
                break;
            }
            return idu;
        }

        public void AddToSendHistory(int FromIdu, int ToIdu, DateTime dateTime)
        {
            connection = new DBConnection();
            commandText = "INSERT INTO Messages(FromIdu,ToIdu,SendDate) " +
                "VALUES(@FromIdu,@ToIdu,@dateTime)";

            connection.InsertSendHistory(commandText, FromIdu, ToIdu, dateTime);
        }
    }
}
