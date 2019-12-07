//------------------------------------------------------------------------------
//
// File Name:	Hero.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class Hero : GameObject, ICombatActor
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        #region constructors

        [JsonConstructor]
        public Hero(string name_ = "", string heroClass = "Warrior")
            : base(name_, heroClass)
        {
            StatPriorities = new List<string>();
            Inventory = new Inventory();
            Equipment = new Equipment();
            random = new Random();
            DiscoveredZones = new List<string>();
            UniqueEventsSeen = new List<string>();
        }

        public Hero(Hero other)
            : base(other)
        {
            StatPriorities = new List<string>(other.StatPriorities);
            Inventory = new Inventory(other.Inventory);
            Equipment = new Equipment(other.Equipment);
            random = new Random();
            DiscoveredZones = new List<string>(other.DiscoveredZones);
            UniqueEventsSeen = new List<string>(other.UniqueEventsSeen);
        }

        #endregion

        #region damageAndHealing

        public string Heal(float amount)
        {
            amount = MathF.Ceiling(amount);

            // Increase health, but keep below max
            Stats["CurrentHealth"] = Math.Min(Stats.BaseValues["CurrentHealth"] + amount, 0);

            return "You heal " + amount + " damage";
        }

        public string Damage(List<DamageArgs> damageList)
        {
            string result = "";

            // TO DO: Calculate actual damage based on damage, resist

            // Decrease health, but keep above 0
            int i = 0;
            foreach (DamageArgs damage in damageList)
            {
                if (i != 0)
                    result += "\n";
                ++i;

                Stats["CurrentHealth"] = Math.Max(Stats.BaseValues["CurrentHealth"] - damage.amount,
                -Stats.ModifiedValues["MaxHealth"]);

                result += "You take " + damage.amount;
                if (damage.damageType != DamageType.Physical)
                    result += " " + damage.damageType.ToString();
                result += " damage.";
            }

            if (IsDead)
            {
                result += "\n";
                Kill();
                result += "You have been vanquished by the forces of evil!\n You have lost all of your gold.";
            }

            return result;
        }

        public string Damage(float damage)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(damage));
            return Damage(damageList);
        }

        public List<DamageArgs> GetAttackDamage()
        {
            var damageList = new List<DamageArgs>();

            int minValue = (int)Stats.ModifiedValues["MinDamage"];
            int maxValue = (int)Stats.ModifiedValues["MaxDamage"];

            damageList.Add(new DamageArgs(random.Next(minValue, maxValue + 1)));
            return damageList;
        }

        public void Kill()
        {
            // Reduce health
            Stats["CurrentHealth"] = -Stats.ModifiedValues["MaxHealth"];

            // Lose gold
            Inventory.GoldAmount = 0;

            // TO DO: Remove temp buffs

        }

        public void Revive()
        {
            Stats["CurrentHealth"] = 0;
        }

        public bool IsDead { get => Stats.ModifiedValues["CurrentHealth"] == 0; }

        #endregion

        #region experience

        public void AddExperience(Monster monster)
        {
            // TO DO: Account for monster level (if significantly
            // higher or lower than player level

            // TO DO: Account for experience buffs

            Stats["Experience"] = Stats.BaseValues["Experience"] + monster.Stats.ModifiedValues["Experience"];

            float currentExperience = Stats.ModifiedValues["Experience"];

            // Check for level up
            if(currentExperience >= CalculateExperienceThreshold(Stats.Level))
            {
                Stats.Level = Stats.Level + 1;
            }

            base.OnPropertyChange("ExperienceNeeded");
        }

        public float ExperienceNeeded
        {
            get
            {
                return CalculateExperienceThreshold(Stats.Level) 
                    - Stats.ModifiedValues["Experience"];
            }
        }

        #endregion

        #region equipment

        public void EquipItem(int inventoryIndex)
        {
            Item itemToEquip = Inventory.Items[inventoryIndex];

            // Don't equip junk items
            if (itemToEquip.junkStatus == JunkStatus.Junk)
                return;

            // TO DO: Handle rings

            // Remove currently equipped item
            Item itemToRemove = Equipment.UnequipItem(itemToEquip.slot, Stats);

            // Equip item in slot
            Equipment.EquipItem(itemToEquip, Stats);

            // Remove item from inventory
            Inventory.RemoveItem(itemToEquip);

            // Add unequipped item to inventory
            if (itemToRemove != null)
            {
                Inventory.AddItem(itemToRemove);
            }
        }

        public Item UnequipItem(SlotType slot)
        {
            // Unequip item
            Item unequipped = Equipment.UnequipItem(slot, Stats);

            // Add to inventory
            if (unequipped is null)
                return null;

           Inventory.AddItem(unequipped);

            return unequipped;
        }

        public void UsePotion()
        {
            if (Inventory.PotionsHeld == 0)
                return;

            --Inventory.PotionsHeld;

            Heal(Stats.ModifiedValues["MaxHealth"] * 0.6f);
        }

        #endregion

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public Inventory Inventory { get; set; }

        public Equipment Equipment { get; set; }

        public List<string> StatPriorities { get; set; }

        public List<string> DiscoveredZones { get; set; }

        public string CurrentZone { get; set; }

        public List<string> UniqueEventsSeen { get; set; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        float CalculateExperienceThreshold(int currentLevel)
        {
            if(currentLevel == 1)
            {
                return 280;
            }
            else if(currentLevel == 2)
            {
                return CalculateExperienceThreshold(1) + 2700;
            }
            else if(currentLevel < 7)
            {
                return CalculateExperienceThreshold(currentLevel - 1) * 2 
                    + 900 + currentLevel * 300;
            }
            else
            {
                return CalculateExperienceThreshold(currentLevel - 1) * 2
                    + 900 + currentLevel * 200;
            }
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Random random;
    }
}
