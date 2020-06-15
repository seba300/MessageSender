using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
            commandText = "SELECT Name, Surname, PhoneNumber FROM Employees";

            reader = connection.GetResult(commandText);

            while(reader.Read())
            {
                people.Add(new Person
                {
                    Name = String.Concat(reader["Name"], " ", reader["Surname"]),
                    PhoneNumber = reader["PhoneNumber"].ToString()
                }) ;
            }
            return people;
        }
    }
}
