using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


/* Добавить расчет золота Gold*/
/* Подумать над инкапсуляцией*/

namespace SimpleRPG
{
    public class Inventory
    {
        
        public Inventory() { Size = 50; }               //Сделать динамическим
        public Inventory(int Size) { this.Size = Size; }
        public int Gold { get { return Gold; } set { Gold = value; } }
        public List<Item> items = new List<Item>();
        public int Size { get; protected set; }



        public void RemoveItem(Item item)
        {
            items.Remove(item);
            Console.WriteLine("Предмет {0} был удален из инвентаря",item);
        }

        public void AddItem(Item item)
        {
            items.Add(item);
            Console.WriteLine("Предмет {0} был добавлен в инвентарь {1}", item.Name,item.Level);
        }
        public void ShowItems()
        {
            if (items.Count == 0) Console.WriteLine("Инвентарь пуст! Нажмите 0 для выхода.");
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine(i+". "+ items[i].Name + " : " + items[i].Type+ " : "+items[i].Level); //Изменить информацию для показа
                }
            }
        }



    }


}


