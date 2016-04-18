using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Coldsteel;
using Microsoft.Xna.Framework;
using Coldsteel.Colliders;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework.Graphics;

namespace TheCanisIncident.Behaviors.Enemies
{
    class NarwhalHorse : Enemy
    {
        bool _firing = false;
        bool _first = true;

        public NarwhalHorse(GameObject player, bool[,] layout) : base(player, layout, 5)
        {
            this.MovementSpeed = ((float)_rand.NextDouble() + 0.1f) * 0.3f;            
        }

        public override void Update(IGameTime gameTime)
        {
            if (_firing)
                return;

            if (_first)
            { 
                StartCoroutine(FireHorn(Player.Transform.Position - this.Transform.Position));
                _first = false;
            }

            this.MoveTowardsPlayerIfGreaterThan(gameTime, 100);            
        }

        private IEnumerator FireHorn(Vector2 direction)
        {
            while (true)
            {
                _firing = true;
                var bullet = new GameObject("rainbow")
                    .SetPosition(Transform.Position + new Vector2(96, 0))
                    .AddComponent(new Bullet(direction, _layout, 2000, 7, 1f))
                    .AddComponent(new BoxCollider(24, 24).SetIsDynamic(true))
                    .AddComponent(new SpriteRenderer(GetLayer("items"), GetContent<Texture2D>("sprites/rainbowbullet")));
                AddGameObject(bullet);
                yield return WaitMSecs(_rand.Next(1000));
                _firing = false;
                yield return WaitMSecs(_rand.Next(1000));
            }
        }
    }
}
