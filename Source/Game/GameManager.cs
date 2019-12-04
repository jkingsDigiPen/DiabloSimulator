//------------------------------------------------------------------------------
//
// File Name:	GameManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Game.Factories;
using DiabloSimulator.Game.World;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class GameManager
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        #region constructors

        public GameManager()
        {
            // Instantiate Console window
            ConsoleManager.Show();

            // Actors
            hero = new Hero("The Vagabond");
            monster = new Monster();

            // Factories
            heroFactory = new HeroFactory();
            monsterFactory = new MonsterFactory();
            itemFactory = new ItemFactory();
            zoneFactory = new WorldZoneFactory();

            // Internal data
            nextEvent = new StringWriter();

            // Action delegates
            actionFunctions = new Dictionary<PlayerActionType, ActionFunction>();
            actionFunctions[PlayerActionType.Look] = Look;
            actionFunctions[PlayerActionType.Attack] = Attack;
            actionFunctions[PlayerActionType.Defend] = Defend;
            actionFunctions[PlayerActionType.Explore] = Explore;
            actionFunctions[PlayerActionType.Rest] = Rest;
            actionFunctions[PlayerActionType.Flee] = Flee;
            actionFunctions[PlayerActionType.TownPortal] = TownPortal;
            actionFunctions[PlayerActionType.Proceed] = Proceed;
            actionFunctions[PlayerActionType.Back] = Back;

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

            // Initial game text (new or otherwise)
            nextEvent.WriteLine("Welcome to the world of Sanctuary!");

            // Load audio shtuff
            audio = new AudioManager();
            audio.LoadBank("Master.strings");
            audio.LoadBank("Master");
            audio.LoadBank("Music");
        }

        #endregion

        #region gameFunctions

        public PlayerActionResult GetActionResult(PlayerAction action)
        {
            // Execute action
            actionFunctions[action.actionType](action.args);

            // Return output
            string result = nextEvent.ToString();
            nextEvent.GetStringBuilder().Clear();

            return new PlayerActionResult(result, currentChoiceText);
        }

        public Hero Hero { get => hero; }

        public Monster Monster { get => monster; }

        public bool InCombat 
        { 
            get => inCombat; 
            set
            {
                inCombat = value;

                if (value == true)
                    currentChoiceText = combatChoiceText;
                else
                    currentChoiceText = exploreChoiceText;
            }
        }

        public int Turns { get => turns; }

        #endregion

        #region saveLoad

        public void SaveState()
        {
            // Attempt to create directories
            string heroSaveLocation = saveLocation + Hero.Name.Trim() + "\\";
            Directory.CreateDirectory(heroSaveLocation);

            // Save hero data, inventory, equipment
            string heroDataFilename = heroSaveLocation + "HeroData.txt";
            string heroStrings = JsonConvert.SerializeObject(hero, Formatting.Indented);

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

            hero = JsonConvert.DeserializeObject<Hero>(heroStrings);
            hero.Stats.RemapModifierSources(hero);

            // Load zone data
            SetZone(hero.CurrentZone);
        }

        public List<string> SavedCharacters
        {
            get => savedCharacters;
        }

        public bool CanLoadState
        {
            get => SavedCharacters.Count > 0;
        }

        #endregion

        //------------------------------------------------------------------------------
        // Private Structures
        //------------------------------------------------------------------------------

        private delegate void ActionFunction(List<string> args);

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        #region playerActions

        private void Look(List<string> args)
        {
            nextEvent.WriteLine(zone.LookText);
        }

        private void Explore(List<string> args)
        {
            WorldEvent worldEvent = zone.EventTable.GenerateObject(hero);
            ProcessWorldEvent(worldEvent);
        }

        private void Attack(List<string> args)
        {
            float damageDealt = Hero.GetAttackDamage()[0].amount;
            string damageDealtString = DamageMonster(damageDealt);
            nextEvent.WriteLine("You attack the " + Monster.Race + ". " + damageDealtString);

            if (!Monster.IsDead())
            {
                damageDealtString = Hero.Damage(Monster.GetAttackDamage());
                nextEvent.WriteLine(Monster.Name + " attacks you. " + damageDealtString);
            }
            else
            {
                hero.AddExperience(monster);
            }

            AdvanceTime();
        }

        private void Defend(List<string> args)
        {
            // TO DO: Add additive bonus dodge chance, mult bonus to block chance
            nextEvent.WriteLine("You steel yourself, waiting for your enemy to attack.");

            if (!Monster.IsDead())
            {
                string damageDealtString = Hero.Damage(Monster.GetAttackDamage());
                nextEvent.WriteLine(Monster.Name + " attacks you. " + damageDealtString);
            }

            // TO DO: Remove bonus dodge chance, block chance

            AdvanceTime();
        }

        private void Rest(List<string> args)
        {
            // Add regen - additive and multiplicative
            StatModifier regenMultBonus = new StatModifier("HealthRegen",
                "Rest", Game.ModifierType.Multiplicative, 0.5f);
            StatModifier regenAddBonus = new StatModifier("HealthRegen",
                "Rest", Game.ModifierType.Additive, 2);

            Hero.Stats.AddModifier(regenMultBonus);
            Hero.Stats.AddModifier(regenAddBonus);
            nextEvent.WriteLine("You rest for a short while. You feel healthier!");

            // Step time forward to heal
            AdvanceTime();

            // Remove temporary regen
            Hero.Stats.RemoveModifier(regenMultBonus);
            Hero.Stats.RemoveModifier(regenAddBonus);
        }

        private void Flee(List<string> args)
        {
            nextEvent.WriteLine("You attempt to flee from the " + Monster.Race + "...");

            // 60% flee chance
            bool fleeSuccess = random.NextDouble() <= 0.6f;
            if(fleeSuccess)
            {
                nextEvent.WriteLine("You have successfully escaped from " + Monster.Name + ".");
                DestroyMonster();
                Look(null);
            }
            else
            {
                nextEvent.WriteLine("You can't seem to find an opening to escape! " +
                    "You are locked in combat with " + Monster.Name + ".");

                if (!Monster.IsDead())
                {
                    string damageDealtString = Hero.Damage(Monster.GetAttackDamage());
                    nextEvent.WriteLine(Monster.Name + " attacks you. " + damageDealtString);
                }

                AdvanceTime();
            }
        }

        private void TownPortal(List<string> args)
        {
            if (zone.ZoneType == WorldZoneType.Town)
            {
                nextEvent.WriteLine("There is no need to cast 'Town Portal' at this time. You are already in town.");
            }
            else
            {
                nextEvent.WriteLine("You reach into your pack and take out a dusty blue tome containing " +
                    "the 'Town Portal' spell. You read the words aloud and suddenly a glowing, translucent " +
                    "portal opens up in the air in front of you. Stepping through it, you find yourself " +
                    "back in town.");
                SetZone("Tristram");
                Look(null);
            }
        }

        private void Proceed(List<string> args)
        {
            SetZone(nextZoneName);
            Look(null);
        }

        private void Back(List<string> args)
        {
            nextEvent.WriteLine("You step back from the entrance to " + nextZoneName 
                + ", remaining in " + zone.Name + ".");

            // TO DO: Provide town choices if in town
            currentChoiceText = exploreChoiceText;
        }

        #endregion

        #region heroFunctions

        public void CreateHero()
        {
            // Create hero class, set name and description
            hero = heroFactory.Create(Hero.Archetype, Hero);

            // Set starting zone
            SetZone("Tristram");
            hero.DiscoveredZones.Add(hero.CurrentZone);

            // Add starting equipment
            hero.Inventory.PotionsHeld = 3;
            hero.Inventory.AddItem(itemFactory.Create("Simple Dagger", hero));
            hero.Inventory.AddItem(itemFactory.Create("Short Sword", hero));
            hero.Inventory.AddItem(itemFactory.Create("Leather Hood", hero));
        }

        private void HeroLifeRegen()
        {
            float lifeRegenAmount = Hero.Stats.ModifiedValues["HealthRegen"];
            if (lifeRegenAmount != 0)
            {
                nextEvent.WriteLine(Hero.Heal(lifeRegenAmount) + " from natural healing.");
            }
        }

        #endregion

        #region monsterFunctions

        public void DestroyMonster()
        {
            Monster.Kill();
            monster = new Monster();
            InCombat = false;
        }

        public string DamageMonster(float amount)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(amount));
            return Monster.Damage(damageList);
        }

        #endregion

        #region zoneFunctions

        private void ProcessWorldEvent(WorldEvent worldEvent)
        {
            nextEvent.WriteLine(worldEvent.EventText);

            switch (worldEvent.EventType)
            {
                // Monster generation
                case WorldEventType.MonsterEvent:
                    turns = 0;
                    InCombat = true;

                    if (worldEvent.Name == "Wandering Monster")
                    {
                        // Get random monster name
                        monster = zone.MonsterTable.GenerateObject(hero, monsterFactory);
                    }
                    else
                    {
                        string monsterName = worldEvent.EventData[0];
                        monster = monsterFactory.Create(monsterName, hero);
                    }

                    nextEvent.WriteLine(Monster.Name + ", a level "
                            + Monster.Stats.Level + " " + Monster.Race + ", appeared!");
                    break;
                default:
                    break;

                case WorldEventType.ZoneDiscoveryEvent:
                    currentChoiceText = discoverChoiceText;

                    if(!hero.DiscoveredZones.Contains(worldEvent.Name))
                    {
                        hero.DiscoveredZones.Add(worldEvent.Name);
                        nextEvent.WriteLine("You have discovered " + worldEvent.Name + "!");
                    }
                    else
                    {
                        nextEvent.WriteLine("You have found the entrance to " + worldEvent.Name + ".");
                    }

                    nextZoneName = worldEvent.Name;

                    nextEvent.WriteLine("Click " + discoverChoiceText.Choice01Text 
                        + " to explore this area. If you wish to return to the previous area, " +
                        "click " + discoverChoiceText.Choice02Text + ".");
                    break;
            }
        }

        private void SetZone(string name)
        {
            hero.CurrentZone = name;

            // Only change zones if necessary
            if (zone != null && hero.CurrentZone == zone.Name)
                return;

            zone = zoneFactory.Create(hero.CurrentZone);

            // TO DO: Provide town choices if in town
            currentChoiceText = exploreChoiceText;

            // Set the mood
            if (ambientTrack.isValid())
                AudioManager.ErrorCheck(ambientTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
            if (zone.AmbientTrackName != null)
                audio.PlayEvent(zone.AmbientTrackName, 0.5f);

            // Play that funky muzak
            if (musicTrack.isValid())
                AudioManager.ErrorCheck(musicTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
            if (zone.MusicTrackName != null)
                audio.PlayEvent(zone.MusicTrackName, 0.5f);
        }

        #endregion

        #region gameStateFunctions

        private void AdvanceTime()
        {
            bool heroDead = Hero.IsDead();
            bool monsterDead = Monster.IsDead();

            if (heroDead || monsterDead)
            {
                InCombat = false;

                // Check for player death
                if (heroDead)
                {
                    GameOver();
                }
            }
            else
            {
                ++turns;
                //nextEvent.WriteLine("A round of combat ends. (Round " + Turns + ")");
            }

            if (!heroDead)
            {
                HeroLifeRegen();
            }
        }

        private void GameOver()
        {
            MessageBox.Show("You have died. You will be revived in town.", 
                "Diablo Simulator", MessageBoxButton.OK, MessageBoxImage.Information);

            Hero.Revive();
            DestroyMonster();

            nextEvent.WriteLine("A fellow wanderer stumbles upon your lifeless body " +
                "and brings you back to town, where the healers somehow manage to breathe life " +
                "into you once again.");
            SetZone("Tristram");
            Look(null);
        }

        #endregion

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        // Pseudo-constants
        public static PlayerChoiceText exploreChoiceText =
            new PlayerChoiceText(PlayerActionType.Explore.ToString(),
                PlayerActionType.Rest.ToString(), 
                PlayerAction.GetDescription(PlayerActionType.TownPortal));

        public static PlayerChoiceText combatChoiceText =
            new PlayerChoiceText(PlayerActionType.Attack.ToString(),
                PlayerActionType.Defend.ToString(), PlayerActionType.Flee.ToString());

        public static PlayerChoiceText discoverChoiceText =
            new PlayerChoiceText(PlayerActionType.Proceed.ToString(),
                PlayerActionType.Back.ToString(), 
                PlayerAction.GetDescription(PlayerActionType.TownPortal));

        public static PlayerChoiceText yesNoChoiceText =
            new PlayerChoiceText(PlayerActionType.Yes.ToString(),
                PlayerActionType.No.ToString(), PlayerActionType.Back.ToString());

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Game state
        private bool inCombat;
        private int turns;
        private StringWriter nextEvent;
        private List<string> savedCharacters;
        private string saveLocation;
        private PlayerChoiceText currentChoiceText;
        private FMOD.Studio.EventInstance musicTrack;
        private FMOD.Studio.EventInstance ambientTrack;

        // Actors
        private Monster monster;
        private Hero hero;
        private WorldZone zone;
        private string nextZoneName;

        // Factories
        private MonsterFactory monsterFactory;
        private HeroFactory heroFactory;
        private ItemFactory itemFactory;
        private WorldZoneFactory zoneFactory;

        // Internal data
        private Dictionary<PlayerActionType, ActionFunction> actionFunctions;
        private Random random = new Random();
        private AudioManager audio;
    }
}
