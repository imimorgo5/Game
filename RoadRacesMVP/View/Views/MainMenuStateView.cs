using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoadRacesMVP
{
    public class MainMenuStateView : StateView
    {
        public MouseState CurrentMouse { get; set; }
        public MouseState PreviousMouse { get; set; }

        public Dictionary<int, IObject> Objects = new Dictionary<int, IObject>();
        public List<IComponent> StateComponents = new List<IComponent>();
        public Color color;

        public override event EventHandler<ActionType> ButtonClicked = delegate { };
        public override event EventHandler<Direction> PlayerSpeedChanged = delegate { };

        public MainMenuStateView(Vector2 visualShift) : base(visualShift) { }

        public override void Draw(SpriteBatch spriteBatch, Dictionary<int, Texture2D> textures, Dictionary<int, Texture2D> backgrounds, SpriteFont font)
        {
            spriteBatch.Draw(backgrounds[(byte)ViewType.MainMenuStateView], new Vector2(0, 0), Color.White);
            foreach (var c in StateComponents)
            {
                color = c.IsHover ? color = Color.Gray : Color.White;
                spriteBatch.Draw(textures[c.ImageId], c.Rectangle, color);
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
                            ButtonClicked.Invoke(this, c.ActionType);
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
        }
    }
}
