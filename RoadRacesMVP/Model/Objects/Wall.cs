using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoadRacesMVP
{
    public class Wall : IObject, ISolid
    {
        public Vector2 Position { get; private set; }
        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; private set; }
        public int ImageId { get; set; }
        public int ImageNumber { get; set; }
        public  int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsColision { get; private set; }

        public Wall(Vector2 position, int width, int height)
        {
            Position = position;
            Height = height;
            Width = width;
            ImageNumber = 0;
            Collider = new((int)Position.X, (int)Position.Y, Width, Height);      
            IsColision = true;
        }

        public void Update(Vector2 offset, HashSet<IObject> objects) => Move(Position + offset);

        public void Move(Vector2 newPos)
        {
            Position = newPos;
            MoveCollider(Position);
        }

        public void MoveCollider(Vector2 newPos)
        {
            Collider = new((int)Position.X, (int)Position.Y, Width, Height);
        }
    }
}
