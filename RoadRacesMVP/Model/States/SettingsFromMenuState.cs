using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class SettingsFromMenuState : IState
    {
        public List<IComponent> Components { get; private set; }
        public Dictionary<int, IObject> Objects { get; private set; }

        public event EventHandler<GameplayEventArgs> UpdatedState = delegate { };

        public void Initialize()
        {
            var exitToMenuButton = new Button(new(50, 50), 300, 120, (byte)ObjectTypes.bigButton, ActionType.quitToMenu, "Назад");
            var resetRecordScore = new Button(new(710, 790), 500, 120, (byte)ObjectTypes.bigButton, ActionType.resetRecordScore, "Сбросить рекорд");
            var increaseVolumeButton = new Button(new(1070, 410), 100, 100, (byte)ObjectTypes.smallButton, ActionType.increaseVolume, "+");
            var decreaseVolumeButton = new Button(new(740, 410), 100, 100, (byte)ObjectTypes.smallButton, ActionType.decreaseVolume, "-");
            var increaseDifficultButton = new Button(new(1070, 640), 100, 100, (byte)ObjectTypes.smallButton, ActionType.increaseDifficult, "+");
            var decreaseDifficultButton = new Button(new(740, 640), 100, 100, (byte)ObjectTypes.smallButton, ActionType.decreaseDifficult, "-");
            var volumeLevel = new Counter(new(855, 410), 200, 100, (byte)ObjectTypes.counter, ActionType.scoreCount, string.Empty);
            var difficultLevel = new Counter(new(855, 640), 200, 100, (byte)ObjectTypes.counter, ActionType.scoreCount, string.Empty);


            Objects = new();
            Components = new() { exitToMenuButton, resetRecordScore, increaseVolumeButton, decreaseVolumeButton, increaseDifficultButton, decreaseDifficultButton, volumeLevel, difficultLevel };
            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }

        public void Update()
        {
            foreach (var component in Components)
                component.Update();

            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }

        public void ChangePlayerSpeed(Direction dir) { }
    }
}
