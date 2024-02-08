using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Rockpaper
{
    class Scoreboard
    {
        public static void displayScoreboard()
        {
            Console.Clear();
            string connectionString = "Data Source=DESKTOP-V3T8DSI\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True;";
            string sqlQuery = "SELECT * FROM Scoreboard ORDER BY Points DESC";
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Scoreboard: \n");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            if(i == 1)
                            {
                                Console.Write(reader.GetName(i) + "\t\t\t");
                            }
                            else
                            {
                                Console.Write(reader.GetName(i) + "\t\t ");
                            }
                        }
                        Console.WriteLine();
                        int rank = 0;
                        while (reader.Read())
                        {
                            rank++;
                            Console.Write($"{rank} ");
                            for(int i = 0;i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetValue(i).ToString().PadRight(15) + " ");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            Console.ReadLine();
        }
    }
}