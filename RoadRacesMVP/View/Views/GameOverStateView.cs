using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadRacesMVP
{
    public class GameOverStateView : StateView
    {
        private MouseState CurrentMouse { get; set; }
        private MouseState PreviousMouse { get; set; }

        private List<IComponent> StateComponents = new();
        private Color color;

        public override event EventHandler<ActionType> ButtonClicked = delegate { };
        public override event EventHandler<Direction> PlayerSpeedChanged = delegate { };

        public GameOverStateView(Vector2 visualShift) : base(visualShift) { }

        public override void Draw(SpriteBatch spriteBatch, Dictionary<int, List<Texture2D>> textures, Dictionary<int, Texture2D> backgrounds, SpriteFont font45, SpriteFont font35)
        {
            spriteBatch.Draw(backgrounds[(byte)ViewType.GameOverStateView], new Vector2(0, 0), Color.White);
            foreach (var c in StateComponents)
            {
                color = c.IsHover ? color = Color.Gray : Color.White;
                spriteBatch.Draw(textures[c.ImageId].First(), c.Rectangle, color);
                if (!string.IsNullOrEmpty(c.Text))
                {
                    var x = (c.Rectangle.X + (c.Rectangle.Width / 2)) - (font35.MeasureString(c.Text).X / 2);
                    var y = (c.Rectangle.Y + (c.Rectangle.Height / 2)) - (font35.MeasureString(c.Text).Y / 2);

                    spriteBatch.DrawString(font35, c.Text, new Vector2(x, y), Color.Black);
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
        }
    }
}
