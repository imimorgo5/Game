﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class MainMenuState : State
    {
        public override event EventHandler<GameplayEventArgs> UpdatedState = delegate { };

        public override void Initialize()
        {
            var newGameButton = new Button(new(735, 405), 450, 120, (byte)ObjectTypes.bigButton, ActionType.startGame, "Играть");
            var settingsButton = new Button(new(735, 550), 450, 120, (byte)ObjectTypes.bigButton, ActionType.settingsFromMenu, "Настройки");
            var quitButton = new Button(new(735, 695), 450, 120, (byte)ObjectTypes.bigButton, ActionType.quitGame, "Выйти");
            var rulesButton = new Button(new(25, 30), 500, 120, (byte)ObjectTypes.bigButton, ActionType.rules, "Правила игры");

            Objects = new();
            Components = new() { newGameButton, settingsButton, quitButton, rulesButton };
            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }
    }
}
