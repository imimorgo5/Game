using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public interface IComponent
    {
        int ImageId { get; set; }
        Vector2 Position { get; }
        Rectangle Rectangle { get; }
        string Text { get; }
        ActionType ActionType { get; }
        bool IsHover {  get; set; }
        double Count { get; set; }

        void Update();
    }
}
