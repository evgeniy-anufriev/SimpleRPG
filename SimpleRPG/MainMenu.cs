using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace SimpleRPG
{
    public static class MainMenu
    {
     

        public static   void Init()
        {
            Console.WriteLine("Главное меню");
            Console.WriteLine();
            Console.WriteLine("1.Новая Игра");
            Console.WriteLine("2.Загрузить игру");
            Console.WriteLine("3.Настройки");
            Console.WriteLine("4.Выход");
            Console.WriteLine();
            int select = 0;


            do
            {
                try
                {
                    select = int.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Введите число от 1 до 5 !");
                }
            }

            while (select == 0);
            switch (select)
            {

                case 1:
                    MainMenu.NewGame();
                    break;

                case 2:
                    MainMenu.LoadGame();
                    break;
                case 3:
                    MainMenu.Settings();
                    break;
                case 4:
                    MainMenu.Statistic();
                    break;
                case 5:
                    MainMenu.Exit();
                    break;

            }
        }

        public static void NewGame()
        {
            Console.WriteLine("1.Новая Игра");
            Game game = new Game();
            game.InitGame();
        }

        public static void LoadGame()
        {
            Console.WriteLine("2.Загрузить игру");
        }
        public static void Settings()
        {

            Console.WriteLine("3.Настройки");

        }

        public static void Statistic()
        {

            Console.WriteLine("4.Статистика");

        }
        public static void Exit()
        {
            Console.WriteLine("5.Выход");
        }

    }
}
