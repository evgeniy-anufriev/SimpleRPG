using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

/* Добавить получение опыта для Hero*/
/* Создать классы(интерфейсы) персонажий(Воин,Разбойник и т.д.)*/
/* Создать интерфейс Босса*/
/* Учитывать в бою оружие,броню*/
/* Добавить магию*/
/* Продолжать настраивать баланс*/


namespace SimpleRPG
{
   public class Character: ICloneable
    {
        
        
        public String name { get; protected set; }
        protected String charClass { get; set; }

        // Primary Stats
        protected int base_strength; 
        protected int base_dexterity;
        protected int base_endurance;
        protected int base_intelect;
        protected int strength { get; set; }
        public int dexterity { get; protected set; }
        protected int endurance { get; set; }
        protected int intelect { get; set; }
        public int hpMax { get; protected set; }

        public int level { get; protected set; }

        // Secondary stats
        public int attack { get; protected set; }
        public int hp { get; set; }
        protected int defense;
        public  int stamina { get; set; }
        public int staminaMax { get; protected set; }
        protected int magic;
        protected int magicResistance;
        protected float critChance;
        protected float critMultiplier;
        protected int avoidChance;
        public int parryChance { get; protected set; }
        public bool blockStance { get; set; }
        public  bool life { get; protected set; }
   
       // protected Inventory myInv;

        public Character(String name, String charClass, int strength,int dexterity,int endurance, int intelect)
        {
            this.name = name;
            this.charClass = charClass;
            this.strength = this.base_strength = strength;
            this.dexterity= this.base_dexterity = dexterity;
            this.endurance = this.base_endurance= endurance;
            this.intelect = this.base_intelect= intelect;
             

        }

        public void calcSecondaryStats()
        {
            life = true;
            level = 1;
            attack = strength * 2;
            hpMax = endurance * 20;
            defense=(int)((2*strength + dexterity*0.5f) / 2.0f);
            critChance = dexterity+(100f/(100f+dexterity));
            critMultiplier = 1.2f + (dexterity)/20.0f;
            avoidChance= (int)(dexterity/2) + (int)(dexterity / 5.0f);
            parryChance = (int)(strength/2) + (int)(strength / 5.0f);
            magic = intelect * 20;
            staminaMax = endurance * 20+strength*5;
            magicResistance = (int)(intelect / 2.0f);
            hp = hpMax;
            stamina = staminaMax;

        }

        public int makeAttack()
        {
  
            int minAttack = (int)(attack * 0.8f);
            int deltaAttack = (int)(attack *0.4f* (stamina * (1.0f / staminaMax)));
            int currentAttack = minAttack + Util.rand.Next(deltaAttack);
            stamina -= Util.rand.Next(strength * 2,strength*3);
            if (stamina < 0) stamina = 0;
            if (critChance > Util.rand.NextDouble() * 100)
            {
                currentAttack *= (int)critMultiplier;
                Console.WriteLine("\n " + this.name + " провел критическую атаку на : " + currentAttack +" ед. урона");
            }
            else Console.WriteLine("\n " + this.name + " провел обычную атаку на: " + currentAttack+ " ед. урона");
            
            return currentAttack;
        }

        public void getDamage(int damage)
        {
            if(avoidChance>Util.rand.Next(100)) Console.WriteLine("\n"+ this.name+" уклонился от удара!");
            else
            {
                damage -= Util.rand.Next(defense);
                if (blockStance)
                {
                    damage -=Util.rand.Next(defense);
                    Console.WriteLine("\n" + this.name + " заблокировал часть урона.");

                }

                if (damage < 0)
                {
                    damage = 0;
                    Console.WriteLine("\n" + this.name + " не получил урона.");
                }
                Console.WriteLine("\n" + this.name + " получил "+damage+" ед.урона");
                hp -= damage;
                if (hp<=0)
                {
                    Console.WriteLine("\n" + this.name + " погиб!");
                    life = false;
                }

              
            }
                 
           
        }

        public Object Clone() // Копирование объектов 
        {
 
              return base.MemberwiseClone();
    
            }
        public void getFullInfo()
        {
            Console.WriteLine("Имя: " + name + " Класс: " + charClass + " Уровень " + level);
            Console.WriteLine("Макс.здоровья "+hpMax+ " Выносливость "+ stamina+ " Мана "+magic);
            Console.WriteLine("Шанс крита " + critChance + " Модификатор крита " + critMultiplier);
            Console.WriteLine("Защита " + defense + " Шанс уклонения " + avoidChance + " Сопротивление магии " + magicResistance);
        }

         public void getInfo()
        {
            Console.WriteLine("Имя: " + name + " Здоровье: " + hp + "/" + hpMax + " Выносливость: "+stamina+ "/" + staminaMax + " Уровень: " + level);
        }
    }

    
    public class Monster : Character
    {
        public Inventory inventory = new Inventory(10);

        public Monster(String name, String charClass, int strength,int dexterity,int endurance, int intelect):base(name,charClass,strength,dexterity,endurance,intelect)
        {
            RandItems();
        }
        public void levelUp(int level)
        {
             
             strength = base_strength + (level - 1) *(base_strength/2);
             endurance = base_endurance + (level - 1) * (base_endurance / 2);
             dexterity = base_dexterity + (level - 1) * (base_dexterity / 2);
             intelect = base_intelect + (level - 1) * (base_intelect / 2);
             calcSecondaryStats();
             this.level = level;
          
        }

        public void RandItems()
        {
            Type type = typeof(Item);
            var classList = Assembly.GetExecutingAssembly().GetTypes().ToList().Where(t => type.IsAssignableFrom(t) && !t.IsAbstract).ToList();
            for (int i = 0; i < Util.rand.Next(10); i++) //Придумать проверку на выпадение 
                foreach (var item in classList)
                    if (20 > Util.rand.Next(100))
                        inventory.items.Add((Item)Activator.CreateInstance(item,level)); //Сделать динамический шанс выпадения

        }

    }

    public class Hero : Character
    {
        public Inventory inventory;
        public Hero(String name, String charClass, int strength, int dexterity, int endurance, int intelect) : base(name, charClass, strength, dexterity, endurance, intelect)
        {
            inventory = new Inventory();
        }

    }
}


 
