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
            //IsMouseVisible = true;
        }

        public void InitializeControls(Input input)
        {
            input.AddControl("MoveUp", new KeyboardButtonControl(Keys.W));
            input.AddControl("MoveDown", new KeyboardButtonControl(Keys.S));
            input.AddControl("MoveLeft", new KeyboardButtonControl(Keys.A));
            input.AddControl("MoveRight", new KeyboardButtonControl(Keys.D));
            input.AddControl("Aim", new MouseDirectionalControl(new Vector2(
                GraphicsDevice.PresentationParameters.Bounds.Width * 0.5f,
                GraphicsDevice.PresentationParameters.Bounds.Height * 0.5f
                )));

            input.AddControl("Fire", new MouseButtonControl(MouseButton.Left));
            input.AddControl("Pointer", new MousePositionalControl());
            input.AddControl("PointerClick", new MouseButtonControl(MouseButton.Left));
        }

        public void RegisterStages(GameStageCollection stages)
        {
            stages.RegisterStage<GeneratedLevelStage>();
            stages.RegisterStage<MainMenuStage>();
            stages.RegisterStage<Paper1Stage>();
            stages.RegisterStage<Paper2Stage>();
            stages.RegisterStage<ResolveStage>();
            stages.RegisterStage<EntranceStage>();            
            stages.RegisterStage<InitialLabStage>();
        }
    }
}
