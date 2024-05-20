using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RoadRacesMVP
{
    public class GameplayPresenter
    {
        private IGameplayView View;
        private IState Model;

        public GameplayPresenter(IGameplayView view, IState state) => Initialize(view, state);

        private void Initialize(IGameplayView view, IState state)
        {
            Model = state;
            View = view;

            if (state is GameState gameState)
                gameState.GameOver += GameOver;

            View.CurrentStateView.ButtonClicked += ChangeState;
            View.CurrentStateView.PlayerSpeedChanged += ViewModelMovePlayer;
            View.CycleFinished += ViewStateUpdate;
            Model.UpdatedState += ModelViewUpdate;

            Model.Initialize();
        }

        private void ViewModelMovePlayer(object sender, Direction direction) => Model.ChangePlayerSpeed(direction);

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
            switch (actionType)
            {
                case ActionType.startGame:
                    View.ChangeStateView(ViewType.GameStateView);
                    Initialize(View, new GameState());
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
                    View.QuitGame();
                    break;

                case ActionType.quitToMenu:
                    View.ChangeStateView(ViewType.MainMenuStateView);
                    Initialize(View, new MainMenuState());
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
                    break;

                case ActionType.decreaseDifficult:
                    break;

                case ActionType.increaseDifficult:
                    break;

                case ActionType.decreaseVolume:
                    break;

                case ActionType.increaseVolume:
                    break;
            }
        }

        public void LaunchGame() => View.Run();
    }
}
