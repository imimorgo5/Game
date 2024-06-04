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

        public CollectedObject(Vector2 position, int width, int height, int imageId)
        {
            Position = position;
            Width = width;
            Height = height;
            IsCollected = false;
            ImageNumber = 0;
            ImageId = imageId;
            Collider = new RectangleCollider((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void Update(Vector2 playerSpeed, HashSet<IObject> objects) => Move(Position - playerSpeed);

        public void Move(Vector2 newPos)
        {
            Position = newPos;
            MoveRectangle();
        }

        public void MoveRectangle() => Collider = new RectangleCollider((int)Position.X, (int)Position.Y, Width, Height);
    }
}
