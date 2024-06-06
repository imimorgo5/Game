using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RoadRacesMVP
{
    public class GameState : State
    {
        public override event EventHandler<GameplayEventArgs> UpdatedState = delegate { };
        public event EventHandler<GameOverArgs> GameOver = delegate { };
        public event EventHandler<SoundType> PlaySound = delegate { };

        private int LineCount { get; set; }
        private int CurrentId { get; set; }
        private int Shift { get; set; }
        private Vector2 POVShift { get; set; }
        private Vector2 PositionOffset { get; set; }
        private Queue<char[]> Map { get; set; }
        private int PlayerId { get; set; }
        public Dictionary<int, ISolid> SolidObjects { get; private set; }
        private double ScoreCount { get; set; }
        private PauseArgs PauseArgs { get; set; }

        public GameState() : this(null) { }

        public GameState(PauseArgs pauseArgs)
        {
            PauseArgs = pauseArgs;
            Map = new();
            Objects = new();
            SolidObjects = new();
            LineCount = 0;
            CurrentId = 1;
            PlayerId = 0;
            Shift = 0;

            var pauseButton = new Button(new(25, 30), 200, 120, (byte)ObjectTypes.bigButton, ActionType.pause, "Пауза");

            var recordCounter = new Counter(new(25, 930), 420, 120, (byte)ObjectTypes.counter, ActionType.scoreCount, "Рекорд: ");
            recordCounter.ScoreFunction = () => recordCounter.Count = GameSettings.RecordScoreCount;

            var scoreCounter = new Counter(new(1545, 30), 350, 120, (byte)ObjectTypes.counter, ActionType.scoreCount, "Счёт: ");
            scoreCounter.ScoreFunction = () => scoreCounter.Count = ScoreCount;

            var coinsCounter = new Counter(new(1545, 165), 350, 120, (byte)ObjectTypes.counter, ActionType.scoreCount, "Монет: ");
            coinsCounter.ScoreFunction = () => coinsCounter.Count = (Objects[PlayerId] as Player).CollectedCoinsCount;

            var healthCounter = new Counter(new(1545, 300), 350, 120, (byte)ObjectTypes.counter, ActionType.scoreCount, "Жизней: ");
            healthCounter.ScoreFunction = () => healthCounter.Count = 3 - (Objects[PlayerId] as Player).CollisionCount;

            Components = new() { pauseButton, recordCounter, scoreCounter, coinsCounter, healthCounter };
        }

        public override void Initialize()
        {
            if (PauseArgs != null)
                InitializeFromPause(PauseArgs);
            else
                BaseInitialize();
        }

        private void BaseInitialize()
        {
            var playerRow = 11;
            var playerPosition = 15;
            var lineCount = 2160 / GameConstants.TileSize;
            ScoreCount = 0;            

            for (var i = 0; i < lineCount; i++)
            {
                LineCount++;
                var line = new char[GameConstants.RowCount];
                line[GameConstants.LeftWallPosition] = 'W';
                line[GameConstants.RightWallPosition] = 'W';

                if (i == playerRow)
                    line[playerPosition] = 'P';

                Map.Enqueue(line);
            }

            AddToObjects();

            POVShift = new(playerPosition * GameConstants.TileSize, ScreenSize.ScreenHeight);
            PositionOffset = new(0, ScreenSize.ScreenHeight - playerRow * GameConstants.TileSize + Objects[PlayerId].Height + GameConstants.TileSize);
            UpdatedState.Invoke(this, new() { Objects = Objects, Components = Components, POVShift = POVShift, PositionOffset = PositionOffset });
        }

        private void InitializeFromPause(PauseArgs args)
        {
            ScoreCount = args.ScoreCount;
            POVShift = args.POVShift + new Vector2(0, ScreenSize.ScreenHeight);
            PositionOffset = args.PositionOffset;

            foreach (var obj in args.GameObjects)
            {
                if (CurrentId == PlayerId)
                    CurrentId = (CurrentId + 1) % 10000;

                Objects.Add(CurrentId, obj);

                if (obj is Player)
                    PlayerId = CurrentId;

                if (obj is ISolid s)
                    SolidObjects.Add(CurrentId, s);

                CurrentId = (CurrentId + 1) % 10000;
            }

            UpdatedState.Invoke(this, new() { Objects = Objects, Components = Components, POVShift = POVShift, PositionOffset = PositionOffset });
        }

        public PauseArgs GetPauseArgs() => new() { GameObjects = Objects.Values.ToHashSet(), ScoreCount = ScoreCount, POVShift = POVShift, PositionOffset = PositionOffset };

        private void GenerateLines(int count)
        {
            for (var i = 0; i < count; i++)
            {
                LineCount = (LineCount + 1) % 100;
                var line = new char[GameConstants.RowCount];
                line[GameConstants.LeftWallPosition] = 'W';
                line[GameConstants.RightWallPosition] = 'W';

                var frequencyOfGenerationCar = 40 - 2 * GameSettings.Difficult;
                var frequencyOfGenerationObjects = 20 + 3 * GameSettings.Difficult;

                if (LineCount % frequencyOfGenerationCar == 0)
                    line[GameConstants.ObjectsPositions[new Random().Next(0, GameConstants.ObjectsPositions.Length)]] = 'C';

                if (LineCount % frequencyOfGenerationObjects == 0)
                {
                    var index = new Random().Next(0, GameConstants.ObjectsPositions.Length);
                    while (line[index] == 'C')
                        index = new Random().Next(0, GameConstants.ObjectsPositions.Length);

                    line[GameConstants.ObjectsPositions[index]] = GameConstants.ObjectsChar[new Random().Next(0, GameConstants.ObjectsChar.Length)];
                }

                Map.Enqueue(line);
            }
        }

        private void AddToObjects()
        {
            var y = Map.Count;

            if (Objects.Count > 0)
            {
                foreach (var index in Objects.Keys)
                {
                    if (Objects[index].Position.Y > ScreenSize.ScreenHeight * 2 || Objects[index].Position.Y < 0)
                    {
                        if (SolidObjects.ContainsKey(index))
                            SolidObjects.Remove(index);

                        Objects.Remove(index);
                    }
                }
            }

            while (y != 0)
            {
                var currentRow = Map.Dequeue();

                for (var x = 0; x < currentRow.Length; x++)
                {
                    if (currentRow[x] != '\0')
                    {
                        if (CurrentId == PlayerId)
                            CurrentId = (CurrentId + 1) % 10000;

                        var generatedObject = Factory.GenerateObject(currentRow[x], x, y - 1);

                        if (generatedObject is Player player)
                        {
                            PlayerId = CurrentId;
                            player.PlaySound += PlaySound;
                        }

                        Objects.Add(CurrentId, generatedObject);
                        if (generatedObject is ISolid s)
                            SolidObjects.Add(CurrentId, s);

                        CurrentId = (CurrentId + 1) % 10000;
                    }
                }
                y--;
            }
        }

        public override void Update()
        {
            var collisionObjects = new Dictionary<int, Vector2>();

            var playerInitPosition = Objects[PlayerId].Position;
            var playerShift = new Vector2(0, -Objects[PlayerId].Speed.Y);

            ScoreCount += Objects[PlayerId].Speed.Y * -0.01 % 1000000000;

            foreach (var c in Components)
                c.Update();

            foreach (var i in Objects.Keys)
            {
                var initPos = Objects[i].Position + playerShift;
                Objects[i].Update(-playerShift, Objects.Values.ToHashSet());

                if (Objects[i] is CollectedObject collectedObj && collectedObj.IsCollected)
                    Objects.Remove(i);

                if (SolidObjects.ContainsKey(i))
                    collisionObjects.Add(i, initPos);
            }

            if (Objects[PlayerId].Speed.Y >= -1)
                foreach (var obj in Objects.Values.Where(o => o is Car))
                    obj.Speed = Vector2.Zero;

            foreach (var i in collisionObjects.Keys)
                foreach (var j in collisionObjects.Keys)
                    if (i != j)
                        CalculateCollision((collisionObjects[i], i), (collisionObjects[j], j));

            if ((Objects[PlayerId] as Player).CollisionCount == 3)
            {
                GameOver.Invoke(this, new GameOverArgs() { ScoreCount = ScoreCount, CoinCount = (Objects[PlayerId] as Player).CollectedCoinsCount });
                return;
            }

            var positionOffset = Objects[PlayerId].Position - playerInitPosition;
            playerShift -= new Vector2(0, positionOffset.Y);

            POVShift = new(POVShift.X + playerShift.X, (POVShift.Y + playerShift.Y) % ScreenSize.ScreenHeight);
            PositionOffset = new(PositionOffset.X, (PositionOffset.Y + positionOffset.Y) % ScreenSize.ScreenHeight);

            Shift += (int)Math.Abs(playerShift.Y);
            if (Shift >= GameConstants.TileSize)
            {
                GenerateLines((Shift + GameConstants.TileSize / 2) / GameConstants.TileSize);
                AddToObjects();
                Shift = 0;
            }

            UpdatedState.Invoke(this, new() { Objects = Objects, Components = Components, POVShift = playerShift, PositionOffset = PositionOffset });
        }

        private void CalculateCollision((Vector2 initPos, int Id) obj1, (Vector2 initPos, int Id) obj2)
        {
            var isCollided = false;
            if (Objects[obj1.Id] is ISolid sObj1 && Objects[obj2.Id] is ISolid sObj2)
            {
                if (sObj1.IsColision && sObj2.IsColision || sObj1 is Wall || sObj2 is Wall)
                {
                    Vector2 offset;
                    while (RectangleCollider.IsCollided(sObj1.Collider, sObj2.Collider))
                    {
                        isCollided = true;
                        if (obj1.initPos != Objects[obj1.Id].Position)
                        {
                            offset = Objects[obj1.Id].Position - obj1.initPos;
                            Objects[obj1.Id].Move(Objects[obj1.Id].Position - offset);
                        }
                        if (obj2.initPos != Objects[obj2.Id].Position)
                        {
                            offset = Objects[obj2.Id].Position - obj2.initPos;
                            Objects[obj2.Id].Move(Objects[obj2.Id].Position - offset);
                        }
                    }
                }
            }
            if (isCollided)
            {                
                Objects[obj1.Id].Speed = Vector2.Zero;
                Objects[obj2.Id].Speed = Vector2.Zero;

                if ((obj1.Id == PlayerId || obj2.Id == PlayerId) && (Objects[PlayerId] as ISolid).IsColision)
                {
                    PlaySound.Invoke(this, SoundType.Hit);
                    var player = Objects[PlayerId] as Player;
                    player.CollisionCount++;
                    player.BecomeNotCollision();
                }
            }            
        }

        public void ChangePlayerSpeed(Direction dir)
        {
            var player = Objects[PlayerId] as Player;
            var turnValue = 20;

            if (player.IsReverseSteering)
                turnValue = -20;

            switch (dir)
            {
                case Direction.right:
                    player.Speed += new Vector2(turnValue, 0);
                    break;
                case Direction.left:
                    player.Speed += new Vector2(-turnValue, 0);
                    break;
            }
        }
    }
}
