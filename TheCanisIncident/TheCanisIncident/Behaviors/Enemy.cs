using Coldsteel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Behaviors
{
    class Enemy : Behavior
    {
        private Vector2 _previousPosition;

        public GameObject Player { get; set; }

        private float _movementSpeed = 0.1f;

        private bool[,] _layout;

        private Vector2 _offset = new Vector2(48, 48);

        public Enemy(GameObject player, bool[,] layout)
        {
            this.Player = player;
            _layout = layout;
        }

        public override void Update(IGameTime gameTime)
        {
            _previousPosition = Transform.Position;
            if (Vector2.Distance(Transform.Position, Player.Transform.Position) > 48)
            {
                var velocity = Player.Transform.Position - Transform.Position;
                velocity.Normalize();
                velocity *= _movementSpeed * gameTime.Delta;
                Transform.Position += new Vector2(velocity.X, 0);
                AdjustWorldCollision();
                _previousPosition.X = Transform.Position.X;
                Transform.Position += new Vector2(0, velocity.Y);
                AdjustWorldCollision();
            }            
        }

        private void AdjustWorldCollision()
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

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "bullet")
            {
                GetComponent<AudioSource>().Play();
                collision.GameObject.Tag = null; // only I will be hit by this bullet
                Destroy();
            }
        }
    }
}
