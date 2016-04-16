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
    class Paper2Stage : GameStage
    {
        protected override void LoadContent()
        {
            LoadContent<Texture2D>("sprites/paper2");
        }

        protected override void Initialize()
        {
            BackgroundColor = new Color(35, 35, 45);
            AddGameObject().AddComponent(new Camera());
            AddGameObject()
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/paper2")));

            AddGameObject()
                .AddComponent(new CutScene("ResolveStage"));
        }
    }
}
