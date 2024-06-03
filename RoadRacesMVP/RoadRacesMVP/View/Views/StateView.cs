using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadRacesMVP
{
    public abstract class StateView
    {
        public virtual event EventHandler<ActionType> ButtonClicked = delegate { };
        public virtual event EventHandler<Direction> PlayerSpeedChanged;
        protected Vector2 VisualShift { get; set; }
        private List<IComponent> StateComponents { get; set; }
        protected MouseState CurrentMouse { get; set; }
        protected MouseState PreviousMouse { get; set; }
        protected Color Color { get; set; }
        protected FontSize FontSize { get; set;  }
        protected ViewType ViewType { get; set; }

        public StateView(Vector2 visualShift) => VisualShift = visualShift;

        public virtual void Update()
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

        public virtual void Draw(SpriteBatch spriteBatch, Dictionary<int, List<Texture2D>> textures, Dictionary<int, Texture2D> backgrounds, Dictionary<int, SpriteFont> fonts)
        {
            var font = fonts[(byte)FontSize];
            spriteBatch.Draw(backgrounds[(byte)ViewType], new Vector2(0, 0), Color.White);
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

        public virtual void LoadGameCycleParameters(List<IComponent> components, Dictionary<int, IObject> objects, Vector2 POVShift, Vector2 positionOffset) => StateComponents = components;
    }
}
