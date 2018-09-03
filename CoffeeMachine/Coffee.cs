using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CoffeeMachine
{
    public class Coffees
    {
        public string CoffeeName { get; set; }
        public double Water { get; set; }
        public double Sugar { get; set; }
        public double Coffee { get; set; }
        public int Price { get; set; }
        public int CoffeeNumber { get; set; }

        public static Store Store { get; set; }

        public static Coffees GetCoffeeByNumber(int coffeeNumber)
        {
            string connectionString = @"Data Source=MARIAM-PC\SQLEXPRESS;Initial Catalog=CoffeeMachine;Integrated Security=True;";
            Coffees coffee = new Coffees();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlCommand = "Select * From Coffees Where CoffeeNumber=@CoffeeNumber";
                SqlCommand cmd = new SqlCommand(sqlCommand, connection);
                cmd.Parameters.AddWithValue("@CoffeeNumber", coffeeNumber);
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        coffee.CoffeeName = reader["CoffeeName"].ToString();
                        coffee.Water = Double.Parse(reader["Water"].ToString());
                        coffee.Sugar = Double.Parse(reader["Sugar"].ToString());
                        coffee.Coffee = Double.Parse(reader["Coffee"].ToString());
                        coffee.Price = Int32.Parse(reader["Price"].ToString());
                        coffee.CoffeeNumber = Int32.Parse(reader["CoffeeNumber"].ToString());
                    }

                    connection.Close();
                }
            } 

            return coffee;
        }

        public static List<Coffees> GetAllCoffees()
        {
            string connectionString = @"Data Source=MARIAM-PC\SQLEXPRESS;Initial Catalog=CoffeeMachine;Integrated Security=True;";
            List<Coffees> coffees = new List<Coffees>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlCommand = "Select * From Coffees";
                SqlCommand cmd = new SqlCommand(sqlCommand, connection);
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Coffees coffee = new Coffees();
                        coffee.CoffeeName = reader["CoffeeName"].ToString();
                        coffee.Water = Double.Parse(reader["Water"].ToString());
                        coffee.Sugar = Double.Parse(reader["Sugar"].ToString());
                        coffee.Coffee = Double.Parse(reader["Coffee"].ToString());
                        coffee.Price = Int32.Parse(reader["Price"].ToString());
                        coffee.CoffeeNumber = Int32.Parse(reader["CoffeeNumber"].ToString());

                        coffees.Add(coffee);
                    }

                    connection.Close();
                }
            }

            return coffees;
        }

        public static bool CheckStoreIsEnough(Coffees coffee, out string message)
        {
            message = "";
            Coffees.Store = Store.LoadStoreData();

            if(coffee.Coffee > Coffees.Store.Coffee)
            {
                message = "Sorry, there is no enough coffee for your order.";
                return false;
            }

            if (coffee.Sugar > Coffees.Store.Sugar)
            {
                message = "Sorry, there is no enough sugar for your order.";
                return false;
            }

            if (coffee.Water > Coffees.Store.Water)
            {
                message = "Sorry, there is no enough water for your order.";
                return false;
            }

            return true;
        }

        public static bool UpdateStore(Coffees coffee)
        {
            return Store.UpdateStoreData(coffee);
        }
    }
}
