using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public abstract class State
    {
        public List<IComponent> Components { get; protected set; }
        public Dictionary<int, IObject> Objects { get; protected set; }
        public virtual event EventHandler<GameplayEventArgs> UpdatedState = delegate { };

        public abstract void Initialize();

        public virtual void Update()
        {
            foreach (var component in Components)
                component.Update();

            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }
    }
}
