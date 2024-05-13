using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace RoadRacesMVP
{
    public class GameState : IGameplayModel, IState
    {
        public event EventHandler<GameplayEventArgs> Updated = delegate { };
        public event EventHandler<GameplayEventArgs> UpdatedState = delegate { };

        private readonly int TileSize = 120;
        private int RowCount = 0;
        private int CurrentId { get; set; }
        private int Shift { get; set; }
        private Queue<HashSet<int>> ObjectsIndexes { get; set; }
        public Queue<char[]> Map { get; set; }
        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }
        public Dictionary<int, ISolid> SolidObjects { get; set; }
        public List<IComponent> Components { get; set; }

        public void Initialize()
        {
            Map = new();
            Objects = new();
            Components = new();
            SolidObjects = new();
            ObjectsIndexes = new();
            CurrentId = 1;
            Shift = 0;

            var playerRow = 5;
            for (var i = 0; i < 18; i++)
            {
                var row = new char[16];
                row[2] = 'W';
                row[13] = 'W';

                if (i == playerRow)
                    row[7] = 'P';

                Map.Enqueue(row);
            }

            AddToObjects();

            var quitToMenuButton = new Button(new Vector2(50, 50), 450, 125, (byte)ObjectTypes.button, ActionType.quitToMenu, "Выйти");
            var scoreCounter = new Counter(new Vector2(1470, 50), 400, 125, (byte)ObjectTypes.counter, ActionType.scoreCount, "Счёт: ");
            Components.Add(quitToMenuButton);
            Components.Add(scoreCounter);

            Updated.Invoke(this, new GameplayEventArgs()
            {
                Objects = Objects,
                Components = Components,
                POVShift = new Vector2(Objects[PlayerId].Position.X + TileSize / 2, 1080),
                PositionOffset = new Vector2(0, 1080 - (playerRow - 2) * TileSize)
            });
        }

        private void GenerateRows(int count)
        {
            var CarPositions = new int[] { 4, 5, 6, 9, 10, 11 };
            for (var i = 0; i < count; i++)
            {
                RowCount = (RowCount + 1) % 100;
                var row = new char[16];
                row[2] = 'W';
                row[13] = 'W';

                if (RowCount % 10 == 0)
                    row[CarPositions[new Random().Next(0, CarPositions.Length)]] = 'C';

                Map.Enqueue(row);
            }
        }

        private void AddToObjects()
        {
            var y = Map.Count;

            for (var i = 0; i < Map.Count; i++)
            {
                if (ObjectsIndexes.Count > 0)
                {
                    var indexes = ObjectsIndexes.Dequeue();
                    foreach (var index in indexes)
                    {
                        if (index == PlayerId)
                            continue;

                        Objects.Remove(index);

                        if (SolidObjects.ContainsKey(index))
                            SolidObjects.Remove(index);
                    }
                }
            }

            while (y != 0)
            {
                var currentRow = Map.Dequeue();
                var objectsIndexes = new HashSet<int>();

                for (var x = 0; x < currentRow.Length; x++)
                {
                    if (currentRow[x] != '\0')
                    {
                        if (CurrentId == PlayerId)
                            CurrentId = (CurrentId + 1) % 1000;

                        var generatedObject = GenerateObject(currentRow[x], x, y - 1);

                        if (currentRow[x] == 'P')
                            PlayerId = CurrentId;

                        Objects.Add(CurrentId, generatedObject);
                        if (generatedObject is ISolid s)
                            SolidObjects.Add(CurrentId, s);

                        objectsIndexes.Add(CurrentId);

                        CurrentId = (CurrentId + 1) % 1000;
                    }
                }
                ObjectsIndexes.Enqueue(objectsIndexes);
                y--;
            }
        }

        private IObject GenerateObject(char sign, float xTile, float yTile)
        {
            var x = xTile * TileSize;
            var y = yTile * TileSize;
            IObject generatedObject = null;

            if (sign == 'C')
                generatedObject = Factory.CreateClassicCar(x + TileSize / 2, y + TileSize / 2, new Vector2(0, 0));
            else if (sign == 'P')
                generatedObject = Factory.CreatePlayerCar(x + TileSize / 2, y + TileSize / 2, new Vector2(0, 0));
            else if (sign == 'W')
                generatedObject = Factory.CreateWall(x + TileSize / 2, y + TileSize / 2);

            return generatedObject;
        }

        public void Update()
        {            
            var collisionObjects = new Dictionary<int, Vector2>();
            var playerPosition = Objects[PlayerId].Position;

            foreach (var c in Components)
            {
                if (c is Counter)
                    c.Count = (c.Count + Objects[PlayerId].Speed.Y * -0.01) % 1000000000;

                c.Update();
            }

            var playerShift = new Vector2(0, -Objects[PlayerId].Speed.Y);

            foreach (var i in Objects.Keys)
            {
                var initPos = Objects[i].Position + playerShift;
                Objects[i].Update(playerShift);

                if (SolidObjects.ContainsKey(i))
                    collisionObjects.Add(i, initPos);
            }

            foreach (var i in collisionObjects.Keys)
            {
                foreach (var j in collisionObjects.Keys)
                {
                    if (i == j)
                        continue;
                    CalculateObstacleCollision((collisionObjects[i], i), (collisionObjects[j], j));
                }
            }

            var positionOffset = Objects[PlayerId].Position - playerPosition;
            playerShift -= new Vector2(0, positionOffset.Y);

            Shift += (int)Math.Abs(playerShift.Y);
            if (Shift >= TileSize)
            {
                GenerateRows((Shift + TileSize / 2) / TileSize);
                AddToObjects();
                Shift = 0;
            }

            Updated.Invoke(this, new GameplayEventArgs { Objects = Objects, Components = Components, POVShift = playerShift, PositionOffset = positionOffset });
        }

        private void CalculateObstacleCollision((Vector2 initPos, int Id) obj1, (Vector2 initPos, int Id) obj2)
        {
            var isCollided = false;
            if (Objects[obj1.Id] is ISolid && Objects[obj2.Id] is ISolid)
            {
                Vector2 oppositeDirection;
                while (RectangleCollider.IsCollided(SolidObjects[obj1.Id].Collider, SolidObjects[obj2.Id].Collider))
                {
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
            if (isCollided)
            {
                Objects[obj1.Id].Speed = new Vector2(0, 0);
                Objects[obj2.Id].Speed = new Vector2(0, 0);
            }
        }

        private Vector2 GetOffset(Vector2 oppositeDirection)
        {
            Vector2 offset;
            if (oppositeDirection.X == 0)
                offset = new Vector2(oppositeDirection.X, oppositeDirection.Y / Math.Abs(oppositeDirection.Y));
            else if (oppositeDirection.Y == 0)
                offset = new Vector2(oppositeDirection.X / Math.Abs(oppositeDirection.X), oppositeDirection.Y);
            else
                offset = new Vector2(oppositeDirection.X / Math.Abs(oppositeDirection.X), oppositeDirection.Y / Math.Abs(oppositeDirection.Y));
            return offset;
        }

        public void ChangePlayerSpeed(Direction dir)
        {
            var p = Objects[PlayerId];
            switch (dir)
            {
                case Direction.right:
                {
                    p.Speed += new Vector2(20, 0);
                    break;
                }
                case Direction.left:
                {
                    p.Speed += new Vector2(-20, 0);
                    break;
                }
            }
        }
    }
}
