using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class GameOverState : IState
    {
        public List<IComponent> Components { get; private set; }
        public Dictionary<int, IObject> Objects { get; private set; }

        public event EventHandler<GameplayEventArgs> UpdatedState = delegate { };

        private double ScoreCount { get; set; }
        private int CoinCount { get; set; }
        private double Result { get; set; }


        public void Initialize()
        {
            var scoreCounter = new Counter(new(985, 415), 350, 100, (byte)ObjectTypes.counter, ActionType.scoreCount, string.Empty);
            scoreCounter.ScoreFunction = () => scoreCounter.Count = ScoreCount;

            var coinCounter = new Counter(new(1135, 565), 200, 100, (byte)ObjectTypes.counter, ActionType.scoreCount, string.Empty);
            coinCounter.ScoreFunction = () => coinCounter.Count = CoinCount;

            var resultCounter = new Counter(new(985, 715), 350, 100, (byte)ObjectTypes.counter, ActionType.scoreCount, string.Empty);
            resultCounter.ScoreFunction = () => resultCounter.Count = Result;

            var quitToMenuButton = new Button(new(550, 910), 410, 120, (byte)ObjectTypes.bigButton, ActionType.quitToMenu, "Выйти в меню");
            var gameButton = new Button(new(975, 910), 410, 120, (byte)ObjectTypes.bigButton, ActionType.startGame, "Сыграть ещё");

            Objects = new();
            Components = new() { scoreCounter, coinCounter, resultCounter, quitToMenuButton, gameButton };
            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }

        public void Update()
        {
            foreach (var component in Components)
                component.Update();

            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }

        public void SetCounts(double scoreCount, int coinCount)
        {
            ScoreCount = scoreCount;
            CoinCount = coinCount;
            Result = scoreCount + coinCount * 100;
        }

        public void ChangePlayerSpeed(Direction dir) { }
    }
}
