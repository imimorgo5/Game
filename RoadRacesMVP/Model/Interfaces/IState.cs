using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public interface IState
    {
        List<IComponent> Components { get; set; }
        event EventHandler<GameplayEventArgs> UpdatedState;

        void Initialize();
        void Update();
    }
}
