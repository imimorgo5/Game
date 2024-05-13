using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public interface IGameplayModel
    {
        int PlayerId { get; set; }
        Dictionary<int, IObject> Objects { get; set; }
        event EventHandler<GameplayEventArgs> Updated;

        void Update();
        void ChangePlayerSpeed(Direction direction);
        void Initialize();
    }
}
