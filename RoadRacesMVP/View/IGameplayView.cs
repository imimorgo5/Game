using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public interface IGameplayView
    {
        StateView CurrentStateView { get; set; }

        event EventHandler CycleFinished;

        void ChangeStateView(ViewType viewType);
        void QuitGame();
        void Run();
    }
}
