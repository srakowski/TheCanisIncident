using Coldsteel;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TheCanisIncident.Behaviors;

namespace TheCanisIncident.Stages
{
    class EndStage : GameStage
    {
        protected override void LoadContent()
        {
            LoadContent<Texture2D>("sprites/fail");
        }

        protected override void Initialize()
        {
            AddGameObject().AddComponent(new Camera());
            AddGameObject()
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/fail")));

            AddGameObject()
                .AddComponent(new CutScene("MainMenuStage", this.Param));
        }
    }
}
