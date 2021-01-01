using System.Linq;
using static ProceduralDungeon.Dice;
using static ProceduralDungeon.ItemsRepository;

namespace ProceduralDungeon
{
    public static class NpcsRepository
    {
        public static readonly Npc[] All = new Npc[]
        {
            new Npc("Giant Rat",        001, Difficulty.VeryEasy,   hp: 6,
                inventory: Junk.RandomSample(D3.RollBaseZero()), gold: D10.RollBaseZero(),
                baseDescription: "It's frothing at the mouth and is missing patches of fur here and there."),
            new Npc("Cave Centipede",   002, Difficulty.VeryEasy,   hp: 4,  ac: 10, dr: 1,
                inventory: Junk.RandomSample(Coin.RollBaseZero()), gold: D12.RollBaseZero(),
                baseDescription: "Its body is covered in a protective layer of chitin and has far too many legs."),
            
            
            new Npc("Giant Viper",      003, Difficulty.Easy,       hp: 6,  ac: 9, 
                damageDice: new Die[]{D8},
                inventory: Junk.RandomSample(D4.RollBaseZero()), gold: D12.RollBaseZero(),
                baseDescription: "Its enlarged fangs drip with venom and its body is corded with muscle."),
            new Npc("Cave Crocodile",   004, Difficulty.Easy,       hp: 10,  ac: 11, dr: 2, 
                damageDice: new Die[]{D6},
                inventory: Junk.RandomSample(D4.RollBaseZero()), gold: D20.RollBaseZero(),
                baseDescription: "Its toothy maw gapes open, waiting for any unwitting prey, and its body is covered in a thick reptilian hide."),
            new Npc("Hunter Spider",    005, Difficulty.Easy,       hp: 5,  ac: 9, 
                attackMod: 1,   damageDice: new Die[]{D8},
                inventory: Junk.RandomSample(D4.RollBaseZero()), gold: D12.RollBaseZero(),
                baseDescription: "It has long spindly legs and its body is covered in course hairs."),

            
            new Npc("Cave Bear",        006, Difficulty.Medium,     hp: 14,  ac: 10, dr: 3, searchRange: 6,
                damageDice: new Die[]{D6, D6},
                inventory: Commons.RandomSample(D4.RollBaseZero()).Union(Uncommons.RandomSample(Coin.RollBaseZero())).ToList(), 
                gold: D20.RollBaseZero(),
                baseDescription: "Its massive paws are the size of your head and its form is etched with scars from past battles."),
            new Npc("Rabid Wolf",       007, Difficulty.Medium,     hp: 9,   ac: 9, searchRange: 8,
                attackMod: 2, damageDice: new Die[]{D4, D4}, damageMod: 1,
                inventory: Commons.RandomSample(D3.RollBaseZero()), 
                gold: D12.RollBaseZero() + D6.RollBaseZero(),
                baseDescription: "It has bloodshot eyes and ragged fur. As it pants, yellowish-white foam drips from its mouth."),
            new Npc("Giant Poison Toad",008, Difficulty.Medium,     hp: 11,  ac: 10, dr: 1,
                attackRange: 2, attackMod: 3, damageDice: new Die[]{D4, D4},
                inventory: Commons.RandomSample(D3.RollBaseZero()).Union(Uncommons.RandomSample(Coin.RollBaseZero())).ToList(), 
                gold: D8.RollBaseZero() + D8.RollBaseZero(),
                baseDescription: "Its long tongue allows it to attack its enemies from range, and its bite could take off a man's head."),
            
            
            new Npc("Spitting Cobra",   009, Difficulty.Hard,       hp: 9,  ac: 10, 
                attackRange: 3, attackMod: 3, damageDice: new Die[]{D8, D8}, damageMod: 2,
                inventory: Uncommons.RandomSample(D3.RollBaseZero()).Union(Rares.RandomSample(Coin.RollBaseZero())).ToList(), 
                gold: D8.RollBaseZero() + D8.RollBaseZero() + D8.RollBaseZero(),
                baseDescription: "Though small, its venomous spray is known to kill creatures ten times its size."),
            new Npc("Black Scorpion",   010, Difficulty.Hard,       hp: 10,  ac: 13, dr: 2, 
                attackMod: 2, damageDice: new Die[]{D12, D4, D4}, damageMod: 1,
                inventory: Uncommons.RandomSample(D3.RollBaseZero()).Union(Rares.RandomSample(Coin.RollBaseZero())).ToList(), 
                gold: D12.RollBaseZero() + D12.RollBaseZero(),
                baseDescription: "It possesses two massive claws and a deadly stinger at the end of its tail."),
            
            
            new Npc("Cave Mastodon",    011, Difficulty.VeryHard,   hp: 20,  ac: 13, dr: 4, 
                attackMod: 2, damageDice: new Die[]{D12, D12}, damageMod: 3,
                inventory: Uncommons.RandomSample(D4.RollBaseZero()).Union(Rares.RandomSample(D3.RollBaseZero())).Union(VeryRares.RandomSample(Coin.RollBaseZero())).ToList(), 
                gold: D20.RollBaseZero() + D20.RollBaseZero(),
                baseDescription: "It has two giant tusks and its hairy body is covered in a thick layer of protective fat."),
        };
    }
}