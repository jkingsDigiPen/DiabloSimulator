//------------------------------------------------------------------------------
//
// File Name:	GameManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Game.Factories;
using System.Collections.Generic;
using System.ComponentModel;
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
            // Actors
            hero = new Hero("The Vagabond");
            monster = new Monster();

            // Factories
            heroFactory = new HeroFactory();
            monsterFactory = new MonsterFactory();
            itemFactory = new ItemFactory();

            // Internal data
            nextEvent = new StringWriter();

            // Action delegates
            actionFunctions = new Dictionary<PlayerActionType, ActionFunction>();
            actionFunctions[PlayerActionType.Look] = Look;
            actionFunctions[PlayerActionType.Attack] = Attack;
            actionFunctions[PlayerActionType.Defend] = Defend;
            actionFunctions[PlayerActionType.Explore] = Explore;
            actionFunctions[PlayerActionType.Rest] = Rest;
            //actionFunctions[PlayerActionType.Flee] = Flee;
            //actionFunctions[PlayerActionType.TownPortal] = TownPortal;
        }

        #endregion

        #region gameStateFunctions

        public string GetActionResult(PlayerAction action)
        {
            // Execute action
            actionFunctions[action.actionType](action.args);

            // Return output
            string result = nextEvent.ToString();
            nextEvent.GetStringBuilder().Clear();
            return result;
        }

        public Hero Hero { get => hero; }

        public Monster Monster { get => monster; }

        public bool InCombat { get; set; }

        public int Turns { get => turns; }

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
            // TO DO: Provide location specific text

            nextEvent.WriteLine("Welcome to the world of Sanctuary!");
            nextEvent.WriteLine("You are in the town of Tristram, a place of relative safety.");
        }

        private void Explore(List<string> args)
        {
            turns = 0;
            nextEvent.WriteLine(CreateMonster());
            InCombat = true;
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
                hero.Stats["Experience"] = hero.Stats.BaseValues["Experience"] + Monster.Stats.ModifiedValues["Experience"];
            }

            AdvanceTime();
        }

        private void Defend(List<string> args)
        {
            // TO DO: Add bonus dodge chance
            nextEvent.WriteLine("You steel yourself, waiting for your enemy to attack.");

            if (!Monster.IsDead())
            {
                string damageDealtString = Hero.Damage(Monster.GetAttackDamage());
                nextEvent.WriteLine(Monster.Name + " attacks you. " + damageDealtString);
            }

            // TO DO: Remove bonus dodge chance

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

        #endregion

        #region heroFunctions

        public void CreateHero()
        {
            hero = heroFactory.Create(Hero);
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

        public string CreateMonster()
        {
            monster = null;
            monster = monsterFactory.Create(Hero);
            return Monster.Name + " (a level "
                + Monster.Stats.Level + " " + Monster.Race + ") appeared!";
        }

        public void DestroyMonster()
        {
            Monster.Kill();
            monster = new Monster();
        }

        public string DamageMonster(float amount)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(amount));
            return Monster.Damage(damageList);
        }

        #endregion

        #region itemFunctions

        public Item CreateItem(string name = "random")
        {
            if (name == "random")
            {
                return itemFactory.Create(hero);
            }
            else
            {
                return itemFactory.Create(hero, name);
            }
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
            MessageBox.Show("You have died. You will be revived in town.");
            nextEvent.WriteLine("You are in the town of Tristram, a place of relative safety.");
            Hero.Revive();
            DestroyMonster();

            // Force monster stat update
            //RaiseEvent(new RoutedEventArgs(MonsterChangedEvent));
        }

        #endregion

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Game state
        private int turns;
        private StringWriter nextEvent;

        // Actors
        private Monster monster;
        private Hero hero;

        // Factories
        private MonsterFactory monsterFactory;
        private HeroFactory heroFactory;
        private ItemFactory itemFactory;

        // Internal data
        private Dictionary<PlayerActionType, ActionFunction> actionFunctions;
    }
}
