using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;

namespace RoadRacesMVP
{
    public class GameStateView : StateView
    {
        public MouseState CurrentMouse { get; set; }
        public MouseState PreviousMouse { get; set; }

        public Dictionary<int, IObject> Objects = new Dictionary<int, IObject>();
        public List<IComponent> StateComponents = new List<IComponent>();
        public Color color;
        public Vector2 PositionOffset = Vector2.Zero;

        public override event EventHandler<ActionType> ButtonClicked = delegate { };
        public override event EventHandler<Direction> PlayerSpeedChanged = delegate { };

        public GameStateView(Vector2 visualShift) : base(visualShift) { }

        public override void Draw(SpriteBatch spriteBatch, Dictionary<int, Texture2D> textures, Dictionary<int, Texture2D> backgrounds, SpriteFont font)
        {
            spriteBatch.Draw(backgrounds[(byte)ViewType.GameStateView], new Vector2(0, VisualShift.Y), Color.White);
            spriteBatch.Draw(backgrounds[(byte)ViewType.GameStateView], new Vector2(0, VisualShift.Y - 1080), Color.White);

            foreach (var obj in Objects.Values)
                if (obj.ImageId != (byte)ObjectTypes.wall)
                    spriteBatch.Draw(textures[obj.ImageId], new Vector2(obj.Position.X + VisualShift.X, obj.Position.Y - PositionOffset.Y), Color.White);

            foreach (var c in StateComponents)
            {
                color = c.IsHover ? color = Color.Gray : Color.White;                
                spriteBatch.Draw(textures[c.ImageId], c.Rectangle, color);

                if (!string.IsNullOrEmpty(c.Text))
                {
                    var x = c.Rectangle.X + (c.Rectangle.Width / 2) - (font.MeasureString(c.Text).X / 2);
                    var y = c.Rectangle.Y + (c.Rectangle.Height / 2) - (font.MeasureString(c.Text).Y / 2);

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
                        {
                            PlayerSpeedChanged.Invoke(this, Direction.right);
                            break;
                        }
                    case Keys.A:
                        {
                            PlayerSpeedChanged.Invoke(this, Direction.left);
                            break;
                        }
                    case Keys.Escape:
                        {
                            ButtonClicked.Invoke(this, ActionType.quitGame);
                            break;
                        }
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
            VisualShift = new Vector2(VisualShift.X + POVShift.X, (VisualShift.Y + POVShift.Y) % 1080);
            PositionOffset = new Vector2(PositionOffset.X, (PositionOffset.Y + positionOffset.Y) % 1080);
        }
    }
}
