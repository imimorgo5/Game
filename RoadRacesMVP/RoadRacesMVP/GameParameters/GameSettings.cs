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
        public static int Difficult = 5;
        public static double Volume = 5;
        public static double RecordScoreCount = 0;

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
                Save();

            var serializedText = File.ReadAllText(PATH);
            var setup = JsonSerializer.Deserialize<GameSetup>(serializedText);
            Difficult = setup.Difficult;
            Volume = setup.Volume;
            RecordScoreCount = setup.RecordScoreCount;
        }
    }
}
