using RoadRacesMVP;
using System;

public static class Program
{
    [STAThread]
    static void Main()
    {        
        GameplayPresenter game = new GameplayPresenter(new MainMenuState(), new RoadRacesView());
        game.LaunchGame();
    }
}
