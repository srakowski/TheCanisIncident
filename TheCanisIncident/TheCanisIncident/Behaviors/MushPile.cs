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
using TheCanisIncident.Models;

namespace TheCanisIncident.Behaviors
{
    class MushPile : Behavior
    {
        private int _hp = 3;

        private static Random _rand = new Random();

       
        private GameObject _player;

        private bool[,] _layout;

        private EnemySpawn _enemySpawn;

        public MushPile(GameObject player, bool[,] layout, EnemySpawn enemySpawn)
        {
            StartCoroutine(SpawnEnemy(_rand.Next(100, 2000)));
            _player = player;
            _layout = layout;
            _enemySpawn = enemySpawn;
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "bullet")
            {
                _hp--;
                if (_hp <= 0)
                {
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
                if (Enemy.TotalEnemies < Enemy.MaxEnemies && Vector2.Distance(_player.Transform.Position, this.Transform.Position) < 1000)
                {
                    Enemy.TotalEnemies++;
                    AddGameObject("enemy")
                        .SetPosition(this.Transform.Position + new Vector2(_rand.Next(-20, 20), _rand.Next(-20, 20)))
                        .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>(_enemySpawn.Texture)))
                        .AddComponent(new BoxCollider(_enemySpawn.Width, _enemySpawn.Height) { IsDynamic = true })
                        .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/hit")))
                        .AddComponent(_enemySpawn.GetBehavior(_player, _layout));
                }
                yield return WaitMSecs(_rand.Next(_enemySpawn.MinSpawnRate, _enemySpawn.MaxSpawnRate));
            }
        }

        private void DropItem()
        {
            AddGameObject("pickup")
                .SetPosition(this.Transform.Position + new Vector2(_rand.Next(-60, 60), _rand.Next(-60, 60)))
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/item")))
                .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/pickup")))
                .AddComponent(new PickupItem(_player))
                .AddComponent(new BoxCollider(12, 12));
        }
    }
}
