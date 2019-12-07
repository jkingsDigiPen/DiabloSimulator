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

    public class PlayerAction
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public PlayerAction(GameEvents actionType_)
        {
            actionType = actionType_;
            args = new List<string>();
        }

        public PlayerAction(GameEvents actionType_, string arg)
        {
            actionType = actionType_;
            args = new List<string>();
            args.Add(arg);
        }

        public PlayerAction(GameEvents actionType_, List<string> args_)
        {
            actionType = actionType_;
            args = new List<string>(args_);
        }

        public GameEvents actionType;
        public List<string> args;

        //------------------------------------------------------------------------------
        // Public Static Functions:
        //------------------------------------------------------------------------------

        // Get a friendlier string for player action enums
        public static string GetDescription(GameEvents value)
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
            Choice01Text = choice01Text_;
            Choice02Text = choice02Text_;
            Choice03Text = choice03Text_;
        }

        public string Choice01Text { get; }
        public string Choice02Text { get; }
        public string Choice03Text { get; }
    }

    public class PlayerActionResult
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public PlayerActionResult(List<string> resultText_, PlayerChoiceText choiceText_)
        {
            resultText = resultText_;
            choiceText = choiceText_;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public PlayerChoiceText choiceText;
        public List<string> resultText;
    }
}
