using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace RoadRacesMVP
{
    public class RoadRacesView : Game, IGameplayView
    {
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Dictionary<int, List<Texture2D>> Textures {  get; set; }
        private Dictionary<int, SoundEffect> SoundEffects { get; set; }
        private Dictionary<int, Song> Songs { get; set; }
        private SpriteFont Font45 {  get; set; }
        private SpriteFont Font35 { get; set; }
        private Dictionary<int, Texture2D> Backgrounds {  get; set; }
        private Vector2 VisualShift { get; set; }
        public StateView CurrentStateView { get; private set; }

        public event EventHandler CycleFinished = delegate { };

        public RoadRacesView()
        {
            CurrentStateView = new MainMenuStateView(VisualShift);
            Graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            VisualShift = Vector2.Zero;
            Backgrounds = new();
            Textures = new();
            SoundEffects = new();
            Songs = new();
        }

        protected override void Initialize()
        {
            base.Initialize();            
            Graphics.IsFullScreen = false;
            Graphics.PreferredBackBufferWidth = GameConstants.SCREENWIDTH;
            Graphics.PreferredBackBufferHeight = GameConstants.SCREENHEIGHT;
            Graphics.ApplyChanges();
            VisualShift -= new Vector2(Graphics.PreferredBackBufferWidth / 2, Graphics.PreferredBackBufferHeight * 0.8f);
            Window.IsBorderless = true;
            MediaPlayer.IsRepeating = true;


            ChangeVolume();
            PlaySound(SoundType.MenuSong);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Textures.Add((byte)ObjectTypes.car, new());
            Textures[(byte)ObjectTypes.car].Add(Content.Load<Texture2D>("Sprites/BlueCar"));
            Textures[(byte)ObjectTypes.car].Add(Content.Load<Texture2D>("Sprites/GreenCar"));
            Textures[(byte)ObjectTypes.car].Add(Content.Load<Texture2D>("Sprites/OrangeCar"));
            Textures[(byte)ObjectTypes.car].Add(Content.Load<Texture2D>("Sprites/PinkCar"));
            Textures[(byte)ObjectTypes.car].Add(Content.Load<Texture2D>("Sprites/YellowCar"));

            Textures.Add((byte)ObjectTypes.wall, new());
            Textures.Add((byte)ObjectTypes.coin, new() { Content.Load<Texture2D>("Sprites/Coin") });
            Textures.Add((byte)ObjectTypes.bomb, new() { Content.Load<Texture2D>("Sprites/Bomb") });
            Textures.Add((byte)ObjectTypes.brokeSteeringObj, new() { Content.Load<Texture2D>("Sprites/BrokeSteeringObj") });
            Textures.Add((byte)ObjectTypes.offColisionObj, new() { Content.Load<Texture2D>("Sprites/OffColisionObj") });
            Textures.Add((byte)ObjectTypes.braker, new() { Content.Load<Texture2D>("Sprites/Braker") });
            Textures.Add((byte)ObjectTypes.player, new() { Content.Load<Texture2D>("Sprites/PlayerCar") });
            Textures.Add((byte)ObjectTypes.bigButton, new() { Content.Load<Texture2D>("Sprites/BigButton") });
            Textures.Add((byte)ObjectTypes.smallButton, new() { Content.Load<Texture2D>("Sprites/SmallButton") });
            Textures.Add((byte)ObjectTypes.counter, new() { Content.Load<Texture2D>("Sprites/BigButton") });

            Backgrounds.Add((byte)ViewType.MainMenuStateView, Content.Load<Texture2D>("Backgrounds/MainMenuBackground"));
            Backgrounds.Add((byte)ViewType.GameStateView, Content.Load<Texture2D>("Backgrounds/GameBackground"));
            Backgrounds.Add((byte)ViewType.SettingsStateView, Content.Load<Texture2D>("Backgrounds/SettingsBackground"));
            Backgrounds.Add((byte)ViewType.PauseStateView, Content.Load<Texture2D>("Backgrounds/PauseBackground"));
            Backgrounds.Add((byte)ViewType.GameOverStateView, Content.Load<Texture2D>("Backgrounds/GameOverBackground"));
            Backgrounds.Add((byte)ViewType.RulesStateView, Content.Load<Texture2D>("Backgrounds/RulesBackground"));

            SoundEffects.Add((byte)SoundType.ButtonClick, Content.Load<SoundEffect>("MusicAndSounds/ButtonClickSound"));
            SoundEffects.Add((byte)SoundType.SelectCoin, Content.Load<SoundEffect>("MusicAndSounds/CoinSelectSound"));
            SoundEffects.Add((byte)SoundType.Bomb, Content.Load<SoundEffect>("MusicAndSounds/BombSound"));
            SoundEffects.Add((byte)SoundType.Hit, Content.Load<SoundEffect>("MusicAndSounds/HitSound"));
            SoundEffects.Add((byte)SoundType.Brake, Content.Load<SoundEffect>("MusicAndSounds/BrakeSound"));
            SoundEffects.Add((byte)SoundType.Standart, Content.Load<SoundEffect>("MusicAndSounds/SelectSound"));
            Songs.Add((byte)SoundType.MenuSong, Content.Load<Song>("MusicAndSounds/MainMenuMusic"));
            Songs.Add((byte)SoundType.GameSong, Content.Load<Song>("MusicAndSounds/GameMusic"));

            Font45 = Content.Load<SpriteFont>("Fonts/Font45");
            Font35 = Content.Load<SpriteFont>("Fonts/Font35");
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
            CurrentStateView.Draw(SpriteBatch, Textures, Backgrounds, Font45, Font35);            
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        public void ChangeStateView(ViewType viewType)
        {
            CycleFinished = delegate { };
            switch (viewType)
            {
                case ViewType.GameStateView:
                    CurrentStateView = new GameStateView(VisualShift);
                    break;
                case ViewType.MainMenuStateView:
                    CurrentStateView = new MainMenuStateView(VisualShift);
                    break;
                case ViewType.SettingsStateView:
                    CurrentStateView = new SettingsStateView(VisualShift);
                    break;
                case ViewType.GameOverStateView:
                    CurrentStateView = new GameOverStateView(VisualShift);
                    break;
                case ViewType.PauseStateView:
                    CurrentStateView = new PauseStateView(VisualShift);
                    break;
                case ViewType.RulesStateView:
                    CurrentStateView = new RulesStateView(VisualShift);
                    break;
            }          
        }

        public void PlaySound(SoundType soundType)
        {
            if (SoundEffects.TryGetValue((byte)soundType, out SoundEffect effect))
                effect.Play();
            else
                MediaPlayer.Play(Songs[(byte)soundType]);
        }

        public void ChangeVolume()
        {
            MediaPlayer.Volume = (float)GameSettings.Volume / 10;
            SoundEffect.MasterVolume = (float)GameSettings.Volume / 10;
        }

        public void QuitGame() => Exit();
    }
}
