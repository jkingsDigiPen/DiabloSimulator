using System;
using System.Collections.Generic;
using System.Text;

namespace DiabloSimulator.Game
{
    public enum PlayerActionType
    {
        Look,
        Attack,
        Explore,
        Defend,
        Rest,
        Flee,
        TownPortal,
    }

    public class PlayerAction
    {
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
    }

    public class PlayerChoiceText
    {
        public PlayerChoiceText(string choice01Text_, 
            string choice02Text_, string choice03Text_)
        {
            choice01Text = choice01Text_;
            choice02Text = choice02Text_;
            choice03Text = choice03Text_;
        }

        public string choice01Text;
        public string choice02Text;
        public string choice03Text;
    }

    public class PlayerActionResult
    {
        public PlayerActionResult(string resultText_, PlayerChoiceText choiceText_)
        {
            resultText = resultText_;
            choiceText = choiceText_;
        }

        public PlayerChoiceText choiceText;
        public string resultText;
    }
}
