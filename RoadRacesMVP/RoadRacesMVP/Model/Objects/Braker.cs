﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class Braker : CollectedObject, IObject
    {        
        public Braker(Vector2 position, int width, int height, int imageId) : base(position, width, height, imageId) { }
    }
}