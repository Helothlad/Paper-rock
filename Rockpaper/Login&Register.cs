using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rockpaper
{
    class Login_Register
    {
        private static int registerCount;
        private static int loginCount;
        public Login_Register()
        {
            registerCount = 0;
            loginCount = 0;
        }

        public static void UserAction()
        {
            Console.Clear();
            Console.WriteLine("1: Login\n" +
                              "2: Register\n" +
                              "3: Exit\n\n" +
                              "Choose from option(1-3)");
            ActionHandler();
        }
        private static void ActionHandler()
        {
            int input = 0;
            while(input != 1 && input != 2 & input != 3)
            {
                input = int.Parse(Console.ReadLine());
                if(input == 1)
                {
                    Login();
                }
                else if(input == 2)
                {
                    Register();
                }
                else if (input == 3)
                {
                    BackToMainMenu.BackToMainMenuFunction();
                }
                else
                {
                    Console.WriteLine("Bad input");
                }
            }
        }
        private static void Login()
        {
            if(loginCount == 0)
            {
                Console.Clear();
            }
            

            Console.WriteLine("Please enter your username:");
            string userName = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string password = ReadPassword();

            string passwordHash = ComputeHash(password);

            string storedPasswordHash = GetStoredPasswordHash(userName);

            if(storedPasswordHash != passwordHash)
            {
                Console.WriteLine("Wrong userName or password");
                loginCount++;
                Login();
            }
            else
            {
                Console.WriteLine("Login successful!");
                Thread.Sleep(1500);
                Console.Clear();
                Countdown(3);
                Game.startGame(userName);
            }


        }
        private static void Countdown(int seconds)
        {
            for (int i = seconds; i >= 0; i--)
            {
                Console.WriteLine("\rThe game will start in {0}", i);
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
        private static string GetStoredPasswordHash(string userName)
        {
            string connectionString = "Data Source=MYPC\\SQLEXPRESS01;Initial Catalog=ChrisDB;Integrated Security=True;";
            string sqlQuery = $"SELECT PasswordHash FROM Users WHERE Username = @UserName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        private static void Register()
        {
            if(registerCount == 0)
            {
                Console.Clear();
            }
            
            
            Console.WriteLine("Provide a username: \n");
            string userName = Console.ReadLine();
            Console.WriteLine("Provide a password: ");
            string password = ReadPassword();

            string hashedPassword = ComputeHash(password);
            string connectionString = "Data Source=MYPC\\SQLEXPRESS01;Initial Catalog=ChrisDB;Integrated Security=True;";
            string sqlQuery = $"INSERT INTO Users (Username, PasswordHash) VALUES (@UserName, @PasswordHash)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string checkQuery = $"SELECT COUNT(*) FROM Users WHERE Username = @UserName";
                using(SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@UserName", userName);
                    int userCount = (int)checkCommand.ExecuteScalar();

                    if (userCount > 0)
                    {
                        Console.WriteLine("This username is already taken, Please try again with a new username");
                        registerCount++;
                        
                        Register();
                        return;
                    }
                }
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName); // Corrected here
                    command.Parameters.AddWithValue("@PasswordHash", hashedPassword); // Corrected here
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Registration successful!");
            Login();
        }

        private static string ComputeHash(string input)
        {
            // This is a simple example. Use a secure method to hash and salt the password in a real-world application.
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // Remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // Replace it with space
                        Console.Write("\b \b");
                    }
                }
                info = Console.ReadKey(true);
            }
            // Add a new line because user pressed Enter at the end of their password
            Console.WriteLine();
            return password;
        }

    }
}
