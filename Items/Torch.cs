using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public class Torch : Item, IEquippable, IDegradable, IInteractable
    {
        public EquipmentSlot Slot {get; protected set;}
        public int Range {get; protected set;}
        public int Duration {get; protected set;}
        public bool IsActive {get; protected set;} = false;
        private string _baseDescription;
        public override string Description => _baseDescription + $" Increases your visible range by {Range*5} feet and should last {Duration} more steps.";

        public Torch(string name, int range, int duration, ItemRarity rarity, EquipmentSlot slot, string description)
        {
            Name = name;
            Rarity = rarity;
            _baseDescription = description;
            Slot = slot;
            Range = range;
            Duration = duration;
            Weight = Duration / 100.0;
            Value = (Range * Duration) / 50;
        }

        public Torch(Torch torchToClone)
        {
            Name = torchToClone.Name;
            Weight = torchToClone.Weight;
            Value = torchToClone.Value;
            _baseDescription = torchToClone._baseDescription;
            Rarity = torchToClone.Rarity;
            Location = torchToClone.Location;
            Slot = torchToClone.Slot;
            Range = torchToClone.Range;
            Duration = torchToClone.Duration;
        }

        public override Item GetClone()
        {
            return new Torch(this);
        }

        public void DecrementDuration()
        {
            if (IsActive) Duration--;
            if (Duration <= 0) 
            {
                Console.WriteLine($"The {Name}'s has burned out.");
                IsActive = false;
                IsDestroyed = true;
            }
        }

        public void Activate(Player player)
        {
            if (!IsActive)
            {
                if (player.EquippedItems.Contains(this))
                {
                    Console.WriteLine($"{player.Name} ignites the {Name}.");
                    IsActive = true;
                }
                else
                {
                    Console.WriteLine($"The {Name} must be equipped before use.");
                }
            }
            else
            {
                Console.WriteLine($"{player.Name} extinguishes the {Name}. It can be ignited again later.");
                IsActive = false;
            }
        }
    }
}