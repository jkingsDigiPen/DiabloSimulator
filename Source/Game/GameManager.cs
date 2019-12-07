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

            // Register for player actions
            AddEventHandler(GameEvents.PlayerAttack, OnPlayerAttack);
            AddEventHandler(GameEvents.PlayerDefend, OnPlayerDefend);
            AddEventHandler(GameEvents.PlayerRest, OnPlayerRest);
            AddEventHandler(GameEvents.PlayerFlee, OnPlayerFlee);
            AddEventHandler(GameEvents.PlayerTownPortal, OnPlayerTownPortal);
            AddEventHandler(GameEvents.PlayerProceed, OnPlayerProceed);
            AddEventHandler(GameEvents.PlayerBack, OnPlayerBack);

            // Register for monster/hero actions
            AddEventHandler(GameEvents.MonsterDead, OnMonsterDead);
            AddEventHandler(GameEvents.HeroDead, OnHeroDead);
            AddEventHandler(GameEvents.AdvanceTime, OnAdvanceTime);
        }

        #endregion

        #region gameFunctions

        public PlayerActionResult GetActionResult(PlayerAction action)
        {
            // Raise action event
            RaiseGameEvent(action.actionType);

            // Return output
            var result = worldEventManager.WorldEventsText;

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
        // Private Functions:
        //------------------------------------------------------------------------------

        #region playerActions

        private void OnPlayerAttack(object sender, GameEventArgs e)
        {
            RaiseGameEvent(GameEvents.AdvanceTime);
        }

        private void OnPlayerDefend(object sender, GameEventArgs e)
        {
            // TO DO: Add additive bonus dodge chance, mult bonus to block chance
            RaiseGameEvent(GameEvents.WorldEventText, this,
                "You steel yourself, waiting for your enemy to attack.");

            if (!monsterManager.Monster.IsDead)
            {
                string damageDealtString = heroManager.Hero.Damage(monsterManager.Monster.GetAttackDamage());
                RaiseGameEvent(GameEvents.WorldEventText, this, monsterManager.Monster.Name 
                    + " attacks you. " + damageDealtString);
            }

            // TO DO: Remove bonus dodge chance, block chance

            RaiseGameEvent(GameEvents.AdvanceTime);
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
            RaiseGameEvent(GameEvents.WorldEventText, this, "You rest for a short while. You feel healthier!");

            // Step time forward to heal
            RaiseGameEvent(GameEvents.AdvanceTime);

            // Remove temporary regen
            heroManager.Hero.Stats.RemoveModifier(regenMultBonus);
            heroManager.Hero.Stats.RemoveModifier(regenAddBonus);
        }

        private void OnPlayerFlee(object sender, GameEventArgs e)
        {
            RaiseGameEvent(GameEvents.WorldEventText, this, 
                "You attempt to flee from the " + monsterManager.Monster.Race + "...");

            // 60% flee chance
            bool fleeSuccess = random.NextDouble() <= 0.6f;
            if(fleeSuccess)
            {
                RaiseGameEvent(GameEvents.WorldEventText, this, 
                    "You have successfully escaped from " + monsterManager.Monster.Name + ".");
                monsterManager.DestroyMonster();
                RaiseGameEvent(GameEvents.PlayerLook);
            }
            else
            {
                RaiseGameEvent(GameEvents.WorldEventText, this, "You can't seem to find an opening to escape! " +
                    "You are locked in combat with " + monsterManager.Monster.Name + ".");

                if (!monsterManager.Monster.IsDead)
                {
                    string damageDealtString = heroManager.Hero.Damage(monsterManager.Monster.GetAttackDamage());
                    RaiseGameEvent(GameEvents.WorldEventText, this, 
                        monsterManager.Monster.Name + " attacks you. " + damageDealtString);
                }

                RaiseGameEvent(GameEvents.AdvanceTime);
            }
        }

        private void OnPlayerTownPortal(object sender, GameEventArgs e)
        {
            if (zoneManager.CurrentZone.ZoneType == WorldZoneType.Town)
            {
                RaiseGameEvent(GameEvents.WorldEventText, this, 
                    "There is no need to cast 'Town Portal' at this time. " +
                    "You are already in town.");
            }
            else
            {
                RaiseGameEvent(GameEvents.WorldEventText, this, "You reach into your pack and take out " +
                    "a dusty blue tome containing the 'Town Portal' spell. You read the words aloud " +
                    "and suddenly a glowing, translucent portal opens up in the air in front of you. " +
                    "Stepping through it, you find yourself back in town.");
                zoneManager.SetZone("Tristram");
                RaiseGameEvent(GameEvents.PlayerLook);
            }
        }

        private void OnPlayerProceed(object sender, GameEventArgs e)
        {
            zoneManager.AdvanceToNextZone();
            RaiseGameEvent(GameEvents.PlayerLook);
        }

        private void OnPlayerBack(object sender, GameEventArgs e)
        {
            RaiseGameEvent(GameEvents.WorldEventText, this, "You step back from the entrance to " 
                + zoneManager.NextZoneName + ", remaining in " + zoneManager.CurrentZone.Name + ".");

            // TO DO: Provide town choices if in town
            CurrentChoiceText = exploreChoiceText;
        }

        #endregion

        #region monsterAndHeroActions

        private void OnMonsterDead(object sender, GameEventArgs e)
        {
            InCombat = false;
        }

        private void OnHeroDead(object sender, GameEventArgs e)
        {
            InCombat = false;
            GameOver();
        }

        #endregion

        #region gameStateFunctions

        private void OnAdvanceTime(object sender, GameEventArgs e)
        {
            if(InCombat) ++Turns;
        }

        private void GameOver()
        {
            MessageBox.Show("You have died. You will be revived in town.", 
                "Diablo Simulator", MessageBoxButton.OK, MessageBoxImage.Information);

            heroManager.Hero.Revive();
            monsterManager.DestroyMonster();

            RaiseGameEvent(GameEvents.WorldEventText, this, "A fellow wanderer stumbles upon your lifeless body " +
                "and brings you back to town, where the healers somehow manage to breathe life " +
                "into you once again.");
            zoneManager.SetZone("Tristram");
            RaiseGameEvent(GameEvents.PlayerLook);
        }

        #endregion

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        // Pseudo-constants
        public static PlayerChoiceText exploreChoiceText =
            new PlayerChoiceText(PlayerAction.GetDescription(GameEvents.PlayerExplore),
                PlayerAction.GetDescription(GameEvents.PlayerRest), 
                PlayerAction.GetDescription(GameEvents.PlayerTownPortal));

        public static PlayerChoiceText combatChoiceText =
            new PlayerChoiceText(PlayerAction.GetDescription(GameEvents.PlayerAttack),
                PlayerAction.GetDescription(GameEvents.PlayerDefend), PlayerAction.GetDescription(GameEvents.PlayerFlee));

        public static PlayerChoiceText discoverChoiceText =
            new PlayerChoiceText(PlayerAction.GetDescription(GameEvents.PlayerProceed),
                PlayerAction.GetDescription(GameEvents.PlayerBack), 
                PlayerAction.GetDescription(GameEvents.PlayerTownPortal));

        public static PlayerChoiceText yesNoChoiceText =
            new PlayerChoiceText(PlayerAction.GetDescription(GameEvents.PlayerYes),
                PlayerAction.GetDescription(GameEvents.PlayerNo), PlayerAction.GetDescription(GameEvents.PlayerBack));

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Game state
        private bool inCombat;

        // Internal data
        private Random random = new Random();

        // Module references
        WorldEventManager worldEventManager;
        MonsterManager monsterManager;
        HeroManager heroManager;
        AudioManager audioManager;
        ZoneManager zoneManager;
    }
}
