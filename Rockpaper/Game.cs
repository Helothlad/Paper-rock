using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rockpaper
{
    class Game
    {
        private static int points;
        private static Random rng;
        private static char[] array;
        
        public static void startGame(string name)
        {
            Console.Clear();
            
            DateTime startTime = DateTime.Now;
            points = 0;
            int score = 0;
            while (score != -1)
            {
                Console.WriteLine("Points: " + points + "\n");

                char computerRandomChoice = RandomChoice();
                Console.WriteLine("Choose from paper, rock or scissors.(p-paper, r-rock, s-scissors)");
               
                string userInput = Console.ReadLine();
                char input = userInput[0];
                score = Decide(computerRandomChoice, input);
                if(score == 1)
                {
                    points++;
                }
                Thread.Sleep(1500);
                Console.Clear();
            }
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;
             
            Console.WriteLine($"You colleted {points} points under {duration.Seconds} seconds");
            AddScore(name, points, duration);
            BackToMainMenu.BackToMainMenuFunction();
        }
        private static char RandomChoice()
        {
            rng = new Random();
            array = new char[] { 'p', 'r', 's' };
            int idx = rng.Next(0, 2);
            return array[idx];
        }
        private static int Decide(char computer, char user)
        {
            int score = 0;
            if(computer == user)
            {
                Console.WriteLine("Tie");
                return score;
            }
            else if(computer == 'p' && user == 'r')
            {
                Console.WriteLine("You lost");
                score--;
                return score;
            }else if(computer == 'r' && user == 'p')
            {
                Console.WriteLine("You win");
                score++;
                return score;
            }
            else if(computer == 's' && user == 'p')
            {
                Console.WriteLine("You lost");
                score--;
                return score;
            }else if(computer == 'p' && user == 's')
            {
                Console.WriteLine("You win");
                score++;
                return score;
            }else if(computer == 'r' && user == 's')
            {
                Console.WriteLine("You lost");
                score--;
                return score;
            }
            else if(computer == 's' && user == 'r')
            {
                Console.WriteLine("You win");
                score++;
                return score;
            }
            else
            {
                Console.WriteLine("Something went wrong");
                return score;
            }
            
        }
        private static void AddScore(string name, int points, TimeSpan duration)
        {
            string connectionString = "Data Source=MYPC\\SQLEXPRESS01;Initial Catalog=ChrisDB;Integrated Security=True;";
            string sqlQuery = "INSERT INTO Scoreboard (Name, TimeColumn, Points) VALUES (@Name, @Time, @Points)";

            double totalSeconds = Math.Round(duration.TotalSeconds);
            TimeSpan roundedDuration = TimeSpan.FromSeconds(totalSeconds);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using(SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Time", roundedDuration);
                    command.Parameters.AddWithValue("@Points", points);

                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("You have been placed onto the scoreboard");
            Thread.Sleep(1000);
        }
    }
}
