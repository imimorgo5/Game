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
        private double MaxSpeed;
        public Vector2 Position { get; private set; }
        public RectangleCollider Collider { get; private set; }
        public int ImageId { get; set; }
        public int ImageNumber { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsColision { get; private set; }
        private HashSet<IObject> GameObjects { get; set; }

        public Car(Vector2 position, int width, int height, Vector2 speed, double maxSpeed)
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

                if (value.Y <= MaxSpeed)
                    speed.Y = (float)MaxSpeed;
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
            MaxSpeed = -15 - 2 * GameSettings.Difficult;
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
            var speedAdd = Vector2.Zero;
            var isLeftObstacle = false;
            var isRightObstacle = false;
            var isCarAhead = false;
            var distanceBetweenCar = GameState.TileSize * 5;
            var distanceToWall = GameState.TileSize * 2;
            foreach (var obj in GameObjects)
            {
                if (obj is not ISolid || obj == this)
                    continue;

                if (obj is Car || obj is Player)
                {
                    var offsetPosition = Position - obj.Position;
                    if (offsetPosition.Y < 500 && Math.Abs(offsetPosition.X) < distanceBetweenCar && Speed.Y < obj.Speed.Y)
                        isCarAhead = true;

                    if (Math.Abs(offsetPosition.Y) < 300 && Math.Abs(offsetPosition.X) == distanceBetweenCar)
                    {
                        if (offsetPosition.X > 0)
                            isLeftObstacle = true;
                        else
                            isRightObstacle = true;
                    }
                }
                else if (obj is Wall)
                {
                    var offsetPosition = Position - obj.Position;
                    if (Math.Abs(offsetPosition.X) == distanceToWall)
                    {
                        if (offsetPosition.X > 0)
                            isLeftObstacle = true;
                        else
                            isRightObstacle = true;
                    }
                }
            }

            if (isCarAhead)
            {
                if (isLeftObstacle && !isRightObstacle)
                    speedAdd = new Vector2(5, 0);
                else if (!isLeftObstacle && isRightObstacle)
                    speedAdd = new Vector2(-5, 0);
                else if (isRightObstacle && isLeftObstacle)
                    speedAdd = new Vector2(0, 1);
                else if (!isLeftObstacle && !isRightObstacle)
                    speedAdd = new Vector2(-5, 0);
            }

            Speed += speedAdd;
        }
    }
}
