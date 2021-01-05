using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Potion : Item, IInteractable
    {
        public Die[] HealingDice {get; protected set;}
        private Map _map {get; set;}
        public Potion(string name, Die[] healingDice, Map map)
        {
            Name = name + " Potion";
            HealingDice = healingDice;
            Weight = HealingDice.Sum(hD => hD.NumSides) * .25;
            Value = HealingDice.Sum(hD => hD.NumSides) * 3;
            Description = $"It bubbles with an effervescent red liquid. Should heal roughly {HealingDice.DiceToString()} hp.";
            _map = map;
        }

        public void Activate(Player player)
        {
            int quantity = HealingDice.Sum(hD => hD.Roll());
            player.ChangeHp(quantity);
            Console.WriteLine($"{player.Name} drank the {Name} and healed {quantity} hp!");
            if (player.Inventory.Contains(this))
            {
                player.RemoveItemFromInventory(this);
            }
            else if (_map.Assets.Contains(this))
            {
                _map.RemoveAsset(this);
            }
            player.RemoveFromMemory(this);
        }
    }
}