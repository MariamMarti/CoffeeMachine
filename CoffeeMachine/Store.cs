using System;
using System.Data.SqlClient;

namespace CoffeeMachine
{
    public class Store
    {
        public double Water { get; set; }
        public double Sugar { get; set; }
        public double Coffee { get; set; }

        public static Store LoadStoreData()
        {
            Store store = new Store();

            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                string sqlCommand = "Select * From Store";
                SqlCommand cmd = new SqlCommand(sqlCommand, connection);
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        store.Water = Double.Parse(reader["Water"].ToString());
                        store.Sugar = Double.Parse(reader["Sugar"].ToString());
                        store.Coffee = Double.Parse(reader["Coffee"].ToString());
                    }

                    connection.Close();
                }
            }

            return store;
        }

        public bool UpdateStoreData(Coffees coffee)
        {
            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                string sqlCommand = "Update Store SET Water=@Water, Coffee=@Coffee, Sugar=@Sugar";
                SqlCommand cmd = new SqlCommand(sqlCommand, connection);
                cmd.Parameters.AddWithValue("@Water", this.Water - coffee.Water);
                cmd.Parameters.AddWithValue("@Coffee", this.Coffee - coffee.Coffee);
                cmd.Parameters.AddWithValue("@Sugar", this.Sugar - coffee.Sugar);
                connection.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
