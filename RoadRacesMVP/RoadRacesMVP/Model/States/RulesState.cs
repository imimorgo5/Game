using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class RulesState : IState
    {
        public List<IComponent> Components { get; private set; }
        public Dictionary<int, IObject> Objects { get; private set; }

        public event EventHandler<GameplayEventArgs> UpdatedState = delegate { };


        public void Initialize()
        {
            var quitToMenuButton = new Button(new(25, 45), 300, 120, (byte)ObjectTypes.bigButton, ActionType.quitToMenu, "Назад");

            Objects = new();
            Components = new() { quitToMenuButton };
            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }

        public void Update()
        {
            foreach (var component in Components)
                component.Update();

            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }
    }
}

