using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RoadRacesMVP
{
    public enum ActionType
    {
        startGame,
        quitGame,
        settingsFromMenu,
        settingsFromPause,
        quitToMenu,
        pause,
        resetRecordScore,
        increaseVolume,
        decreaseVolume,
        increaseDifficult,
        decreaseDifficult,
        scoreCount,
        continueGame,
        rules
    }
}
