using Coldsteel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework;

namespace TheCanisIncident.Behaviors
{
    class Remains : Behavior
    {
        private static Random _rand = new Random();

        public Remains()
        {
            StartCoroutine(Explode());
        }

        private IEnumerator Explode()
        {
            yield return null;
            var renderer = new SpriteRenderer(GetLayer("items"), GetContent<Texture2D>("sprites/xplod"));
            this.GameObject.AddComponent(renderer);
            yield return null;
            this.GameObject.AddChild(new GameObject().SetPosition(_rand.Next(-100, 100), _rand.Next(-100, 100))
                .AddComponent(new SpriteRenderer(GetLayer("items"), GetContent<Texture2D>("sprites/gib1")) { Color = Color.Black }));
            this.GameObject.AddChild(new GameObject().SetPosition(_rand.Next(-100, 100), _rand.Next(-100, 100))
                .AddComponent(new SpriteRenderer(GetLayer("items"), GetContent<Texture2D>("sprites/gib2")) { Color = Color.Black }));
            yield return null;
            renderer.Color = Color.Black;
            renderer.Texture = GetContent<Texture2D>("sprites/stain");
        }
    }
}
