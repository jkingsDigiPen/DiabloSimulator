//------------------------------------------------------------------------------
//
// File Name:	HeroManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using DiabloSimulator.Engine;
using DiabloSimulator.Game.World;
using Newtonsoft.Json;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    class HeroManager : IModule
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public override void Inintialize()
        {
            // Create save location if it doesn't exist
            saveLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\DiabloSimulator\\Saves\\";
            Directory.CreateDirectory(saveLocation);

            // Populate save list
            string[] saveFileList = Directory.GetDirectories(saveLocation);
            foreach (string file in saveFileList)
            {
                SavedCharacters.Add(file.Substring(saveLocation.Length));
            }

            // Register for events
            AddEventHandler(GameEvents.PlayerAttack, OnPlayerAttack);
            AddEventHandler(GameEvents.MonsterAttack, OnMonsterAttack);
            AddEventHandler(GameEvents.MonsterDead, OnMonsterDead);
            AddEventHandler(GameEvents.AdvanceTime, OnAdvanceTime);
            AddEventHandler(GameEvents.HeroDead, OnHeroDead);
            AddEventHandler(GameEvents.PlayerRest, OnPlayerRest);
            AddEventHandler(GameEvents.WorldZoneDiscovery, OnWorldZoneDiscovery);
            AddEventHandler(GameEvents.SetWorldZone, OnSetWorldZone);

            AddEventHandler(GameEvents.ItemEquip, OnItemEquip);
            AddEventHandler(GameEvents.ItemDiscard, OnItemDiscard);
            AddEventHandler(GameEvents.ItemSell, OnItemSell);
            AddEventHandler(GameEvents.ItemJunk, OnItemJunk);
            AddEventHandler(GameEvents.ItemKeep, OnItemKeep);
            AddEventHandler(GameEvents.ItemUnequip, OnItemUnequip);

            AddEventHandler(GameEvents.PlayerUsePotion, OnPlayerUsePotion);
            AddEventHandler(GameEvents.GameSave, OnGameSave);
            AddEventHandler(GameEvents.GameLoad, OnGameLoad);
            AddEventHandler(GameEvents.HeroCreate, OnHeroCreate);
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public Hero Hero { get; set; } = new Hero("The Vagabond");

        public List<string> SavedCharacters { get; } = new List<string>();

        public bool CanLoadState
        {
            get => SavedCharacters.Count > 0;
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnPlayerAttack(object sender, GameEventArgs e)
        {
            var damageDealt = Hero.GetAttackDamage();

            // TO DO: Specify target monster
            RaiseGameEvent(GameEvents.HeroAttack, Hero, damageDealt);
        }

        private void OnMonsterAttack(object sender, GameEventArgs e)
        {
            Monster monster = sender as Monster;
            string damageDealtString = Hero.Damage(e.Get<List<DamageArgs>>());
            RaiseGameEvent(GameEvents.AddWorldEventText, this, monster.Name + " attacks you. " + damageDealtString);

            if (Hero.IsDead)
                RaiseGameEvent(GameEvents.HeroDead, Hero);
        }

        private void OnMonsterDead(object sender, GameEventArgs e)
        {
            Monster monster = sender as Monster;
            if (monster.Name != Monster.EmptyMonster)
                Hero.AddExperience(monster);
        }

        private void OnAdvanceTime(object sender, GameEventArgs e)
        {
            if (Hero.IsDead) return;

            float lifeRegenAmount = Hero.Stats.ModifiedValues["HealthRegen"];
            if (lifeRegenAmount != 0)
            {
                RaiseGameEvent(GameEvents.AddWorldEventText, Hero,
                    Hero.Heal(lifeRegenAmount) + " from natural healing.");
            }
        }

        private void OnHeroDead(object sender, GameEventArgs e)
        {
            MessageBox.Show("You have died. You will be revived in town.",
                "Diablo Simulator", MessageBoxButton.OK, MessageBoxImage.Information);
            Hero.Revive();
            RaiseGameEvent(GameEvents.AddWorldEventText, this, "A fellow wanderer stumbles upon your lifeless body " +
               "and brings you back to town, where the healers somehow manage to breathe life " +
               "into you once again.");
            RaiseGameEvent(GameEvents.SetWorldZone, this, "New Tristram");
        }

        private void OnPlayerRest(object sender, GameEventArgs e)
        {
            // Add regen - additive and multiplicative
            StatModifier regenMultBonus = new StatModifier("HealthRegen",
                "Rest", ModifierType.Multiplicative, 0.5f);
            StatModifier regenAddBonus = new StatModifier("HealthRegen",
                "Rest", ModifierType.Additive, 2);

            Hero.Stats.AddModifier(regenMultBonus);
            Hero.Stats.AddModifier(regenAddBonus);
            RaiseGameEvent(GameEvents.AddWorldEventText, this, "You rest for a short while. You feel healthier!");

            // Step time forward to heal
            RaiseGameEvent(GameEvents.AdvanceTime);

            // Remove temporary regen
            Hero.Stats.RemoveModifier(regenMultBonus);
            Hero.Stats.RemoveModifier(regenAddBonus);
        }

        private void OnWorldZoneDiscovery(object sender, GameEventArgs e)
        {
            WorldEvent worldEvent = e.Get<WorldEvent>();
            if (!Hero.DiscoveredZones.Contains(worldEvent.Name))
            {
                Hero.DiscoveredZones.Add(worldEvent.Name);
                RaiseGameEvent(GameEvents.AddWorldEventText, this,
                    "You have discovered " + worldEvent.Name + "!");
            }
            else
            {
                RaiseGameEvent(GameEvents.AddWorldEventText, this,
                    "You have found the entrance to " + worldEvent.Name + ".");
            }
            RaiseGameEvent(GameEvents.AddWorldEventText, this,
                "Click " + GameManager.discoverChoiceText.Choice01Text
                + " to explore this area. If you wish to return to the previous area, " +
                "click " + GameManager.discoverChoiceText.Choice02Text + ".");
        }

        private void OnSetWorldZone(object sender, GameEventArgs e)
        {
            Hero.CurrentZone = e.Get<string>();
        }

        private void OnItemUnequip(object sender, GameEventArgs e)
        {
            SlotType slot = e.Get<SlotType>();

            if(slot == SlotType.MainHand)
            {
                Item unequipped = Hero.UnequipItem(SlotType.MainHand);

                if (unequipped is null)
                    return;

                if (unequipped.slot == SlotType.BothHands)
                    Hero.UnequipItem(SlotType.OffHand);
            }
            else if(slot == SlotType.OffHand)
            {
                Item unequipped = Hero.UnequipItem(SlotType.OffHand);

                if (unequipped is null)
                    return;

                if (unequipped.slot == SlotType.BothHands)
                    Hero.UnequipItem(SlotType.MainHand);
            }
            else 
            {
                Hero.UnequipItem(e.Get<SlotType>());
            }
        }

        private void OnPlayerUsePotion(object sender, GameEventArgs e)
        {
            Hero.UsePotion();
        }

        private void OnGameSave(object sender, GameEventArgs e)
        {
            // Attempt to create directories
            string heroSaveLocation = saveLocation + Hero.Name.Trim() + "\\";
            Directory.CreateDirectory(heroSaveLocation);

            // Save hero data, inventory, equipment
            string heroDataFilename = heroSaveLocation + "HeroData.txt";
            string heroStrings = JsonConvert.SerializeObject(Hero, Formatting.Indented);

            var stream = new StreamWriter(heroDataFilename);
            stream.Write(heroStrings);
            stream.Close();
        }

        private void OnGameLoad(object sender, GameEventArgs e)
        {
            string heroSaveLocation = saveLocation + e.Get<string>() + "\\";

            // Load hero data, inventory, equipment
            string heroDataFilename = heroSaveLocation + "HeroData.txt";
            var stream = new StreamReader(heroDataFilename);
            string heroStrings = stream.ReadToEnd();
            stream.Close();

            Hero = JsonConvert.DeserializeObject<Hero>(heroStrings);
            Hero.Stats.RemapModifierSources(Hero);

            // Load zone data
            RaiseGameEvent(GameEvents.SetWorldZone, this, Hero.CurrentZone);
        }

        private void OnHeroCreate(object sender, GameEventArgs e)
        {
            // Create hero class, set name and description
            Hero = heroFactory.Create(Hero.Archetype, Hero);

            // Set starting zone
            RaiseGameEvent(GameEvents.SetWorldZone, this, "New Tristram");
            Hero.DiscoveredZones.Add(Hero.CurrentZone);

            // Add starting equipment
            Hero.Inventory.PotionsHeld = 3;
            Hero.Inventory.AddItem(itemFactory.Create("Simple Dagger", Hero));
            Hero.Inventory.AddItem(itemFactory.Create("Short Sword", Hero));
            Hero.Inventory.AddItem(itemFactory.Create("Leather Hood", Hero));
        }

        #region inventoryEvents

        private void OnItemKeep(object sender, GameEventArgs e)
        {
            Hero.Inventory.KeepItem(e.Get<int>());
        }

        private void OnItemJunk(object sender, GameEventArgs e)
        {
            Hero.Inventory.JunkItem(e.Get<int>());
        }

        private void OnItemSell(object sender, GameEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnItemDiscard(object sender, GameEventArgs e)
        {
            Item itemToRemove = Hero.Inventory.Items[e.Get<int>()];
            Hero.Inventory.DiscardItem(itemToRemove);
        }

        private void OnItemEquip(object sender, GameEventArgs e)
        {
            Hero.EquipItem(e.Get<int>());
        }

        #endregion

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private HeroFactory heroFactory = new HeroFactory();
        private ItemFactory itemFactory = new ItemFactory();
        private string saveLocation;
    }
}
