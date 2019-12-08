//------------------------------------------------------------------------------
//
// File Name:	GameManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;

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
            heroManager = EngineCore.GetModule<HeroManager>();

            // Register for player actions
            AddEventHandler(GameEvents.PlayerAttack, OnPlayerAttack);
            AddEventHandler(GameEvents.PlayerRest, OnPlayerRest);
            AddEventHandler(GameEvents.PlayerProceed, OnPlayerProceed);
            AddEventHandler(GameEvents.PlayerBack, OnPlayerBack);

            // Register for monster/hero actions
            AddEventHandler(GameEvents.MonsterDead, OnMonsterDead);
            AddEventHandler(GameEvents.HeroDead, OnHeroDead);
            AddEventHandler(GameEvents.AdvanceTime, OnAdvanceTime);

            // Load audio shtuff
            RaiseGameEvent(GameEvents.LoadAudioBank, this, "Master.strings");
            RaiseGameEvent(GameEvents.LoadAudioBank, this, "Master");
            RaiseGameEvent(GameEvents.LoadAudioBank, this, "Music");
        }

        #endregion

        #region gameFunctions

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

        public PlayerChoiceText CurrentChoiceText { get; set; } = exploreChoiceText;

        #endregion

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        #region playerActions

        private void OnPlayerAttack(object sender, GameEventArgs e)
        {
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
            RaiseGameEvent(GameEvents.AddWorldEventText, this, "You rest for a short while. You feel healthier!");

            // Step time forward to heal
            RaiseGameEvent(GameEvents.AdvanceTime);

            // Remove temporary regen
            heroManager.Hero.Stats.RemoveModifier(regenMultBonus);
            heroManager.Hero.Stats.RemoveModifier(regenAddBonus);
        }

        private void OnPlayerProceed(object sender, GameEventArgs e)
        {
            // TO DO: Provide town choices if in town
            CurrentChoiceText = exploreChoiceText;
        }

        private void OnPlayerBack(object sender, GameEventArgs e)
        {
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
        }

        #endregion

        #region gameStateFunctions

        private void OnAdvanceTime(object sender, GameEventArgs e)
        {
            if(InCombat) ++Turns;
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

        // Module references
        HeroManager heroManager;
    }
}
