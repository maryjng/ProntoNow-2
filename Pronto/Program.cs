using System;
using System.Data;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=localhost;Port=3307;Database=Pronto;User=root;Password=8ncAdorpnz;";

        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connection Successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection Failed: {ex.Message}");
            }
        }
    }
}