//------------------------------------------------------------------------------
//
// File Name:	GameManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game.Factories;
using DiabloSimulator.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class GameManager : IModule
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        #region constructors

        public GameManager()
        {
            // Factories
            itemFactory = new ItemFactory();

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
        }

        public void Inintialize()
        {
            eventManager = EngineCore.GetModule<WorldEventManager>();
            monsterManager = EngineCore.GetModule<MonsterManager>();
            heroManager = EngineCore.GetModule<HeroManager>();
            audioManager = EngineCore.GetModule<AudioManager>();

            // Load audio shtuff
            audioManager.LoadBank("Master.strings");
            audioManager.LoadBank("Master");
            audioManager.LoadBank("Music");
        }

        #endregion

        #region gameFunctions

        public PlayerActionResult GetActionResult(PlayerAction action)
        {
            // Execute action
            actionFunctions[action.actionType](action.args);

            // Return output
            string result = eventManager.NextEvent;

            return new PlayerActionResult(result, CurrentChoiceText);
        }

        public bool InCombat 
        { 
            get => inCombat; 
            set
            {
                inCombat = value;

                if (value == true)
                    CurrentChoiceText = combatChoiceText;
                else
                    CurrentChoiceText = exploreChoiceText;
            }
        }

        public int Turns { get => turns; }

        public PlayerChoiceText CurrentChoiceText { get; set; }

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
            eventManager.NextEvent = zone.LookText;
        }

        private void Explore(List<string> args)
        {
            WorldEvent worldEvent = zone.EventTable.GenerateObject(hero);
            eventManager.ProcessWorldEvent(worldEvent);
        }

        private void Attack(List<string> args)
        {
            float damageDealt = Hero.GetAttackDamage()[0].amount;
            string damageDealtString = DamageMonster(damageDealt);
            eventManager.NextEvent = "You attack the " + Monster.Race + ". " + damageDealtString;

            if (!Monster.IsDead())
            {
                damageDealtString = Hero.Damage(Monster.GetAttackDamage());
                eventManager.NextEvent = Monster.Name + " attacks you. " + damageDealtString;
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
            eventManager.NextEvent = "You steel yourself, waiting for your enemy to attack.";

            if (!Monster.IsDead())
            {
                string damageDealtString = Hero.Damage(Monster.GetAttackDamage());
                eventManager.NextEvent = Monster.Name + " attacks you. " + damageDealtString;
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
            eventManager.NextEvent = "You rest for a short while. You feel healthier!";

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
                eventManager.NextEvent = "You have successfully escaped from " + Monster.Name + ".";
                DestroyMonster();
                Look(null);
            }
            else
            {
                eventManager.NextEvent = "You can't seem to find an opening to escape! " +
                    "You are locked in combat with " + Monster.Name + ".";

                if (!Monster.IsDead())
                {
                    string damageDealtString = Hero.Damage(Monster.GetAttackDamage());
                    eventManager.NextEvent = Monster.Name + " attacks you. " + damageDealtString;
                }

                AdvanceTime();
            }
        }

        private void TownPortal(List<string> args)
        {
            if (zone.ZoneType == WorldZoneType.Town)
            {
                eventManager.NextEvent = "There is no need to cast 'Town Portal' at this time. " +
                    "You are already in town.";
            }
            else
            {
                eventManager.NextEvent = "You reach into your pack and take out a dusty blue tome containing " +
                    "the 'Town Portal' spell. You read the words aloud and suddenly a glowing, translucent " +
                    "portal opens up in the air in front of you. Stepping through it, you find yourself " +
                    "back in town.";
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
            eventManager.NextEvent = "You step back from the entrance to " + nextZoneName 
                + ", remaining in " + zone.Name + ".";

            // TO DO: Provide town choices if in town
            currentChoiceText = exploreChoiceText;
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

            eventManager.NextEvent = "A fellow wanderer stumbles upon your lifeless body " +
                "and brings you back to town, where the healers somehow manage to breathe life " +
                "into you once again.";
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

        // Factories
        private ItemFactory itemFactory;

        // Internal data
        private Dictionary<PlayerActionType, ActionFunction> actionFunctions;
        private Random random = new Random();

        // Module references
        WorldEventManager eventManager;
        MonsterManager monsterManager;
        HeroManager heroManager;
        AudioManager audioManager;
    }
}
