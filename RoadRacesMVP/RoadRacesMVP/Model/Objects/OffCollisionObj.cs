using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class OffColisionObj : CollectedObject, IObject
    {
        public OffColisionObj(Vector2 position, int width, int height) : base(position, width, height) { }
    }
}