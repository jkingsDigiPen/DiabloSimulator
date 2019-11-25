//------------------------------------------------------------------------------
//
// File Name:	WorldEventDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DiabloSimulator.Game;

namespace DiabloSimulator.UserControls
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public partial class WorldEventDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public WorldEventDisplay()
        {
            InitializeComponent();

            // Event handlers
            btnExploreAttack.Click += btnExploreAttack_Click;
            btnDefend.Click += btnDefend_Click;

            // Misc setup
            Turns = 0;
            PopulateEvents();
        }

        // The number of turns taken so far in the game
        public uint Turns
        {
            get; set;
        }

        // Allows events to reach other parts of UI
        public static readonly RoutedEvent MonsterChangedEvent =
            EventManager.RegisterRoutedEvent("MonsterChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<string>), typeof(Control));

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void clMonsterChanged(object sender, RoutedEventArgs e)
        {

        }

        private void PopulateEvents()
        {
            AddWorldEvent("Welcome to Sanctuary!");
            AddWorldEvent("You are in the town of Tristram, a place of relative safety.");
        }

        private void btnExploreAttack_Click(object sender, RoutedEventArgs e)
        {
            // TO DO: Remove this
            //View.HeroStats.Level = View.HeroStats.Level + 1;

            if(View.InCombat())
            {
                float damageDealt = View.GetHeroAttackDamage()[0].amount;
                string damageDealtString = View.DamageMonster(damageDealt);
                AddWorldEvent("You attack the " + View.MonsterType + ". " + damageDealtString);

                if (!View.IsMonsterNullOrDead())
                {
                    damageDealtString = View.DamageHero(View.GetMonsterAttackDamage());
                    AddWorldEvent(View.MonsterName + " attacks you. " + damageDealtString);
                }

                AdvanceTime();
            }
            else if(!View.IsHeroDead())
            {
                Turns = 0;
                AddWorldEvent(View.GenerateMonster());
                RaiseEvent(new RoutedEventArgs(MonsterChangedEvent));
            }
        }

        private void btnDefend_Click(object sender, RoutedEventArgs e)
        {

            if (View.InCombat())
            {
                // TO DO: Add bonus dodge chance
                AddWorldEvent("You steel yourself, waiting for your enemy to attack.");

                if (!View.IsMonsterNullOrDead())
                {
                    string damageDealtString = View.DamageHero(View.GetMonsterAttackDamage());
                    AddWorldEvent(View.MonsterName + " attacks you. " + damageDealtString);
                }

                // TO DO: Remove bonus dodge chance
            }
            else if(!View.IsHeroDead())
            {
                // Add regen - additive and multiplicative
                StatModifier regenMultBonus = new StatModifier("HealthRegen",
                    "Rest", Game.ModifierType.Multiplicative, 0.5f);
                StatModifier regenAddBonus = new StatModifier("HealthRegen",
                    "Rest", Game.ModifierType.Additive, 2);

                View.HeroStats.AddModifier(regenMultBonus);
                View.HeroStats.AddModifier(regenAddBonus);
                AddWorldEvent("You rest for a short while. You feel healthier!");

                // Remove temporary regen
                View.HeroStats.RemoveModifier(regenMultBonus);
                View.HeroStats.RemoveModifier(regenAddBonus);
            }

            AdvanceTime();
        }

        private void AddWorldEvent(string worldEvent)
        {
            lvEvents.Items.Add(worldEvent);

            // FIX THIS DAMN SCROLLING
            if (VisualTreeHelper.GetChildrenCount(lvEvents) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(lvEvents, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
        }

        private void AdvanceTime()
        {
            // Check for player death
            if (!View.IsHeroDead())
            {
                HeroLifeRegen();

                if(View.InCombat())
                {
                    ++Turns;
                    AddWorldEvent("A round of combat ends. (Round " + Turns + ")");
                }
            }
            else
            {
                GameOver();
            }
        }

        private void HeroLifeRegen()
        {
            float lifeRegenAmount = View.HeroStats.ModifiedValues["HealthRegen"];
            if (lifeRegenAmount != 0)
            {
                AddWorldEvent(View.HealHero(lifeRegenAmount) + " from natural healing.");
            }
        }

        private void GameOver()
        {
            MessageBox.Show("You have died. You will be revived in town.");
            AddWorldEvent("You are in the town of Tristram, a place of relative safety.");
            View.ReviveHero();
        }

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }
    }
}
