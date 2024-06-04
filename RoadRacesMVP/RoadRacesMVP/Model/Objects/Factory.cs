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
            var car = new Car(new(x, y), Objects["Car"].width, Objects["Car"].height, Objects["Car"].type);
            car.ImageNumber = new Random().Next(0, 5);
            return car;
        }

        private static Player CreatePlayerCar(float x, float y) => 
            new(new(x, y), Objects["PlayerCar"].width, Objects["PlayerCar"].height, Objects["PlayerCar"].type);

        private static Wall CreateWall(float x, float y) =>
            new(new(x, y), Objects["Wall"].width, Objects["Wall"].height, Objects["Wall"].type);

        private static Coin CreateCoin(float x, float y) =>
            new(new(x, y), Objects["Coin"].width, Objects["Coin"].height, Objects["Coin"].type);

        private static Braker CreateBraker(float x, float y) =>
            new(new(x, y), Objects["Braker"].width, Objects["Braker"].height, Objects["Braker"].type);

        private static OffColisionObj CreateOffColisionObj(float x, float y) => 
            new(new(x, y), Objects["OffColisionObj"].width, Objects["OffColisionObj"].height, Objects["OffColisionObj"].type);

        private static BrokeSteeringObj CreateBrokeSteeringObj(float x, float y) =>
            new(new(x, y), Objects["BrokeSteeringObj"].width, Objects["BrokeSteeringObj"].height, Objects["BrokeSteeringObj"].type);

        private static Bomb CreateBomb(float x, float y) => 
            new(new(x, y), Objects["Bomb"].width, Objects["Bomb"].height, Objects["Bomb"].type);

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
                    generatedObject = CreatePlayerCar(x + GameConstants.TileSize / 2, y);
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
