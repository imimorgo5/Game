using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{   public interface IObject
    {
        int ImageId { get; set; }

        Vector2 Position { get; set; }

        Vector2 Speed { get; set; }

        void Update(Vector2 offset);

        void Move(Vector2 pos);
    }
}
