using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RoadRacesMVP
{
    public class GameState : IState
    {
        public event EventHandler<GameplayEventArgs> UpdatedState = delegate { };
        public event EventHandler<GameOverArgs> GameOver = delegate { };

        public const int TileSize = 60;
        public const int LeftWallPosition = 6;
        public const int RightWallPosition = 28;
        public const int RowCount = 1920 / TileSize;
        private int LineCount { get; set; }
        private int CurrentId { get; set; }
        private int Shift { get; set; }
        private Vector2 POVShift { get; set; }
        private Vector2 PositionOffset { get; set; }
        private Queue<char[]> Map { get; set; }
        private int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; private set; }
        public Dictionary<int, ISolid> SolidObjects { get; private set; }
        public List<IComponent> Components { get; private set; }
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

            var scoreCounter = new Counter(new(1545, 30), 350, 120, (byte)ObjectTypes.counter, ActionType.scoreCount, "Счёт: ");
            scoreCounter.ScoreFunction = () => scoreCounter.Count = ScoreCount;

            var coinsCounter = new Counter(new(1545, 165), 350, 120, (byte)ObjectTypes.counter, ActionType.scoreCount, "Монет: ");
            coinsCounter.ScoreFunction = () => coinsCounter.Count = (Objects[PlayerId] as Player).CollectedCoinsCount;

            var healthCounter = new Counter(new(1545, 300), 350, 120, (byte)ObjectTypes.counter, ActionType.scoreCount, "Жизней: ");
            healthCounter.ScoreFunction = () => healthCounter.Count = 3 - (Objects[PlayerId] as Player).CollisionCount;

            Components = new() { pauseButton, recordCounter, scoreCounter, coinsCounter, healthCounter };
        }

        public void Initialize()
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
            var lineCount = 2160 / TileSize;
            ScoreCount = 0;            

            for (var i = 0; i < lineCount; i++)
            {
                LineCount++;
                var line = new char[RowCount];
                line[LeftWallPosition] = 'W';
                line[RightWallPosition] = 'W';

                if (i == playerRow)
                    line[playerPosition] = 'P';

                Map.Enqueue(line);
            }

            AddToObjects();

            POVShift = new(playerPosition * TileSize, 1080);
            PositionOffset = new(0, 1080 - playerRow * TileSize + Objects[PlayerId].Height + TileSize);
            UpdatedState.Invoke(this, new() { Objects = Objects, Components = Components, POVShift = POVShift, PositionOffset = PositionOffset });
        }

        private void InitializeFromPause(PauseArgs args)
        {
            ScoreCount = args.ScoreCount;
            POVShift = args.POVShift + new Vector2(0, 1080);
            PositionOffset = args.PositionOffset;

            foreach (var obj in args.GameObjects)
            {
                if (CurrentId == PlayerId)
                    CurrentId = (CurrentId + 1) % 1000;

                Objects.Add(CurrentId, obj);

                if (obj is Player)
                    PlayerId = CurrentId;

                if (obj is ISolid s)
                    SolidObjects.Add(CurrentId, s);

                CurrentId = (CurrentId + 1) % 1000;
            }

            UpdatedState.Invoke(this, new() { Objects = Objects, Components = Components, POVShift = POVShift, PositionOffset = PositionOffset });
        }

        public PauseArgs GetPauseArgs() => new() { GameObjects = Objects.Values.ToHashSet(), ScoreCount = ScoreCount, POVShift = POVShift, PositionOffset = PositionOffset };

        private void GenerateRows(int count)
        {
            var objectsPositions = new int[] { 8, 13, 18, 23 };
            var objects = new char[] { 'S', 'B', 'O', 'R', 'E' };
            for (var i = 0; i < count; i++)
            {
                LineCount = (LineCount + 1) % 100;
                var line = new char[RowCount];
                line[LeftWallPosition] = 'W';
                line[RightWallPosition] = 'W';

                if (LineCount % 20 == 0)
                    line[objectsPositions[new Random().Next(0, objectsPositions.Length)]] = 'C';

                if (LineCount % 30 == 0)
                {
                    var index = new Random().Next(0, objectsPositions.Length);
                    while (line[index] == 'C')
                        index = new Random().Next(0, objectsPositions.Length);

                    line[objectsPositions[index]] = objects[new Random().Next(0, objects.Length)];
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
                    if (Objects[index].Position.Y > 2160)
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
                            CurrentId = (CurrentId + 1) % 1000;

                        var generatedObject = Factory.GenerateObject(currentRow[x], x, y - 1);

                        if (currentRow[x] == 'P')
                            PlayerId = CurrentId;

                        Objects.Add(CurrentId, generatedObject);
                        if (generatedObject is ISolid s)
                            SolidObjects.Add(CurrentId, s);

                        CurrentId = (CurrentId + 1) % 1000;
                    }
                }
                y--;
            }
        }

        public void Update()
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
                Objects[i].Update(playerShift, Objects.Values.ToHashSet());

                if (Objects[i] is CollectedObject collectedObj && collectedObj.IsCollected)
                    Objects.Remove(i);

                if (SolidObjects.ContainsKey(i))
                    collisionObjects.Add(i, initPos);
            }

            if (Objects[PlayerId].Speed.Y >= -1)
                foreach (var obj in Objects.Values.Where(o => o is Car))
                    obj.Speed = Vector2.Zero;

            foreach (var i in collisionObjects.Keys)
            {
                foreach (var j in collisionObjects.Keys)
                {
                    if (i == j)
                        continue;
                    CalculateObstacleCollision((collisionObjects[i], i), (collisionObjects[j], j));
                }
            }

            if ((Objects[PlayerId] as Player).CollisionCount == 3)
                GameOver.Invoke(this, new GameOverArgs() { ScoreCount = ScoreCount, CoinCount = (Objects[PlayerId] as Player).CollectedCoinsCount });

            var positionOffset = Objects[PlayerId].Position - playerInitPosition;
            playerShift -= new Vector2(0, positionOffset.Y);

            POVShift = new(POVShift.X + playerShift.X, (POVShift.Y + playerShift.Y) % 1080);
            PositionOffset = new(PositionOffset.X, (PositionOffset.Y + positionOffset.Y) % 1080);

            Shift += (int)Math.Abs(playerShift.Y);
            if (Shift >= TileSize)
            {
                GenerateRows((Shift + TileSize / 2) / TileSize);
                AddToObjects();
                Shift = 0;
            }

            UpdatedState.Invoke(this, new() { Objects = Objects, Components = Components, POVShift = playerShift, PositionOffset = PositionOffset });
        }

        private void CalculateObstacleCollision((Vector2 initPos, int Id) obj1, (Vector2 initPos, int Id) obj2)
        {
            var isCollided = false;
            if (Objects[obj1.Id] is ISolid sObj1 && Objects[obj2.Id] is ISolid sObj2)
            {
                if (sObj1.IsColision && sObj2.IsColision || sObj1 is Wall || sObj2 is Wall)
                {
                    Vector2 oppositeDirection;
                    while (RectangleCollider.IsCollided(sObj1.Collider, sObj2.Collider))
                    {
                        //Временное решение, пока нет логики перестроения в другой ряд, если впереди более медленная машина
                        if (sObj1 is Car && sObj2 is Car)
                        {
                            if (Objects[obj1.Id].Speed.Y < Objects[obj2.Id].Speed.Y)
                                Objects[obj1.Id].Move(obj1.initPos);
                            else
                                Objects[obj2.Id].Move(obj2.initPos);
                        }
                        //

                        isCollided = true;
                        if (obj1.initPos != Objects[obj1.Id].Position)
                        {
                            oppositeDirection = Objects[obj1.Id].Position - obj1.initPos;
                            Objects[obj1.Id].Move(Objects[obj1.Id].Position - GetOffset(oppositeDirection));
                        }
                        if (obj2.initPos != Objects[obj2.Id].Position)
                        {
                            oppositeDirection = Objects[obj2.Id].Position - obj2.initPos;
                            Objects[obj2.Id].Move(Objects[obj2.Id].Position - GetOffset(oppositeDirection));
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
                    var player = Objects[PlayerId] as Player;
                    player.CollisionCount++;
                    player.BecomeNotCollision();
                }
            }            
        }

        private Vector2 GetOffset(Vector2 oppositeDirection)
        {
            Vector2 offset;
            if (oppositeDirection.X == 0)
                offset = new(oppositeDirection.X, oppositeDirection.Y / Math.Abs(oppositeDirection.Y));
            else if (oppositeDirection.Y == 0)
                offset = new(oppositeDirection.X / Math.Abs(oppositeDirection.X), oppositeDirection.Y);
            else
                offset = new(oppositeDirection.X / Math.Abs(oppositeDirection.X), oppositeDirection.Y / Math.Abs(oppositeDirection.Y));
            return offset;
        }

        public void ChangePlayerSpeed(Direction dir)
        {
            var p = Objects[PlayerId] as Player;
            var turnValue = 20;

            if (p.IsReverseSteering)
                turnValue = -20;

            switch (dir)
            {
                case Direction.right:
                    p.Speed += new Vector2(turnValue, 0);
                    break;
                case Direction.left:
                    p.Speed += new Vector2(-turnValue, 0);
                    break;
            }
        }
    }
}
