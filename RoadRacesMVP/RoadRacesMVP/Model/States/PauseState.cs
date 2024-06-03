﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class PauseState : State
    {

        public override event EventHandler<GameplayEventArgs> UpdatedState = delegate { };
        private PauseArgs PauseArgs { get; set; }

        public PauseState(PauseArgs pauseArgs) => PauseArgs = pauseArgs;

        public PauseArgs GetPauseArgs() => PauseArgs;

        public override void Initialize()
        {
            var countinueButton = new Button(new(735, 405), 450, 125, (byte)ObjectTypes.bigButton, ActionType.continueGame, "Продолжить");
            var settingsButton = new Button(new(735, 550), 450, 125, (byte)ObjectTypes.bigButton, ActionType.settingsFromPause, "Настройки");
            var quitToMenuButton = new Button(new(735, 695), 450, 125, (byte)ObjectTypes.bigButton, ActionType.quitToMenuFromGame, "Выйти в меню");

            Objects = new();
            Components = new() { countinueButton, settingsButton, quitToMenuButton };
            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }
    }
}