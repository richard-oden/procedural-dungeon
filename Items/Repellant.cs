using System;
using System.Collections.Generic;  
using System.Linq;

namespace ProceduralDungeon
{
    public class Repellant : Item, IInteractable, IDegradable
    {
        public CreatureCategory TargetCreatureCategory {get; protected set;}
        public bool IsActive {get; protected set;} = false;
        public int Duration {get; protected set;}
        public override int Value => Duration * 2;
        public override double Weight => Duration *.05;
        public override ItemRarity Rarity {get; protected set;} = ItemRarity.Uncommon;
        public Repellant(string name, CreatureCategory targetCreatureCategory, int duration, string description)
        {
            Name = name;
            TargetCreatureCategory = targetCreatureCategory;
            Duration = duration;
            Description = description;
        }

        public Repellant(Repellant repellantToClone)
        {
            Name = repellantToClone.Name;
            TargetCreatureCategory = repellantToClone.TargetCreatureCategory;
            Duration = repellantToClone.Duration;
            Description = repellantToClone.Description;
            IsActive = repellantToClone.IsActive;
            Location = repellantToClone.Location;
        }

        public override Item GetClone()
        {
            return new Repellant(this);
        }

        public void DecrementDuration()
        {
            Duration--;
            if (Duration <= 0) 
            {
                Console.WriteLine($"The {Name}'s effect has ended. {TargetCreatureCategory.ToString()}s are no longer turned.");
                IsActive = false;
                IsDestroyed = true;
            }
        }

        public void Activate(Player player)
        {
            if (!IsActive)
            {
                Console.WriteLine($"{player.Name} activates the {Name}. For the next {Duration} steps, all {TargetCreatureCategory.ToString()}s are turned.");
                IsActive = true;
            }
            else
            {
                var endRepellantInput = ExtensionsAndHelpers.PromptKey($"The {Name} is already active. Do you wish to end the effect? (Y/N).");
                if (endRepellantInput == ConsoleKey.Y)
                {
                    Console.WriteLine($"{player.Name} ends the {Name}'s effect early. It still has some use left.");
                    IsActive = false;
                }
                else
                {
                    Console.WriteLine("The effect is not ended.");
                }
            }
        }
    }
}