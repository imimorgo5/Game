using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.IO;

namespace RoadRacesMVP
{
    public static class GameSettings
    {
        private const string PATH = "GameSettings.json";
        public static int Difficult;
        public static double Volume;
        public static double RecordScoreCount;

        public static void Save()
        {
            var setup = new GameSetup()
            {
                Difficult = Difficult,
                Volume = Volume,
                RecordScoreCount = RecordScoreCount
            };

            var serializedText = JsonSerializer.Serialize<GameSetup>(setup);
            Trace.WriteLine(serializedText);
            File.WriteAllText(PATH, serializedText);
        }

        public static void Load()
        {
            if (!File.Exists(PATH))
                Create();

            var serializedText = File.ReadAllText(PATH);
            var setup = JsonSerializer.Deserialize<GameSetup>(serializedText);
            Difficult = setup.Difficult;
            Volume = setup.Volume;
            RecordScoreCount = setup.RecordScoreCount;
        }

        private static void Create()
        {
            var setup = new GameSetup()
            {
                Difficult = 5,
                Volume = 5,
                RecordScoreCount = 0
            };

            var serializedText = JsonSerializer.Serialize<GameSetup>(setup);
            Trace.WriteLine(serializedText);
            File.WriteAllText(PATH, serializedText);
        }
    }
}
