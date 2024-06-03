using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadRacesMVP
{
    public class MainMenuStateView : StateView
    {
        public MainMenuStateView(Vector2 visualShift) : base(visualShift) 
        {
            FontSize = FontSize.Size45;
            ViewType = ViewType.MainMenuStateView;
        }
    }
}