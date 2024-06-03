using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoadRacesMVP
{
    public class GameOverState : State
    {
        public override event EventHandler<GameplayEventArgs> UpdatedState = delegate { };

        private double ScoreCount { get; set; }
        private int CoinCount { get; set; }
        private double Result { get; set; }


        public override void Initialize()
        {
            var scoreCounter = new Counter(new(985, 415), 350, 100, (byte)ObjectTypes.counter, ActionType.scoreCount, string.Empty);
            scoreCounter.ScoreFunction = () => scoreCounter.Count = ScoreCount;

            var coinCounter = new Counter(new(1135, 565), 200, 100, (byte)ObjectTypes.counter, ActionType.scoreCount, string.Empty);
            coinCounter.ScoreFunction = () => coinCounter.Count = CoinCount;

            var resultCounter = new Counter(new(985, 715), 350, 100, (byte)ObjectTypes.counter, ActionType.scoreCount, string.Empty);
            resultCounter.ScoreFunction = () => resultCounter.Count = Result;

            var quitToMenuButton = new Button(new(550, 910), 410, 120, (byte)ObjectTypes.bigButton, ActionType.quitToMenuFromGame, "Выйти в меню");
            var gameButton = new Button(new(975, 910), 410, 120, (byte)ObjectTypes.bigButton, ActionType.startGame, "Сыграть ещё");

            Objects = new();
            Components = new() { scoreCounter, coinCounter, resultCounter, quitToMenuButton, gameButton };
            UpdatedState.Invoke(this, new() { Components = Components, Objects = Objects, POVShift = Vector2.Zero });
        }

        public void SetCounts(double scoreCount, int coinCount)
        {
            ScoreCount = scoreCount;
            CoinCount = coinCount;
            Result = scoreCount + coinCount * 100;

            if (Result > GameSettings.RecordScoreCount)
                GameSettings.RecordScoreCount = Result;
        }
    }
}
