using Coldsteel;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TheCanisIncident.Behaviors;

namespace TheCanisIncident.Stages
{
    class Paper1Stage : GameStage
    {
        protected override void LoadContent()
        {
            LoadContent<Texture2D>("sprites/paper1");
        }

        protected override void Initialize()
        {
            AddGameObject().AddComponent(new Camera());
            AddGameObject()
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/paper1")));

            AddGameObject()
                .AddComponent(new CutScene("InitialLabStage", this.Param));
        }
    }
}
