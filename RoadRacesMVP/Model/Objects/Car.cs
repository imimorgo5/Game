using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class Car : IObject, ISolid
    {
        private Vector2 speed;
        private Vector2 MaxSpeed;
        public Vector2 Position { get; private set; }
        public RectangleCollider Collider { get; private set; }
        public int ImageId { get; set; }
        public int ImageNumber { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsColision { get; private set; }
        private HashSet<IObject> GameObjects { get; set; }

        public Car(Vector2 position, int width, int height, Vector2 speed, Vector2 maxSpeed)
        {
            Position = position;
            Height = height;
            Width = width;
            Collider = new((int)Position.X, (int)Position.Y, Width, Height);
            MaxSpeed = maxSpeed;
            Speed = speed;
            IsColision = true;
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

                if (value.Y <= MaxSpeed.Y)
                    speed.Y = MaxSpeed.Y;
            }
        }

        public void MoveCollider(Vector2 newPos)
        {
            Collider = new((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void Update(Vector2 offset, HashSet<IObject> objects)
        {
            GameObjects = objects;
            //MoveIfCarAhead();
            Move(Position + Speed + offset);
            Speed = new(0, Speed.Y);

            if (Speed.Y > -10)
                Speed += new Vector2(0, -0.05f);
            else
                Speed += new Vector2(0, -0.005f);
        }

        public void Move(Vector2 newPos)
        {
            Position = newPos;
            MoveCollider(Position);
        }

        private void MoveIfCarAhead()
        {

        }
    }
}
