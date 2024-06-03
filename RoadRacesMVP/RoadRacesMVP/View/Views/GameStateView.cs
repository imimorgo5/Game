﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace RoadRacesMVP
{
    public class GameStateView : StateView
    {
        private Dictionary<int, IObject> Objects = new();
        private List<IComponent> StateComponents = new();
        private Vector2 PositionOffset = Vector2.Zero;

        public override event EventHandler<ActionType> ButtonClicked = delegate { };
        public override event EventHandler<Direction> PlayerSpeedChanged = delegate { };

        public GameStateView(Vector2 visualShift) : base(visualShift) 
        {
            FontSize = FontSize.Size35;
            ViewType = ViewType.GameStateView;
        }

        public override void Draw(SpriteBatch spriteBatch, Dictionary<int, List<Texture2D>> textures, Dictionary<int, Texture2D> backgrounds, Dictionary<int, SpriteFont> fonts)
        {
            spriteBatch.Draw(backgrounds[(byte)ViewType], new Vector2(0, VisualShift.Y), Color.White);
            spriteBatch.Draw(backgrounds[(byte)ViewType], new Vector2(0, VisualShift.Y - ScreenSize.ScreenHeight), Color.White);

            foreach (var obj in Objects.Values)
            {
                if (textures[obj.ImageId].Count != 0)
                {
                    Color = (obj is Player player && (player.IsColision == false || player.IsReverseSteering)) ? Color.Gray : Color.White;
                    spriteBatch.Draw(textures[obj.ImageId][obj.ImageNumber], new Vector2(obj.Position.X + VisualShift.X, obj.Position.Y - PositionOffset.Y), Color);
                }
            }

            var font = fonts[(byte)FontSize];
            foreach (var c in StateComponents)
            {
                Color = c.IsHover ? Color = Color.Gray : Color.White;
                spriteBatch.Draw(textures[c.ImageId].First(), c.Rectangle, Color);
                if (!string.IsNullOrEmpty(c.Text))
                {
                    var x = (c.Rectangle.X + (c.Rectangle.Width / 2)) - (font.MeasureString(c.Text).X / 2);
                    var y = (c.Rectangle.Y + (c.Rectangle.Height / 2)) - (font.MeasureString(c.Text).Y / 2);

                    spriteBatch.DrawString(font, c.Text, new Vector2(x, y), Color.Black);
                }
            }
        }

        public override void Update()
        {
            var keys = Keyboard.GetState().GetPressedKeys();
            if (keys.Length > 0)
            {
                var k = keys[0];
                switch (k)
                {
                    case Keys.D:
                        PlayerSpeedChanged.Invoke(this, Direction.right);
                        break;
                    case Keys.A:
                        PlayerSpeedChanged.Invoke(this, Direction.left);
                        break;
                    case Keys.Escape:
                        ButtonClicked.Invoke(this, ActionType.pause);
                        break;
                }
            }

            PreviousMouse = CurrentMouse;
            CurrentMouse = Mouse.GetState();
            var mouseRectangle = new Rectangle(CurrentMouse.X, CurrentMouse.Y, 1, 1);

            foreach (var c in StateComponents)
            {
                if (c is Button)
                {
                    if (mouseRectangle.Intersects(c.Rectangle))
                    {
                        c.IsHover = true;
                        if (CurrentMouse.LeftButton == ButtonState.Released && PreviousMouse.LeftButton == ButtonState.Pressed)
                        {
                            ButtonClicked.Invoke(this, c.ActionType);
                            break;
                        }
                    }
                    else
                        c.IsHover = false;
                }
            }
        }

        public override void LoadGameCycleParameters(List<IComponent> components, Dictionary<int, IObject> objects, Vector2 POVShift, Vector2 positionOffset)
        {
            StateComponents = components;
            Objects = objects;
            VisualShift = new(VisualShift.X + POVShift.X, (VisualShift.Y + POVShift.Y) % ScreenSize.ScreenHeight);
            PositionOffset = positionOffset;
        }
    }
}
