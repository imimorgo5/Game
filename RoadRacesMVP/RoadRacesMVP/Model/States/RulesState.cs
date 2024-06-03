using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class RulesState : State
    {
        public override event EventHandler<GameplayEventArgs> UpdatedState = delegate { };

        public override void Initialize()
        {
            var quitToMenuButton = new Button(new(25, 45), 300, 120, (byte)ObjectTypes.bigButton, ActionType.quitToMenu, "Назад");

            Objects = new();
            Components = new() { quitToMenuButton };
            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }
    }
}

