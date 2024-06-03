using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoadRacesMVP
{
    public static class Factory
    {
        private static readonly Dictionary<string, (byte type, int width, int height)> Objects = new()
        {
            {"Car", ((byte)ObjectTypes.car, 125, 250)},
            {"Wall", ((byte)ObjectTypes.wall, 10, 50)},
            {"PlayerCar", ((byte)ObjectTypes.player, 145, 250)},
            {"Coin",  ((byte)ObjectTypes.coin, 208, 214)},
            {"Braker",  ((byte)ObjectTypes.braker, 200, 200)},
            {"OffColisionObj",  ((byte)ObjectTypes.offColisionObj, 200, 200)},
            {"BrokeSteeringObj",  ((byte)ObjectTypes.brokeSteeringObj, 200, 200)},
            {"Bomb", ((byte)ObjectTypes.bomb, 200, 200)}
        };

        private static Car CreateClassicCar(float x, float y)
        {
            var width = Objects["Car"].width;
            var height = Objects["Car"].height;
            var car = new Car(new(x, y), width, height);
            car.ImageId = Objects["Car"].type;
            car.ImageNumber = new Random().Next(0, 5);
            return car;
        }

        private static Player CreatePlayerCar(float x, float y, Vector2 speed)
        {
            var width = Objects["PlayerCar"].width;
            var height = Objects["PlayerCar"].height;
            var playerCar = new Player(new(x, y), width, height);
            playerCar.ImageId = Objects["PlayerCar"].type;
            playerCar.Speed = speed;
            return playerCar;
        }

        private static Wall CreateWall(float x, float y)
        {
            var width = Objects["Wall"].width;
            var height = Objects["Wall"].height;            
            var wall = new Wall(new(x, y), width, height);
            wall.ImageId = Objects["Wall"].type;
            return wall;
        }

        private static Coin CreateCoin(float x, float y)
        {
            var width = Objects["Coin"].width;
            var height = Objects["Coin"].height;
            var coin = new Coin(new(x, y), width, height);
            coin.ImageId = Objects["Coin"].type;
            return coin;
        }

        private static Braker CreateBraker(float x, float y)
        {
            var width = Objects["Braker"].width;
            var height = Objects["Braker"].height;
            var braker = new Braker(new(x, y), width, height);
            braker.ImageId = Objects["Braker"].type;
            return braker;
        }

        private static OffColisionObj CreateOffColisionObj(float x, float y)
        {
            var width = Objects["OffColisionObj"].width;
            var height = Objects["OffColisionObj"].height;
            var offColisionObj = new OffColisionObj(new(x, y), width, height);
            offColisionObj.ImageId = Objects["OffColisionObj"].type;
            return offColisionObj;
        }

        private static BrokeSteeringObj CreateBrokeSteeringObj(float x, float y)
        {
            var width = Objects["BrokeSteeringObj"].width;
            var height = Objects["BrokeSteeringObj"].height;
            var brokeSteeringObj = new BrokeSteeringObj(new(x, y), width, height);
            brokeSteeringObj.ImageId = Objects["BrokeSteeringObj"].type;
            return brokeSteeringObj;
        }

        private static Bomb CreateBomb(float x, float y)
        {
            var width = Objects["Bomb"].width;
            var height = Objects["Bomb"].height;
            var bomb = new Bomb(new(x, y), width, height);
            bomb.ImageId = Objects["Bomb"].type;
            return bomb;
        }

        public static IObject GenerateObject(char sign, float xTile, float yTile)
        {
            var x = xTile * GameConstants.TileSize;
            var y = yTile * GameConstants.TileSize;
            IObject generatedObject = null;

            switch (sign)
            {
                case 'C':
                    generatedObject = CreateClassicCar(x, y);
                    break;
                case 'P':
                    generatedObject = CreatePlayerCar(x + GameConstants.TileSize / 2, y, new(0, 0));
                    break;
                case 'W':
                    generatedObject = CreateWall(x, y);
                    break;
                case 'S':
                    generatedObject = CreateCoin(x, y);
                    break;
                case 'B':
                    generatedObject = CreateBraker(x, y);
                    break;
                case 'O':
                    generatedObject = CreateOffColisionObj(x, y);
                    break;
                case 'R':
                    generatedObject = CreateBrokeSteeringObj(x, y);
                    break;
                case 'E':
                    generatedObject = CreateBomb(x, y);
                    break;
            }

            return generatedObject;
        }
    }
}
