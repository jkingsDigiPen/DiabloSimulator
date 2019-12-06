using System;
using System.Collections.Generic;
using System.IO;
using DiabloSimulator.Engine;
using DiabloSimulator.Game.World;
using Newtonsoft.Json;

namespace DiabloSimulator.Game
{
    class HeroManager : IModule
    {
        public void Inintialize()
        {
            eventManager = EngineCore.GetModule<WorldEventManager>();
            zoneManager = EngineCore.GetModule<ZoneManager>();

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
        }

        public void CreateHero()
        {
            // Create hero class, set name and description
            Hero = heroFactory.Create(Hero.Archetype, Hero);

            // Set starting zone
            EngineCore.GetModule<ZoneManager>().SetZone("Tristram");
            Hero.DiscoveredZones.Add(Hero.CurrentZone);

            // Add starting equipment
            Hero.Inventory.PotionsHeld = 3;
            Hero.Inventory.AddItem(itemFactory.Create("Simple Dagger", Hero));
            Hero.Inventory.AddItem(itemFactory.Create("Short Sword", Hero));
            Hero.Inventory.AddItem(itemFactory.Create("Leather Hood", Hero));
        }

        public void HeroLifeRegen()
        {
            float lifeRegenAmount = Hero.Stats.ModifiedValues["HealthRegen"];
            if (lifeRegenAmount != 0)
            {
                eventManager.NextEvent = Hero.Heal(lifeRegenAmount) + " from natural healing.";
            }
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
            zoneManager.SetZone(Hero.CurrentZone);
        }

        public Hero Hero { get; set; } = new Hero("The Vagabond");

        public List<string> SavedCharacters { get; } = new List<string>();

        public bool CanLoadState
        {
            get => SavedCharacters.Count > 0;
        }

        private HeroFactory heroFactory = new HeroFactory();
        private ItemFactory itemFactory = new ItemFactory();
        private string saveLocation;

        // Modules
        private WorldEventManager eventManager;
        private ZoneManager zoneManager;
    }
}
