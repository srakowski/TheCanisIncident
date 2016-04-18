using Coldsteel;
using Coldsteel.Colliders;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TheCanisIncident.Stages;

namespace TheCanisIncident.Behaviors
{
    abstract class Enemy : Behavior
    {
        public bool IsBoss { get; set; } = false;

        public static int TotalEnemies;
        public static int MaxEnemies = 32;

        protected static Random _rand = new Random();

        protected int HP { get; set; } 

        private Vector2 _previousPosition;

        public GameObject Player { get; set; }

        protected virtual float MovementSpeed { get; set; } = 0.1f;

        protected bool[,] _layout;

        private Vector2 _offset = new Vector2(48, 48);

        public Enemy(GameObject player, bool[,] layout, int hp  )
        {
            this.Player = player;
            _layout = layout;
            this.HP = hp;
        }

        public override void Update(IGameTime gameTime)
        {
            MoveTowardsPlayerIfGreaterThan(gameTime, 48);
        }

        protected bool MoveTowardsPlayerIfGreaterThan(IGameTime gameTime, int distance)
        {
            _previousPosition = Transform.Position;
            if (Vector2.Distance(Transform.Position, Player.Transform.Position) > distance)
            {
                var velocity = Player.Transform.Position - Transform.Position;
                velocity.Normalize();
                velocity *= MovementSpeed * gameTime.Delta;
                Transform.Position += new Vector2(velocity.X, 0);
                AdjustWorldCollision();
                _previousPosition.X = Transform.Position.X;
                Transform.Position += new Vector2(0, velocity.Y);
                AdjustWorldCollision();
                return true;
            }

            return false;
        }

        protected void AdjustWorldCollision()
        {
            try
            {
                if (this.Transform.Position.X < _previousPosition.X) _offset.X = 30;
                else _offset.X = 66;
                if (this.Transform.Position.Y < _previousPosition.Y) _offset.Y = 44;
                else _offset.Y = 52;
                var pos = this.Transform.Position + _offset;
                var px = (int)pos.X / 96;
                var py = (int)pos.Y / 96;
                if (!_layout[px, py])
                    this.Transform.Position = _previousPosition;
            }
            catch
            {
                Destroy();
            }
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "bullet")
            {
                this.HP--;
                if (this.HP <= 0)
                {
                    GameplayStage.CameraShake.Shake();
                    collision.GameObject.Tag = null; // only I will be hit by this bullet
                    KillMe();
                }
            }
            else if (collision.GameObject.Tag == "player")
            {
                KillMe();
            }
        }

        private void KillMe()
        {
            GetComponent<AudioSource>().Play();
            AddGameObject().SetPosition(this.Transform.Position).AddComponent(new Remains());
            DropItem();            
            WhenKilled();
            TotalEnemies--;
            Destroy();
        }

        private void DropItem()
        {
            AddGameObject("pickup")
                .SetPosition(this.Transform.Position + new Vector2(_rand.Next(-20, 20), _rand.Next(-20, 20)))
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/item")))
                .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/pickup")))
                .AddComponent(new PickupItem(Player, IsBoss))
                .AddComponent(new BoxCollider(12, 12));
        }

        public virtual void WhenKilled() { }
    }
}
