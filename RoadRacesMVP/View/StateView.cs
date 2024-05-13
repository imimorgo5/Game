using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoadRacesMVP
{
    public abstract class StateView
    {
        public abstract event EventHandler<ActionType> ButtonClicked;
        public abstract event EventHandler<Direction> PlayerSpeedChanged;
        public Vector2 VisualShift;

        public StateView(Vector2 visualShift)
        {
            VisualShift = visualShift;
        }

        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch, Dictionary<int, Texture2D> textures, Dictionary<int, Texture2D> backgrounds, SpriteFont font);
        public abstract void LoadGameCycleParameters(List<IComponent> components, Dictionary<int, IObject> objects, Vector2 POVShift, Vector2 positionOffset);
    }
}
