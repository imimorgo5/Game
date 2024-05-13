using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class Player : IObject, ISolid
    {
        protected Vector2 speed;
        public Vector2 Position { get; set; }
        public RectangleCollider Collider { get; set; }
        public int ImageId { get; set; }

        public Player(Vector2 position)
        {
            Position = position;
            Collider = new RectangleCollider((int)Position.X, (int)Position.Y, 135, 234); //Исправить магические числа!!!
        }

        public Vector2 Speed
        {
            get => speed;

            set
            {
                speed = value;

                if (value.Y <= -40)
                    speed.Y = -40;
            }
        }

        public void Update(Vector2 offset)
        {
            Move(Position + Speed + offset);
            Speed = new Vector2(0, Speed.Y);

            if (Speed.Y > -10)
                Speed += new Vector2(0, -0.1f);
            else
                Speed += new Vector2(0, -0.01f);
        }

        public void Move(Vector2 newPos)
        {
            Position = newPos;
            MoveCollider(Position);
        }

        public void MoveCollider(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)Position.X, (int)Position.Y, 135, 234); //Исправить магические числа!!!
        }
    }
}
