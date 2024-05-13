using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class Counter : IComponent
    {
        public int ImageId { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public string Prefix {  get; set; }
        public double Count { get; set; }
        public ActionType ActionType { get; set; }
        public bool IsHover { get; set; }
        public Rectangle Rectangle { get => new((int)Position.X, (int)Position.Y, Width, Height); }

        public Counter(Vector2 position, int width, int height, int imageId, ActionType type, string prefix)
        {
            Position = position;
            Width = width;
            Height = height;
            ImageId = imageId;
            ActionType = type;
            Count = 0;
            Prefix = prefix;
            Text = prefix + Count.ToString();
        }

        public void Update() 
        {
            Text = Prefix + ((long)Count).ToString();
        }
    }
}

