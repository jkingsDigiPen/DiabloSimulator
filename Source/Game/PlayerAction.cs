//------------------------------------------------------------------------------
//
// File Name:	PlayerAction.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public enum PlayerActionType
    {
        Look,
        Attack,
        Explore,
        Defend,
        Rest,
        Flee,
        [Description("Town Portal")]
        TownPortal,
        Proceed,
        Back,
        Yes,
        No,
    }

    public class PlayerAction
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public PlayerAction(PlayerActionType actionType_)
        {
            actionType = actionType_;
            args = new List<string>();
        }

        public PlayerAction(PlayerActionType actionType_, string arg)
        {
            actionType = actionType_;
            args = new List<string>();
            args.Add(arg);
        }

        public PlayerAction(PlayerActionType actionType_, List<string> args_)
        {
            actionType = actionType_;
            args = new List<string>(args_);
        }

        public PlayerActionType actionType;
        public List<string> args;

        //------------------------------------------------------------------------------
        // Public Static Functions:
        //------------------------------------------------------------------------------

        // Get a friendlier string for player action enums
        public static string GetDescription(PlayerActionType value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =

                  (DescriptionAttribute[])fi.GetCustomAttributes(

                  typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }

    public class PlayerChoiceText
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public PlayerChoiceText(string choice01Text_, 
            string choice02Text_, string choice03Text_)
        {
            choice01Text = choice01Text_;
            choice02Text = choice02Text_;
            choice03Text = choice03Text_;
        }

        public string Choice01Text { get => choice01Text; }
        public string Choice02Text { get => choice02Text;}
        public string Choice03Text { get => choice03Text; }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private string choice01Text;
        private string choice02Text;
        private string choice03Text;
    }

    public class PlayerActionResult
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public PlayerActionResult(string resultText_, PlayerChoiceText choiceText_)
        {
            resultText = resultText_;
            choiceText = choiceText_;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public PlayerChoiceText choiceText;
        public string resultText;
    }
}
