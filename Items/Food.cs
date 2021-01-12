using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Food : Item, IInteractable, IDegradable
    {
        public int Duration {get; protected set;}
        private bool _isSpoiled;
        public int HealingAmount {get; protected set;}
        private string _baseDescription;
        public override string Description => _baseDescription + $" It should be good for {Duration} more steps and should heal roughly {HealingAmount} hp.";
        public Food(string name, int healingAmount, int duration, ItemRarity rarity, string description)
        {
            Name = name;
            HealingAmount = healingAmount;
            Duration = duration;
            Rarity = rarity;
            Weight = HealingAmount * .2;
            Value = (HealingAmount * Duration) / 100;
            _baseDescription = description;
        }

        public Food(Food foodToClone)
        {
            Name = foodToClone.Name;
            HealingAmount = foodToClone.HealingAmount;
            Duration = foodToClone.Duration;
            Rarity = foodToClone.Rarity;
            Weight = foodToClone.Weight;
            Value = foodToClone.Value;
            _baseDescription = foodToClone._baseDescription;
            Location = foodToClone.Location;
        }

        public override Item GetClone()
        {
            return new Food(this);
        }

        public void Activate(Player player)
        {
            if (!_isSpoiled)
            {
                player.ChangeHp(HealingAmount);
                Console.WriteLine($"{player.Name} consumed the {Name} and healed {HealingAmount} hp.");
                IsDestroyed = true;
            }
            else
            {
                Console.WriteLine($"That {Name} looks off... I wouldn't eat it.");
            }
        }

        public void DecrementDuration()
        {
            if (Duration > 0) Duration--;
            if (Duration <= 0 && !_isSpoiled)
            {
                _isSpoiled = true;
                Name += " (spoiled)";
                Value = 0;
            }
        }
    }
}