using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoadRacesMVP
{
    public class RoadRacesView : Game, IGameplayView
    {
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        public Dictionary<int, Texture2D> Textures = new Dictionary<int, Texture2D>();
        public SpriteFont Font;
        public Dictionary<int, Texture2D> Backgrounds = new Dictionary<int, Texture2D>();
        public Vector2 VisualShift = Vector2.Zero;
        public StateView CurrentStateView { get; set; }

        public event EventHandler CycleFinished = delegate { };

        public RoadRacesView()
        {
            CurrentStateView = new MainMenuStateView(VisualShift);
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Graphics.IsFullScreen = false;
            Graphics.PreferredBackBufferWidth = 1920;
            Graphics.PreferredBackBufferHeight = 1080;
            Graphics.ApplyChanges();
            VisualShift -= new Vector2(Graphics.PreferredBackBufferWidth / 2, Graphics.PreferredBackBufferHeight * 0.8f);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Textures.Add((byte)ObjectTypes.car, Content.Load<Texture2D>("Sprites/Car"));
            Textures.Add((byte)ObjectTypes.player, Content.Load<Texture2D>("Sprites/Car"));
            Textures.Add((byte)ObjectTypes.button, Content.Load<Texture2D>("Sprites/Button"));
            Textures.Add((byte)ObjectTypes.counter, Content.Load<Texture2D>("Sprites/Button"));
            Backgrounds.Add((byte)ViewType.MainMenuStateView, Content.Load<Texture2D>("Backgrounds/MenuBackground"));
            Backgrounds.Add((byte)ViewType.GameStateView, Content.Load<Texture2D>("Backgrounds/GameBackground"));
            Font = Content.Load<SpriteFont>("Fonts/Font");

        }

        protected override void Update(GameTime gameTime)
        {
            CurrentStateView.Update();
            base.Update(gameTime);
            CycleFinished.Invoke(this, new EventArgs());
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);            
            SpriteBatch.Begin();
            CurrentStateView.Draw(SpriteBatch, Textures, Backgrounds, Font);            
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        public void ChangeStateView(ViewType viewType)
        {            
            switch (viewType)
            {
                case ViewType.GameStateView:
                    {
                        CurrentStateView = new GameStateView(VisualShift);
                        break;
                    }
                case ViewType.MainMenuStateView:
                    {
                        CurrentStateView = new MainMenuStateView(VisualShift);
                        break;
                    }
            }            
        }

        public void QuitGame() => Exit();
    }
}
