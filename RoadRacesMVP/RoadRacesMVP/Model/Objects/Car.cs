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
        private bool IsSteering {  get; set; }
        private float TargetPositionX { get; set; }

        public Car(Vector2 position, int width, int height)
        {
            Position = position;
            Height = height;
            Width = width;
            Collider = new((int)Position.X, (int)Position.Y, Width, Height);
            IsColision = true;
            Speed = new(0, -5);

            var maxSpeed = -15 - 2 * GameSettings.Difficult;
            MaxSpeed = new Random().Next(maxSpeed, -15);
        }

        public Vector2 Speed
        {
            get => speed;

            set
            {
                speed = value;

                if (value.Y < MaxSpeed)
                    speed.Y = (float)MaxSpeed;
            }
        }

        public void MoveCollider() => Collider = new((int)Position.X, (int)Position.Y, Width, Height);

        public void Update(Vector2 offset, HashSet<IObject> objects)
        {
            GameObjects = objects;
            MoveIfCarAhead();
            Move(Position + Speed + offset);

            var maxSpeed = -15 - 2 * GameSettings.Difficult;
            if (MaxSpeed < maxSpeed)
                MaxSpeed = maxSpeed;

            if (Speed.Y > -10)
                Speed += new Vector2(0, -0.05f);
            else
                Speed += new Vector2(0, -0.005f);
        }

        public void Move(Vector2 newPos)
        {
            Position = newPos;
            MoveCollider();
        }

        private void MoveIfCarAhead()
        {
            var speedAdd = Vector2.Zero;
            var isLeftObstacle = false;
            var isRightObstacle = false;
            var isCarAhead = false;
            var distanceToLeftWall = GameConstants.TileSize * (GameConstants.ObjectsPositions.Min() - GameConstants.LeftWallPosition);
            var distanceToRightWall = GameConstants.TileSize * (GameConstants.RightWallPosition - GameConstants.ObjectsPositions.Max());

            if (!IsSteering)
            {
                foreach (var obj in GameObjects)
                {
                    if (obj is not ISolid || obj == this)
                        continue;

                    var offsetPosition = Position - obj.Position;
                    if (obj is Car || obj is Player)
                    {
                        if (offsetPosition.Y < 600 && offsetPosition.Y > 0 && Math.Abs(offsetPosition.X) < GameConstants.DistanceBeetweenCar && Speed.Y < obj.Speed.Y)                        
                            isCarAhead = true;

                        if (Math.Abs(offsetPosition.Y) < 600 && Math.Abs(offsetPosition.X) <= GameConstants.DistanceBeetweenCar && Math.Abs(offsetPosition.X) != 0)
                        {
                            if (offsetPosition.X > 0)
                                isLeftObstacle = true;
                            else
                                isRightObstacle = true;
                        }
                    }
                    else if (obj is Wall)
                    {
                        if (Math.Abs(offsetPosition.X) == distanceToLeftWall)
                            isLeftObstacle = true;
                        else if (Math.Abs(offsetPosition.X) == distanceToRightWall)
                            isRightObstacle = true;
                    }
                }

                if (isCarAhead)
                {
                    IsSteering = true;
                    if (isLeftObstacle && !isRightObstacle)
                        speedAdd = new Vector2(5, 0);
                    else if (!isLeftObstacle && isRightObstacle)
                        speedAdd = new Vector2(-5, 0);
                    else if (!isLeftObstacle && !isRightObstacle)
                        speedAdd = new Vector2(new int[] { -5, 5 }[new Random().Next(0, 2)], 0);
                    else
                    {
                        speedAdd = new Vector2(0, 1);
                        IsSteering = false;
                    }

                    Speed += speedAdd;
                    TargetPositionX = Position.X + (speedAdd.X / Math.Abs(speedAdd.X) * GameConstants.DistanceBeetweenCar);
                }
            }

            if (IsSteering && Position.X == TargetPositionX)
            {
                IsSteering = false;
                Speed = new(0, Speed.Y);
            }
        }
    }
}
