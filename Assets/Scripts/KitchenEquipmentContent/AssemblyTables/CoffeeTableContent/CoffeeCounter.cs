using System.Collections.Generic;
using UnityEngine;

namespace KitchenEquipmentContent.AssemblyTables.CoffeeTableContent
{
    public class CoffeeCounter : MonoBehaviour
    {
        private List<Item> _coffeeItems = new List<Item>();

        public void AddCoffee(Item item)
        {
            _coffeeItems.Add(item);
        }

        public void RemoveCoffee(Item item)
        {
            _coffeeItems.Remove(item);
        }
    }
}