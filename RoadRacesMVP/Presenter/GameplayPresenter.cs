using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RoadRacesMVP
{
    public class GameplayPresenter
    {
        private IGameplayView GameplayView;
        private IGameplayModel GameplayModel;
        private IState MainMenu;

        public GameplayPresenter(IGameplayView gameplayView, IGameplayModel gameplayModel)
        {
            GameplayModel = gameplayModel;
            GameplayView = gameplayView;

            GameplayView.CurrentStateView.ButtonClicked += ChangeState;
            GameplayView.CurrentStateView.PlayerSpeedChanged += ViewModelMovePlayer;
            GameplayView.CycleFinished += ViewModelUpdate;
            GameplayModel.Updated += ModelViewUpdate;

            GameplayModel.Initialize();
        }

        public GameplayPresenter(IState mainMenu, IGameplayView gameplayView)
        {
            MainMenu = mainMenu;
            GameplayView = gameplayView;
            GameplayView.CurrentStateView.ButtonClicked += ChangeState;

            GameplayView.CycleFinished += ViewStateUpdate;
            MainMenu.UpdatedState += ModelViewUpdate;
            MainMenu.Initialize();
        }

        private void ViewModelMovePlayer(object sender, Direction direction)
        {
            GameplayModel.ChangePlayerSpeed(direction);
        }

        private void ModelViewUpdate(object sender, GameplayEventArgs e)
        {
            GameplayView.CurrentStateView.LoadGameCycleParameters(e.Components, e.Objects, e.POVShift, e.PositionOffset);
        }

        private void ViewModelUpdate(object sender, EventArgs e)
        {
            GameplayModel.Update();
        }

        private void ViewStateUpdate(object sender, EventArgs e)
        {
            MainMenu.Update();
        }

        private void ChangeState(object sender, ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.newGame:
                    {
                        GameplayView.ChangeStateView(ViewType.GameStateView);
                        var game = new GameplayPresenter(GameplayView, new GameState());
                        break;
                    }
                case ActionType.loadGame:
                    {
                        break;
                    }
                case ActionType.settings:
                    {
                        break;
                    }
                case ActionType.quitGame:
                    {                        
                        GameplayView.QuitGame();
                        break;
                    }
                case ActionType.quitToMenu:
                    {
                        GameplayView.ChangeStateView(ViewType.MainMenuStateView);
                        var game = new GameplayPresenter(new MainMenuState(), GameplayView);
                        break;
                    }
            }
        }

        public void LaunchGame()
        {
            GameplayView.Run();
        }
    }
}
