using System;
using System.Linq;
using System.Collections.Generic;
using static ProceduralDungeon.ExtensionsAndHelpers;

namespace ProceduralDungeon
{
    public class Chest : IMappable, INameable, IDescribable, IContainer, IInteractable, IDestroyable
    {
        public string Name {get; private set;}
        public List<Item> Inventory {get; private set;} = new List<Item>();
        public int Gold {get; set;}
        public double MaxCarryWeight {get; private set;} = 500;
        public double CurrentCarryWeight => Inventory.Sum(i => i.Weight);
        public string Description {get; private set;}
        public Point Location {get; set;}
        public char Symbol {get; private set;} = Symbols.Chest;
        public int TotalValue => Inventory.Sum(i => i.Value + Gold);
        public bool IsDestroyed {get; set;} = false;
        public Chest(string name, int averageGold, string description, List<Item> validItems)
        {
            Name = name;
            Description = description;
            int numItems = Dice.D6.Roll() + 1;
            int hasGold = Dice.Coin.RollBaseZero();
            Gold = hasGold > 0 ? (int)RandomDouble(averageGold - (averageGold*.25), averageGold + (averageGold*.25)) : 0;
            while (Inventory.Count < numItems) 
            {
                if (Inventory.Count < numItems / 2) 
                {
                    Inventory.Add(validItems.Where(i => i is IEquippable || i is IInteractable).RandomElement().GetClone());
                }
                else
                {
                    Inventory.Add(validItems.Where(i => !(i is IEquippable || i is IInteractable)).RandomElement().GetClone());
                }
            }
        }

        public bool AddItemToInventory(Item itemToAdd, bool cloneItem = false)
        {
            var actualItem = cloneItem ? itemToAdd.GetClone() : itemToAdd;
            if (CurrentCarryWeight + actualItem.Weight <= MaxCarryWeight)
            {
                Inventory.Add(actualItem);
                itemToAdd.Location = null;
                return true;
            }
            else
            {
                System.Console.WriteLine($"The {itemToAdd.Name} cannot fit inside the {Name}.");
                return false;
            }
        }

        public bool RemoveItemFromInventory(Item itemToRemove)
        {
            if (Inventory.Contains(itemToRemove))
            {
                Inventory.Remove(itemToRemove);
                return true;
            }
            else
            {
                System.Console.WriteLine($"{itemToRemove.Name} could not be found in the {Name}'s inventory.");
                return false;
            }
        }

        public string GetCarryWeightString()
        {
                return $"{Math.Round(CurrentCarryWeight, 1)}/{MaxCarryWeight} lbs";
        }

        public void Activate(Player player)
        {
            System.Console.WriteLine($"{player.Name} searches the {Name}.");
            WaitForInput();
            Console.Clear();
            (this as IContainer).OpenTradeMenu(player);
        }
    }
}