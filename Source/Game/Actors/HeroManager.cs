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
        HeroManager()
        {
            // Create save location if it doesn't exist
            saveLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\DiabloSimulator\\Saves\\";
            Directory.CreateDirectory(saveLocation);

            // Populate save list
            savedCharacters = new List<string>();
            string[] saveFileList = Directory.GetDirectories(saveLocation);
            foreach (string file in saveFileList)
            {
                savedCharacters.Add(file.Substring(saveLocation.Length));
            }
        }

        public void Inintialize()
        {

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

        private void HeroLifeRegen()
        {
            float lifeRegenAmount = Hero.Stats.ModifiedValues["HealthRegen"];
            if (lifeRegenAmount != 0)
            {
                nextEvent.WriteLine(Hero.Heal(lifeRegenAmount) + " from natural healing.");
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
            SetZone(Hero.CurrentZone);
        }

        public Hero Hero { get; set; } = new Hero("The Vagabond");

        public List<string> SavedCharacters
        {
            get => savedCharacters;
        }

        public bool CanLoadState
        {
            get => SavedCharacters.Count > 0;
        }

        private HeroFactory heroFactory = new HeroFactory();
        private List<string> savedCharacters = new List<string>();
        private string saveLocation;
    }
}
