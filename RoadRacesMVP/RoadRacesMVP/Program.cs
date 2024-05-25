using RoadRacesMVP;
using System;
using System.Text.Json;

public static class Program
{
    [STAThread]
    static void Main()
    {        
        GameplayPresenter game = new(new RoadRacesView(), new MainMenuState());
        game.LaunchGame();
    }
}
