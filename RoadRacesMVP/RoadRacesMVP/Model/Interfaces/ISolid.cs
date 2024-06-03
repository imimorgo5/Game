using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public interface ISolid
    {
        bool IsColision { get; }
        RectangleCollider Collider { get; }
        void MoveCollider();
    }
}
