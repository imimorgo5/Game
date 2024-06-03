using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class GameplayEventArgs : EventArgs
    {
        public Dictionary<int, IObject> Objects { get; set; }
        public List<IComponent> Components { get; set; }
        public Vector2 POVShift { get; set; }
        public Vector2 PositionOffset { get; set; }
    }
}
