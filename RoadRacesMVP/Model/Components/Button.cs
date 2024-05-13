using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class Button : IComponent
    {
        public int ImageId { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Vector2 Position { get; set; }
        public ActionType ActionType { get; set; }
        public string Text { get; set; }
        public double Count { get; set; }
        public bool IsHover { get; set; }
        public Rectangle Rectangle{ get => new((int)Position.X, (int)Position.Y, Width, Height); }

        public Button(Vector2 position, int width, int height, int imageId, ActionType type, string text)
        {
            Position = position;
            Width = width;
            Height = height;
            ImageId = imageId;
            ActionType = type;
            Text = text;
        }

        public void Update() { }
    }
}
