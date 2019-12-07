//------------------------------------------------------------------------------
//
// File Name:	GameManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game.World;
using System;
using System.Collections.Generic;
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

        public override void Inintialize()
        {
            worldEventManager = EngineCore.GetModule<WorldEventManager>();
            monsterManager = EngineCore.GetModule<MonsterManager>();
            heroManager = EngineCore.GetModule<HeroManager>();
            audioManager = EngineCore.GetModule<AudioManager>();
            zoneManager = EngineCore.GetModule<ZoneManager>();

            // Load audio shtuff
            audioManager.LoadBank("Master.strings");
            audioManager.LoadBank("Master");
            audioManager.LoadBank("Music");

            // Register for events
            AddEventHandler(PlayerActionType.Attack.ToString(), OnPlayerAttack);
            AddEventHandler(PlayerActionType.Defend.ToString(), OnPlayerDefend);
            AddEventHandler(PlayerActionType.Rest.ToString(), OnPlayerRest);
            AddEventHandler(PlayerActionType.Flee.ToString(), OnPlayerFlee);

            AddEventHandler(PlayerActionType.TownPortal.ToString(), OnPlayerTownPortal);
            AddEventHandler(PlayerActionType.Proceed.ToString(), OnPlayerProceed);
            AddEventHandler(PlayerActionType.Back.ToString(), OnPlayerBack);
        }

        #endregion

        #region gameFunctions

        public PlayerActionResult GetActionResult(PlayerAction action)
        {
            // Execute action
            ActionFunction actionFunction = null;
            if(actionFunctions.TryGetValue(action.actionType, out actionFunction))
            {
                actionFunction(action.args);
            }

            // Raise action event
            RaiseGameEvent(new GameEventArgs(action.actionType.ToString()));

            // Return output
            string result = worldEventManager.NextEvent;

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

        public int Turns { get; set; }

        public PlayerChoiceText CurrentChoiceText { get; set; }

        #endregion

        //------------------------------------------------------------------------------
        // Private Structures:
        //------------------------------------------------------------------------------

        private delegate void ActionFunction(List<string> args);

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        #region playerActions

        private void OnPlayerAttack(object sender, GameEventArgs e)
        {
            AdvanceTime();
        }

        private void OnPlayerDefend(object sender, GameEventArgs e)
        {
            // TO DO: Add additive bonus dodge chance, mult bonus to block chance
            worldEventManager.NextEvent = "You steel yourself, waiting for your enemy to attack.";

            if (!monsterManager.Monster.IsDead())
            {
                string damageDealtString = heroManager.Hero.Damage(monsterManager.Monster.GetAttackDamage());
                worldEventManager.NextEvent = monsterManager.Monster.Name + " attacks you. " + damageDealtString;
            }

            // TO DO: Remove bonus dodge chance, block chance

            AdvanceTime();
        }

        private void OnPlayerRest(object sender, GameEventArgs e)
        {
            // Add regen - additive and multiplicative
            StatModifier regenMultBonus = new StatModifier("HealthRegen",
                "Rest", Game.ModifierType.Multiplicative, 0.5f);
            StatModifier regenAddBonus = new StatModifier("HealthRegen",
                "Rest", Game.ModifierType.Additive, 2);

            heroManager.Hero.Stats.AddModifier(regenMultBonus);
            heroManager.Hero.Stats.AddModifier(regenAddBonus);
            worldEventManager.NextEvent = "You rest for a short while. You feel healthier!";

            // Step time forward to heal
            AdvanceTime();

            // Remove temporary regen
            heroManager.Hero.Stats.RemoveModifier(regenMultBonus);
            heroManager.Hero.Stats.RemoveModifier(regenAddBonus);
        }

        private void OnPlayerFlee(object sender, GameEventArgs e)
        {
            worldEventManager.NextEvent = "You attempt to flee from the " + monsterManager.Monster.Race + "...";

            // 60% flee chance
            bool fleeSuccess = random.NextDouble() <= 0.6f;
            if(fleeSuccess)
            {
                worldEventManager.NextEvent = "You have successfully escaped from " + monsterManager.Monster.Name + ".";
                monsterManager.DestroyMonster();
                RaiseGameEvent(new GameEventArgs("PlayerLook"));
            }
            else
            {
                worldEventManager.NextEvent = "You can't seem to find an opening to escape! " +
                    "You are locked in combat with " + monsterManager.Monster.Name + ".";

                if (!monsterManager.Monster.IsDead())
                {
                    string damageDealtString = heroManager.Hero.Damage(monsterManager.Monster.GetAttackDamage());
                    worldEventManager.NextEvent = monsterManager.Monster.Name + " attacks you. " + damageDealtString;
                }

                AdvanceTime();
            }
        }

        private void OnPlayerTownPortal(object sender, GameEventArgs e)
        {
            if (zoneManager.CurrentZone.ZoneType == WorldZoneType.Town)
            {
                worldEventManager.NextEvent = "There is no need to cast 'Town Portal' at this time. " +
                    "You are already in town.";
            }
            else
            {
                worldEventManager.NextEvent = "You reach into your pack and take out a dusty blue tome containing " +
                    "the 'Town Portal' spell. You read the words aloud and suddenly a glowing, translucent " +
                    "portal opens up in the air in front of you. Stepping through it, you find yourself " +
                    "back in town.";
                zoneManager.SetZone("Tristram");
                RaiseGameEvent(new GameEventArgs("PlayerLook"));
            }
        }

        private void OnPlayerProceed(object sender, GameEventArgs e)
        {
            zoneManager.AdvanceToNextZone();
            RaiseGameEvent(new GameEventArgs("PlayerLook"));
        }

        private void OnPlayerBack(object sender, GameEventArgs e)
        {
            worldEventManager.NextEvent = "You step back from the entrance to " + zoneManager.NextZoneName 
                + ", remaining in " + zoneManager.CurrentZone.Name + ".";

            // TO DO: Provide town choices if in town
            CurrentChoiceText = exploreChoiceText;
        }

        #endregion

        #region gameStateFunctions

        private void AdvanceTime()
        {
            bool heroDead = heroManager.Hero.IsDead();
            bool monsterDead = monsterManager.Monster.IsDead();

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
                ++Turns;
                //nextEvent.WriteLine("A round of combat ends. (Round " + Turns + ")");
            }

            if (!heroDead)
            {
                heroManager.HeroLifeRegen();
            }
        }

        private void GameOver()
        {
            MessageBox.Show("You have died. You will be revived in town.", 
                "Diablo Simulator", MessageBoxButton.OK, MessageBoxImage.Information);

            heroManager.Hero.Revive();
            monsterManager.DestroyMonster();

            worldEventManager.NextEvent = "A fellow wanderer stumbles upon your lifeless body " +
                "and brings you back to town, where the healers somehow manage to breathe life " +
                "into you once again.";
            zoneManager.SetZone("Tristram");
            RaiseGameEvent(new GameEventArgs("PlayerLook"));
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

        // Internal data
        private Dictionary<PlayerActionType, ActionFunction> actionFunctions 
            = new Dictionary<PlayerActionType, ActionFunction>();
        private Random random = new Random();

        // Module references
        WorldEventManager worldEventManager;
        MonsterManager monsterManager;
        HeroManager heroManager;
        AudioManager audioManager;
        ZoneManager zoneManager;
    }
}
