using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoadRacesMVP
{
    public static class GameConstants
    {
        public const int TileSize = 60;
        public const int LeftWallPosition = 6;
        public const int RightWallPosition = 28;
        public const int RowCount = ScreenSize.ScreenWidth / TileSize;
        public static readonly char[] ObjectsChar = { 'S', 'O', 'R', 'B', 'E' };
        public static readonly int[] ObjectsPositions = { 8, 13, 18, 23 };
        public static readonly int DistanceBeetweenCar = TileSize * Math.Abs(ObjectsPositions[0] - ObjectsPositions[1]);
    }
}
