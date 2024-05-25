using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text.Json;

namespace RoadRacesMVP
{
    public class GameplayPresenter
    {
        private IGameplayView View;
        private IState Model;

        public GameplayPresenter(IGameplayView view, IState state)
        {
            GameSettings.Load();
            Initialize(view, state);
        }

        private void Initialize(IGameplayView view, IState state)
        {
            Model = state;
            View = view;

            if (state is GameState gameState)
            {
                gameState.GameOver += GameOver;
                gameState.PlaySound += PlaySound;
            }

            View.CurrentStateView.ButtonClicked += ChangeState;
            View.CurrentStateView.PlayerSpeedChanged += ViewModelMovePlayer;
            View.CycleFinished += ViewStateUpdate;
            Model.UpdatedState += ModelViewUpdate;

            Model.Initialize();
        }

        private void ViewModelMovePlayer(object sender, Direction direction) => (Model as GameState).ChangePlayerSpeed(direction);

        private void ModelViewUpdate(object sender, GameplayEventArgs args) => View.CurrentStateView.LoadGameCycleParameters(args.Components, args.Objects, args.POVShift, args.PositionOffset);

        private void ViewStateUpdate(object sender, EventArgs args) => Model.Update();

        private void GameOver(object sender, GameOverArgs args)
        {
            View.ChangeStateView(ViewType.GameOverStateView);
            var gameOverState = new GameOverState();
            gameOverState.SetCounts(args.ScoreCount, args.CoinCount);
            Initialize(View, gameOverState);
        }

        private void ChangeState(object sender, ActionType actionType)
        {
            PlaySound(this, SoundType.ButtonClick);
            switch (actionType)
            {
                case ActionType.startGame:
                    View.ChangeStateView(ViewType.GameStateView);
                    Initialize(View, new GameState());
                    PlaySound(this, SoundType.GameSong);
                    break;

                case ActionType.settingsFromMenu:
                    View.ChangeStateView(ViewType.SettingsStateView);
                    Initialize(View, new SettingsFromMenuState());
                    break;

                case ActionType.settingsFromPause:
                    View.ChangeStateView(ViewType.SettingsStateView);
                    Initialize(View, new SettingsFromPauseState((Model as PauseState).GetPauseArgs()));
                    break;

                case ActionType.quitGame:
                    GameSettings.Save();
                    View.QuitGame();
                    break;

                case ActionType.quitToMenu:
                    View.ChangeStateView(ViewType.MainMenuStateView);
                    Initialize(View, new MainMenuState());
                    break;

                case ActionType.quitToMenuFromGame:
                    View.ChangeStateView(ViewType.MainMenuStateView);
                    Initialize(View, new MainMenuState());
                    PlaySound(this, SoundType.MenuSong);
                    break;

                case ActionType.pause:
                    View.ChangeStateView(ViewType.PauseStateView);

                    var args = new PauseArgs();
                    if (Model is GameState gameState)
                        args = gameState.GetPauseArgs();
                    else if (Model is SettingsFromPauseState settings)
                        args = settings.GetPauseArgs();

                    Initialize(View, new PauseState(args));
                    break;

                case ActionType.continueGame:
                    View.ChangeStateView(ViewType.GameStateView);
                    Initialize(View, new GameState((Model as PauseState).GetPauseArgs()));
                    break;

                case ActionType.rules:
                    View.ChangeStateView(ViewType.RulesStateView);
                    Initialize(View, new RulesState());
                    break;

                case ActionType.resetRecordScore:
                    GameSettings.RecordScoreCount = 0;
                    break;

                case ActionType.decreaseDifficult:
                    if (GameSettings.Difficult > 0)
                        GameSettings.Difficult--;
                    break;

                case ActionType.increaseDifficult:
                    if (GameSettings.Difficult < 10)
                        GameSettings.Difficult++;
                    break;

                case ActionType.decreaseVolume:
                    if (GameSettings.Volume > 0)
                        GameSettings.Volume--;
                    View.ChangeVolume();
                    break;

                case ActionType.increaseVolume:
                    if (GameSettings.Volume < 10)
                        GameSettings.Volume++;
                    View.ChangeVolume();
                    break;
            }
        }

        public void PlaySound(object sender, SoundType soundType) => View.PlaySound(soundType);

        public void LaunchGame() => View.Run();
    }
}
