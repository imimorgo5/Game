using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class RectangleCollider
    {
        public Rectangle Boundary { get; set; }
        public RectangleCollider(int x, int y, int width, int height)
        {
            Boundary = new Rectangle(x, y, width, height);
        }

        public static bool IsCollided(RectangleCollider r1, RectangleCollider r2)
        {
            return r1.Boundary.Intersects(r2.Boundary);
        }
    }
}
