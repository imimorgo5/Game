using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoadRacesMVP
{
    public static class Factory
    {
        private static Dictionary<string, (byte type, int width, int height)> Objects = new Dictionary<string, (byte, int, int)>()
        {
            {"Car", ((byte)ObjectTypes.car, 135, 234)},
            {"wall", ((byte)ObjectTypes.wall, 10, 100)},
            {"PlayerCar", ((byte)ObjectTypes.player, 135, 234)},
        };

        public static Car CreateClassicCar(float x, float y, Vector2 speed)
        {
            var car = new Car(new Vector2(x, y));
            car.ImageId = Objects["Car"].type;
            car.Speed = speed;
            return car;
        }

        public static Player CreatePlayerCar(float x, float y, Vector2 speed)
        {
            var playerCar = new Player(new Vector2(x, y));
            playerCar.ImageId = Objects["PlayerCar"].type;
            playerCar.Speed = speed;
            return playerCar;
        }

        public static Wall CreateWall(float x, float y)
        {
            var width = Objects["wall"].width;
            var length = Objects["wall"].height;            
            var wall = new Wall(new Vector2(x, y), width, length);
            wall.ImageId = Objects["wall"].type;
            return wall;
        }
    }
}
