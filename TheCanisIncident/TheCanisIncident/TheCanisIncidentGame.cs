using System;
using Coldsteel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Coldsteel.Controls;
using TheCanisIncident.Stages;

namespace TheCanisIncident.WindowsDX
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TheCanisIncidentGame : Game, IColdsteelInitializer
    {
        GraphicsDeviceManager _graphics;
        public TheCanisIncidentGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            Content.RootDirectory = "Content";
            Components.Add(new ColdsteelComponent(this, this));
        }

        public void InitializeControls(Input input)
        {
            input.AddControl("MoveUp", new KeyboardButtonControl(Keys.W));
            input.AddControl("MoveDown", new KeyboardButtonControl(Keys.S));
            input.AddControl("MoveLeft", new KeyboardButtonControl(Keys.A));
            input.AddControl("MoveRight", new KeyboardButtonControl(Keys.D));
        }

        public void RegisterStages(GameStageCollection stages)
        {
            stages.RegisterStage<MainMenuStage>();
        }
    }
}
