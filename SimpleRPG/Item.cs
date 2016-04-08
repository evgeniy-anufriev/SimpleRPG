using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Добавить тип итема enum(качество предмета).Влияет на цену и на хар-ки для оружия и брони.*/
/* Добавить классы оружия, брони, и т.д.*/
/* Подумать над инкапсуляцией*/

namespace SimpleRPG
{

    abstract public class Item : ICloneable
    {
        public Item() { }
        public Item(int level) { }
        public virtual string Type { get; set; }
        public virtual string Name { get; set; }
        public virtual    int Level { get; set; }
        public virtual    int Cost  { get; set; }



        public virtual void UseItem()
        {
            Console.WriteLine("Этот предмент невозможно использовать!");
        }


        public virtual void UseItem(ref Character chr)
        {
        }

        public Object Clone() // Копирование объектов 
        {
            return base.MemberwiseClone();
        }
    }
   

        abstract public class Pot:Item
    {       
        public Pot(int level): base(level)
        {
            Level = level/7+1;                      //Уровень завит от уровню того, у кого создался. Добавить немного рандома 
            Cost= (int)Math.Pow(Level * 5, 2) + 50; //Цена зависит от уровня предмета
        }
        public override int Level { get; set; }

        public override int Cost  { get; set; }  
    }


    public class HealthPot : Pot
    {
        public HealthPot(int level) : base(level)
        {
        }

        public override string Name { get { return "Здоровье"; } set { } }
        public override void UseItem(ref Character chr)
        {
            chr.hp += 20;
            if (chr.hp >= chr.hpMax) chr.hp = chr.hpMax;
        }
    }

    public class StaminaPot : Pot
    {
        public StaminaPot(int level) : base(level)
        {
        }

        public override String Name { get { return "Выносливость"; } set { } }
        public override void UseItem(ref Character chr)
        {
            chr.stamina += 20;
            if (chr.stamina > chr.staminaMax) chr.stamina = chr.staminaMax;
             
        }

    }


}
