using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class GameOverArgs : EventArgs
    {
        public double ScoreCount { get; set; }
        public int CoinCount { get; set; }
    }
}
