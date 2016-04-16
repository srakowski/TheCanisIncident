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

        private float _movementSpeed = 0.03f;

        public Enemy(GameObject player)
        {
            this.Player = player;
        }

        public override void Update(IGameTime gameTime)
        {
            _previousPosition = Transform.Position;

            var velocity = Player.Transform.Position - Transform.Position;
            velocity.Normalize();
            velocity *= _movementSpeed;
            Transform.Position += velocity * gameTime.Delta;
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "ceiling" || collision.GameObject.Tag == "wall")
                this.Transform.Position = _previousPosition;

            if (collision.GameObject.Tag == "bullet")
            {
                GetComponent<AudioSource>().Play();
                Destroy();
            }
        }
    }
}
