using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoadRacesMVP
{
    public class Wall : IObject, ISolid
    {
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; set; }
        public int ImageId { get; set; }

        public Wall(Vector2 position, int width, int length)
        {
            Position = position;
            Collider = new RectangleCollider((int)Position.X, (int)Position.Y, width, length);            
        }

        public void Update(Vector2 offset)
        {
            Move(Position + offset);
        }

        public void Move(Vector2 newPos)
        {
            Position = newPos;
            MoveCollider(Position);
        }

        public void MoveCollider(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)Position.X, (int)Position.Y, Collider.Boundary.Width, Collider.Boundary.Height);
        }
    }
}
