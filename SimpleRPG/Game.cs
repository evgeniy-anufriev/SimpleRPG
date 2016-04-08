using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;

/* Избавиться от кучи кейсов!!!!*/
/*Продумать классы и динамическое создание в процессе*/

namespace SimpleRPG
{
    public class Game
    {
       

        Hero hero;
        Monster enemy;
        List<Monster> patternMonster = new List<Monster>();
        List<Hero> patternHero = new List<Hero>();

        public void InitGame()
        {
            //Создание монстров героев. 

            patternMonster.Add(new Monster("Орк", "Гуманоид", 4, 5, 3, 0));
            patternMonster.Add(new Monster("Троль", "Гуманоид", 6, 2, 4, 0));
            patternMonster.Add(new Monster("Гоблин", "Гуманоид", 2, 4, 1, 0));
            patternMonster.Add(new Monster("Огр", "Гуманоид", 5, 1, 6, 0));
            patternMonster.Add(new Monster("Паук", "Зверь", 2, 2, 2, 0));
            patternMonster.Add(new Monster("Волк", "Зверь", 2, 3, 2, 0));
            patternMonster.Add(new Monster("Медведь", "Зверь", 4, 0, 6, 0));

            patternHero.Add(new Hero("Рыцарь", "Ближний бой", 5, 3, 7, 0));
            patternHero.Add(new Hero("Воин", "Ближний бой", 7, 5, 3, 0));
            patternHero.Add(new Hero("Маг", "Маг", 1, 2, 2, 10));
            patternHero.Add(new Hero("Лучник", "Дальний бой", 5, 7, 3, 0));
            patternHero.Add(new Hero("Разбойник", "Ближний бой", 4, 9, 2, 0));
            patternHero.Add(new Hero("Варвар", "Ближний бой", 10, 2, 3, 0));

            int select = 0;
            Console.WriteLine("Выберите персонажа\n");

            for (int i = 0; i < patternHero.Count; i++)
            {
                Console.WriteLine(i + 1 + " " + patternHero[i].name);

            }
            select = int.Parse(Console.ReadLine());
            hero = (Hero)patternHero[select - 1].Clone();
            hero.calcSecondaryStats();
            hero.getFullInfo();
            Console.WriteLine("\n Персонаж " + hero.name + " отправился в путешествие!\n");
            Loop();

        }

        public void Loop()
        {
            enemyFound();

        }

        public void enemyFound()
        {
            //Встреча с врагом

            label:
            int level = hero.level;
            int minLevel = hero.level - 2;
            if (minLevel <= 0) minLevel = hero.level;
            level = Util.rand.Next(minLevel, level + 2);
            enemy = (Monster)patternMonster[Util.rand.Next(0, patternMonster.Count - 1)].Clone();
            enemy.levelUp(level);
            
            int select = 0;
            do
            {
                Console.WriteLine("Вы обнаружили врага " + enemy.name + ". Ваши действия: 1.Незаметно напасть. 2.Напасть. 3.Попытаться пройти мимо. 4.Спровоцировать врага. 5.Приглядеться. 6.Отступить.\n");
                select = int.Parse(Console.ReadLine());
                switch (select)
                {
                    case 1:
                        if (hero.dexterity > Util.rand.Next(2 * enemy.dexterity, 4 * enemy.dexterity))
                        {
                            Console.WriteLine("Вам удалось напасть из засады!");
                            enemy.getDamage(hero.makeAttack() * 3);
                        }
                        battle(hero, enemy);
                        break;
                    case 2:
                        battle(hero, enemy);
                        break;
                    case 3:
                        MainMenu.Settings();
                        break;
                    case 4:
                        MainMenu.Statistic();
                        break;
                    case 5:
                        Console.WriteLine("\n Перед вами " + enemy.name + " Уровень: " + enemy.level + " Здоровье: " + enemy.hp + "/" + enemy.hpMax + "\n");
                        break;
                }
            } while (select == 5 || select == 0);
            goto label;
        }
        

        public void battle(Hero hero, Monster enemy)
        {
            //Бой
            int currentRound = 1;
            Console.WriteLine("\n Вы вступили в бой с " + enemy.name);
            do
            {
                Console.WriteLine("\n\n Ход игрока: 1.Атака 2.Защита 3.Попытаться сбежать " + " 4.Покопаться в инвентаре ");
                int select = int.Parse(Console.ReadLine());
                switch (select)
                {
                    case 1:
                        enemy.getDamage(hero.makeAttack()); // Монстр.ПолучитьУрон(Герой.СделатьАтаку)
                        break;
                    case 2:
                        hero.blockStance = true;
                        break;
                    case 3:
                        int maxDexterity = (int)(enemy.dexterity * 1.2f);
                        if (hero.dexterity > Util.rand.Next(enemy.dexterity, maxDexterity)) { Console.WriteLine("Вы еле унесли ноги!"); currentRound = 0; } // Попытка сбежать. Сравниваются ловкости
                        break;
                    case 4:
                        hero.inventory.ShowItems();
                        int selectItem = int.Parse(Console.ReadLine());
                        if (selectItem != 0)
                        {
                            var chr = (Character)hero;
                            hero.inventory.items[selectItem].UseItem(ref chr);          //Передача в метод референсной ссылки на персонажа для изменения его какой-либо характеристики(можно лучше?) 
                            hero.inventory.RemoveItem(hero.inventory.items[selectItem]);
                        }
                         
                        break;
                }
                

                hero.getDamage(enemy.makeAttack()); //После хода игрока - атака монстра. Продумать вариации действий
                if (hero.blockStance == true && hero.parryChance>Util.rand.Next(100))
                {
                    Console.WriteLine("\n" + hero.name + " контратаковал!\n");
                    enemy.getDamage(hero.makeAttack());
                }
                //Окончание хода игрока
                hero.getInfo();
                enemy.getInfo();
                hero.blockStance = false;
                currentRound++;
                if (currentRound == 1) break;  
            }
            while (hero.life == true && enemy.life == true);    //выход из боя если кто то убит
            enemy.inventory.ShowItems();
            hero.inventory.items.AddRange(enemy.inventory.items);//Добавление вещей из монстар герою. Исправить попадание вещей, даже если монст не умер!
            
        }



    }
}
 
