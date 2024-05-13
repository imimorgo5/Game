using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public interface ISolid
    {
        RectangleCollider Collider { get; set; }
        void MoveCollider(Vector2 newPos);
    }
}
