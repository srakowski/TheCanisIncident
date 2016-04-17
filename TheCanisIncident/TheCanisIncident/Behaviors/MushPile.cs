using Coldsteel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Coldsteel.Renderers;
using Coldsteel.Colliders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace TheCanisIncident.Behaviors
{
    class MushPile : Behavior
    {
        private int _hp = 3;

        private static Random _rand = new Random();

        public int MinSpawnRate { get; set; } = 1000;

        public int MaxSpawnRate { get; set; } = 20000;

        private GameObject _player;

        private bool[,] _layout;

        public MushPile(GameObject player, bool[,] layout)
        {
            StartCoroutine(SpawnEnemy(_rand.Next(100, 2000)));
            _player = player;
            _layout = layout;
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "bullet")
            {
                _hp--;
                if (_hp <= 0)
                {
                    if (_rand.Next(101) < 75)
                        DropItem();

                    GetComponent<AudioSource>().Play();
                    Destroy();
                }
            }
        }

        private IEnumerator SpawnEnemy(int wait)
        {
            yield return WaitMSecs(wait);
            while (true)
            {
                AddGameObject("enemy")
                    .SetPosition(this.Transform.Position + new Vector2(_rand.Next(-20, 20), _rand.Next(-20, 20)))
                    .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/enemy")))
                    .AddComponent(new BoxCollider(80, 80) { IsDynamic = true })
                    .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/hit")))
                    .AddComponent(new Enemy(_player, _layout));

                yield return WaitMSecs(_rand.Next(MinSpawnRate, MaxSpawnRate));
            }
        }

        private void DropItem()
        {
            AddGameObject("pickup")
                .SetPosition(this.Transform.Position)
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/item")))
                .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/pickup")))
                .AddComponent(new PickupItem())
                .AddComponent(new BoxCollider(24, 24));
        }
    }
}
