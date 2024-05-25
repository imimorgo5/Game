using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{   public interface IObject
    {
        int ImageId { get; set; }
        int ImageNumber { get; set; }

        Vector2 Position { get; }
        RectangleCollider Collider { get; }

        Vector2 Speed { get; set; }

        int Width { get; }
        int Height { get; }

        void Update(Vector2 offset, HashSet<IObject> objects);

        void Move(Vector2 pos);
    }
}
