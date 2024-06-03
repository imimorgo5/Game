using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class CollectedObject
    {
        public int ImageId { get; set; }
        public int ImageNumber { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Speed { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsCollected { get; set; }
        public RectangleCollider Collider { get; private set; }

        public CollectedObject(Vector2 position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;
            IsCollected = false;
            ImageNumber = 0;
            Collider = new RectangleCollider((int)Position.X, (int)Position.Y, Width, Height);
        }
        public void Update(Vector2 offset, HashSet<IObject> objects)
        {
            Move(Position + offset);
        }

        public void Move(Vector2 pos)
        {
            Position = pos;
            MoveRectangle(Position);
        }

        public void MoveRectangle(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)Position.X, (int)Position.Y, Width, Height);
        }
    }
}
