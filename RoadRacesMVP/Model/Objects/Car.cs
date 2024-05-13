using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class Car : IObject, ISolid
    {
        protected Vector2 speed;
        public Vector2 Position { get; set; }
        public RectangleCollider Collider { get; set; }
        public int ImageId { get; set; }

        public Car(Vector2 position)
        {
            Position = position;
            Collider = new RectangleCollider((int)Position.X, (int)Position.Y, 135, 234); //Исправить магические числа!!!            
        }

        public Vector2 Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        public void MoveCollider(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)Position.X, (int)Position.Y, 135, 234); //Исправить магические числа!!!
        }

        public void Update(Vector2 offset)
        {
            Move(Position + Speed + offset);
            Speed = new Vector2(0, Speed.Y);
        }

        public void Move(Vector2 newPos)
        {
            Position = newPos;
            MoveCollider(Position);
        }
    }
}
