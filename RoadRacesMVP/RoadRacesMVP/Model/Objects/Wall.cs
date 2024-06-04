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
        public int ImageNumber { get; private set; }
        public  int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsColision { get; private set; }

        public Wall(Vector2 position, int width, int height, int imageId)
        {
            Position = position;
            Height = height;
            Width = width;
            ImageId = imageId;
            ImageNumber = 0;
            Collider = new((int)Position.X, (int)Position.Y, Width, Height);      
            IsColision = true;
        }

        public void Update(Vector2 playerSpeed, HashSet<IObject> objects) => Move(Position - playerSpeed);

        public void Move(Vector2 newPos)
        {
            Position = newPos;
            MoveCollider();
        }

        public void MoveCollider() => Collider = new((int)Position.X, (int)Position.Y, Width, Height);
    }
}
