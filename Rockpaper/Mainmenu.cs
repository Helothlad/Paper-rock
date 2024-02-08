using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rockpaper
{
    class Mainmenu
    {
        

        public static void MainMenu()
        {
            Console.WriteLine("Welcome to Rock Paper Scissors: \n");
            Console.WriteLine("1. Play: ");
            Console.WriteLine("2. Scoreboard: ");
            Console.WriteLine("3. Exit");
            Console.WriteLine("Insert the correct number into the console according to your prefrence.");
            int input = int.Parse(Console.ReadLine());

            handleMainMenu(input);

        }
        private static void handleMainMenu(int number)
        {
            
            if (number == 1)
            {
                
                Game.startGame();
            }
            else if (number == 2)
            {

                Scoreboard.displayScoreboard();
            }
            else
            {
                Console.WriteLine("Rossz input");
            }
        }
    }
}
