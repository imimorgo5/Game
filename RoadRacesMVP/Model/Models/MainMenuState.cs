using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class MainMenuState : IState
    {
        public List<IComponent> Components { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }

        public event EventHandler<GameplayEventArgs> UpdatedState = delegate { };

        public void Initialize()
        {
            var newGameButton = new Button(new Vector2(735, 330), 450, 125, (byte)ObjectTypes.button, ActionType.loadGame, "Играть");

            var loadGameButton = new Button(new Vector2(735, 470), 450, 125, (byte)ObjectTypes.button, ActionType.newGame, "Новая игра");

            var settingsButton = new Button(new Vector2(735, 610), 450, 125, (byte)ObjectTypes.button, ActionType.settings, "Настройки");

            var quitButton = new Button(new Vector2(735, 750), 450, 125, (byte)ObjectTypes.button, ActionType.quitGame, "Выйти");

            Objects = new Dictionary<int, IObject>();
            Components = new List<IComponent>() { newGameButton, loadGameButton, settingsButton, quitButton };
            UpdatedState.Invoke(this, new GameplayEventArgs { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }

        public void Update()
        {
            foreach (var component in Components)
                component.Update();

            UpdatedState.Invoke(this, new GameplayEventArgs { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }
    }
}
