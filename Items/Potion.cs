using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Potion : Item, IInteractable
    {
        public Die[] HealingDice {get; protected set;}
        public Potion(string name, Die[] healingDice)
        {
            Name = name + " Potion";
            HealingDice = healingDice;
            Weight = HealingDice.Sum(hD => hD.NumSides) * .25;
            Value = HealingDice.Sum(hD => hD.NumSides) * 3;
            Description = $"It bubbles with an effervescent red liquid. Should heal roughly {HealingDice.DiceToString()} hp.";
        }

        public Potion(Potion potionToClone)
        {
            Name = potionToClone.Name;
            HealingDice = potionToClone.HealingDice;
            Weight = potionToClone.Weight;
            Value = potionToClone.Value;
            Description = potionToClone.Description;
            Location = potionToClone.Location;
        }

        public override Item GetClone()
        {
            return new Potion(this);
        }

        public void Activate(Player player)
        {
            int quantity = HealingDice.Sum(hD => hD.Roll());
            player.ChangeHp(quantity);
            Console.WriteLine($"{player.Name} drank the {Name} and healed {quantity} hp!");
            IsDestroyed = true;
        }
    }
}