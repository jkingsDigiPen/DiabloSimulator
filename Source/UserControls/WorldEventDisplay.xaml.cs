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
                typeof(RoutedEventHandler), typeof(WorldEventDisplay));

        // Allows events to reach other parts of UI
        public event RoutedEventHandler MonsterChanged
        {
            add
            {
                AddHandler(MonsterChangedEvent, value);
            }
            remove
            {
                RemoveHandler(MonsterChangedEvent, value);
            }
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void PopulateEvents()
        {
            AddWorldEvent("Welcome to Sanctuary!");
            AddWorldEvent("You are in the town of Tristram, a place of relative safety.");
        }

        private void btnExploreAttack_Click(object sender, RoutedEventArgs e)
        {
            if(View.InCombat())
            {
                float damageDealt = View.Hero.GetAttackDamage()[0].amount;
                string damageDealtString = View.DamageMonster(damageDealt);
                AddWorldEvent("You attack the " + View.Monster.Archetype + ". " + damageDealtString);

                if (!View.Monster.IsDead())
                {
                    damageDealtString = View.Hero.Damage(View.Monster.GetAttackDamage());
                    AddWorldEvent(View.Monster.Name + " attacks you. " + damageDealtString);
                }

                AdvanceTime();
            }
            else if(!View.Hero.IsDead())
            {
                Turns = 0;
                AddWorldEvent(View.GenerateMonster());
                // Force monster stat update
                RaiseEvent(new RoutedEventArgs(MonsterChangedEvent));
            }
        }

        private void btnDefend_Click(object sender, RoutedEventArgs e)
        {

            if (View.InCombat())
            {
                // TO DO: Add bonus dodge chance
                AddWorldEvent("You steel yourself, waiting for your enemy to attack.");

                if (!View.Monster.IsDead())
                {
                    string damageDealtString = View.Hero.Damage(View.Monster.GetAttackDamage());
                    AddWorldEvent(View.Monster.Name + " attacks you. " + damageDealtString);
                }

                // TO DO: Remove bonus dodge chance

                AdvanceTime();
            }
            else if (!View.Hero.IsDead())
            {
                // Add regen - additive and multiplicative
                StatModifier regenMultBonus = new StatModifier("HealthRegen",
                    "Rest", Game.ModifierType.Multiplicative, 0.5f);
                StatModifier regenAddBonus = new StatModifier("HealthRegen",
                    "Rest", Game.ModifierType.Additive, 2);

                View.Hero.stats.AddModifier(regenMultBonus);
                View.Hero.stats.AddModifier(regenAddBonus);
                AddWorldEvent("You rest for a short while. You feel healthier!");

                // Step time forward to heal
                AdvanceTime();

                // Remove temporary regen
                View.Hero.stats.RemoveModifier(regenMultBonus);
                View.Hero.stats.RemoveModifier(regenAddBonus);
            }
        }

        private void AddWorldEvent(string worldEvent)
        {
            lvEvents.Items.Add(worldEvent);

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
            if (!View.Hero.IsDead())
            {
                HeroLifeRegen();

                if (View.InCombat())
                {
                    ++Turns;
                    AddWorldEvent("A round of combat ends. (Round " + Turns + ")");
                }

                // Force monster stat update
                RaiseEvent(new RoutedEventArgs(MonsterChangedEvent));
            }
            else
            {
                GameOver();
            } 
        }

        private void HeroLifeRegen()
        {
            float lifeRegenAmount = View.Hero.stats.ModifiedValues["HealthRegen"];
            if (lifeRegenAmount != 0)
            {
                AddWorldEvent(View.Hero.Heal(lifeRegenAmount) + " from natural healing.");
            }
        }

        private void GameOver()
        {
            MessageBox.Show("You have died. You will be revived in town.");
            AddWorldEvent("You are in the town of Tristram, a place of relative safety.");
            View.Hero.Revive();
            View.KillMonster();

            // Force monster stat update
            RaiseEvent(new RoutedEventArgs(MonsterChangedEvent));
        }

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }
    }
}
