using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class SettingsFromPauseState : State
    {
        private PauseArgs PauseArgs { get; set; }

        public override event EventHandler<GameplayEventArgs> UpdatedState = delegate { };
        public event EventHandler<SoundType> PlaySound = delegate { };

        public SettingsFromPauseState(PauseArgs pauseArgs) => PauseArgs = pauseArgs;

        public PauseArgs GetPauseArgs() => PauseArgs;

        public override void Initialize()
        {
            var exitToMenuButton = new Button(new(50, 50), 300, 120, (byte)ObjectTypes.bigButton, ActionType.pause, "Назад");
            var resetRecordScore = new Button(new(710, 790), 500, 120, (byte)ObjectTypes.bigButton, ActionType.resetRecordScore, "Сбросить рекорд");
            var increaseVolumeButton = new Button(new(1070, 410), 100, 100, (byte)ObjectTypes.smallButton, ActionType.increaseVolume, "+");
            var decreaseVolumeButton = new Button(new(740, 410), 100, 100, (byte)ObjectTypes.smallButton, ActionType.decreaseVolume, "-");
            var increaseDifficultButton = new Button(new(1070, 640), 100, 100, (byte)ObjectTypes.smallButton, ActionType.increaseDifficult, "+");
            var decreaseDifficultButton = new Button(new(740, 640), 100, 100, (byte)ObjectTypes.smallButton, ActionType.decreaseDifficult, "-");
            var volumeLevel = new Counter(new(855, 410), 200, 100, (byte)ObjectTypes.counter, ActionType.scoreCount, string.Empty);
            volumeLevel.ScoreFunction = () => volumeLevel.Count = GameSettings.Volume;
            var difficultLevel = new Counter(new(855, 640), 200, 100, (byte)ObjectTypes.counter, ActionType.scoreCount, string.Empty);
            difficultLevel.ScoreFunction = () => difficultLevel.Count = GameSettings.Difficult;


            Objects = new();
            Components = new() { exitToMenuButton, resetRecordScore, increaseVolumeButton, decreaseVolumeButton, increaseDifficultButton, decreaseDifficultButton, volumeLevel, difficultLevel };
            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }
    }
}
