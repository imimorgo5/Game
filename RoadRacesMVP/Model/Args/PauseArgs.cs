using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class PauseArgs : EventArgs
    {
        public HashSet<IObject> GameObjects {  get; set; }
        public double ScoreCount { get; set; }
        public Vector2 POVShift { get; set; }
        public Vector2 PositionOffset { get; set; }
    }
}
