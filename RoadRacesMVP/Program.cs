using RoadRacesMVP;
using System;

public static class Program
{
    [STAThread]
    static void Main()
    {        
        GameplayPresenter game = new(new RoadRacesView(), new MainMenuState());
        game.LaunchGame();
    }
}
