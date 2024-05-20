using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class Player : IObject, ISolid
    {
        private Vector2 speed;
        public Vector2 Position { get; private set; }
        public RectangleCollider Collider { get; private set; }
        public int ImageId { get; set; }
        public int ImageNumber {  get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsColision { get; private set; }
        public bool IsReverseSteering { get; private set; }
        private DateTime OffColisionTime { get; set; }
        private DateTime BrokeSteeringTime { get; set; }
        private bool InCollision {  get; set; }
        public int CollectedCoinsCount { get; set; }
        public int CollisionCount { get; set; }
        private HashSet<IObject> GameObjects { get; set; }


        public Player(Vector2 position, int width, int height)
        {
            Position = position;
            Height = height;
            Width = width;
            ImageNumber = 0;
            Collider = new((int)Position.X, (int)Position.Y, Width, Height);
            IsColision = true;
            IsReverseSteering = false;
            CollectedCoinsCount = 0;
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

        public void Update(Vector2 offset, HashSet<IObject> objects)
        {
            GameObjects = objects;
            ContactWithCollectedObject();
            Move(Position + Speed + offset);
            Speed = new(0, Speed.Y);

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
            Collider = new((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void ContactWithCollectedObject()
        {
            InCollision = false;
            foreach (var obj in GameObjects)
            {
                if (obj.ImageId != ImageId && RectangleCollider.IsCollided(obj.Collider, Collider))
                    InCollision = true;

                if (obj is CollectedObject collectedObj)
                {
                    if (RectangleCollider.IsCollided(collectedObj.Collider, Collider))
                    {
                        collectedObj.IsCollected = true;

                        if (collectedObj is Coin)
                            CollectedCoinsCount++;
                        else if (collectedObj is Braker)
                            speed.Y /= 2;
                        else if (collectedObj is OffColisionObj)
                            BecomeNotCollision();
                        else if (collectedObj is BrokeSteeringObj)
                        {
                            IsReverseSteering = true;
                            BrokeSteeringTime = DateTime.Now;
                        }
                        else if (collectedObj is Bomb)
                            CollisionCount++;

                        break;
                    }
                }
            }

            if ((DateTime.Now - BrokeSteeringTime).TotalSeconds > 5)
                IsReverseSteering = false;

            if ((DateTime.Now - OffColisionTime).TotalSeconds > 5 && !InCollision)
                IsColision = true;
        }

        public void BecomeNotCollision()
        {
            IsColision = false;
            OffColisionTime = DateTime.Now;
        }
    }
}
