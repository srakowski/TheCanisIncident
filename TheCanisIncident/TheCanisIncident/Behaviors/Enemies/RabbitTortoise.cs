using System;
using System.Collections.Generic;
using System.Text;
using Coldsteel;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Coldsteel.Renderers;
using Coldsteel.Colliders;
using Microsoft.Xna.Framework.Audio;

namespace TheCanisIncident.Behaviors.Enemies
{
    class RabbitTortoise : Enemy
    {
        private bool _moving = false;

        private IGameTime _gameTime;

        public RabbitTortoise(GameObject player, bool[,] layout) : base(player, layout, 10)
        {
            this.MovementSpeed = ((float)_rand.NextDouble() + 0.1f) * 0.1f;
        }

        public override void Update(IGameTime gameTime)
        {
            _gameTime = gameTime;

            if (_moving)
                return;

            if (!this.MoveTowardsPlayerIfGreaterThan(gameTime, 300))
                StartCoroutine(MoveTo(this.Transform.Position + new Vector2(_rand.Next(-99, 100), _rand.Next(-99, 100))));
        }

        private IEnumerator MoveTo(Vector2 pos)
        {
            _moving = true;
            float time = 0;
            while (time < 1000f)
            {
                time += _gameTime?.Delta ?? 0;
                var t = time / 1000f;
                this.Transform.Position = Vector2.SmoothStep(this.Transform.Position, pos, t);
                yield return null;
            }
            this.Transform.Position = pos;
            yield return WaitMSecs(_rand.Next(2000));
            _moving = false;
        }

        public override void WhenKilled()
        {            
            var numToSpawn = _rand.Next(2, 6);
            for (var x = 0; x < numToSpawn; x++)
            {
                if (Enemy.TotalEnemies < Enemy.MaxEnemies)
                {
                    Enemy.TotalEnemies++;
                    AddGameObject("enemy")
                        .SetPosition(this.Transform.Position + new Vector2(_rand.Next(-90, 90), _rand.Next(-90, 90)))
                        .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/littletr")))
                        .AddComponent(new BoxCollider(24, 24) { IsDynamic = true })
                        .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/hit")))
                        .AddComponent(new LittleRabitTortoise(Player, _layout));
                }
            }
        }
    }
}
