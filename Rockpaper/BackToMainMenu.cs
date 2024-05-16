using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rockpaper
{
    class BackToMainMenu
    {
        public static void BackToMainMenuFunction()
        {
            int number = 0;
            bool given = false;
            while (!given)
            {
                Console.WriteLine("Type 1 if you wish to go back to the main menu");
                number = int.Parse(Console.ReadLine());
                given = InputHandleing(number);
            }
            Mainmenu.MainMenu();
        }

        private static bool InputHandleing(int number)
        {
            if (number == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
