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
}
