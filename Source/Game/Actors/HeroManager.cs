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
            zoneManager = EngineCore.GetModule<WorldZoneManager>();

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
        }

        public void CreateHero()
        {
            // Create hero class, set name and description
            Hero = heroFactory.Create(Hero.Archetype, Hero);

            // Set starting zone
            zoneManager.SetZone("New Tristram");
            Hero.DiscoveredZones.Add(Hero.CurrentZone);

            // Add starting equipment
            Hero.Inventory.PotionsHeld = 3;
            Hero.Inventory.AddItem(itemFactory.Create("Simple Dagger", Hero));
            Hero.Inventory.AddItem(itemFactory.Create("Short Sword", Hero));
            Hero.Inventory.AddItem(itemFactory.Create("Leather Hood", Hero));
        }

        public void SaveState()
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

        public void LoadState(string saveFileName)
        {
            string heroSaveLocation = saveLocation + saveFileName + "\\";

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

            if(Hero.IsDead)
                RaiseGameEvent(GameEvents.HeroDead, Hero);
        }

        private void OnMonsterDead(object sender, GameEventArgs e)
        {
            Monster monster = sender as Monster;
            if(monster.Name != Monster.EmptyMonster)
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

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private HeroFactory heroFactory = new HeroFactory();
        private ItemFactory itemFactory = new ItemFactory();
        private string saveLocation;

        // Modules
        private WorldZoneManager zoneManager;
    }
}
