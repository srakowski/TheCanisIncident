using Coldsteel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Behaviors
{
    class PickupItem : Behavior
    {
        private static Random _random = new Random();

        private GameObject _player;

        private float _speed;

        public PickupItem(GameObject player)
        {
            _player = player;
            _speed = 0.12f;
        }

        public override void Update(IGameTime gameTime)
        {
            if (Vector2.Distance(this.Transform.Position, _player.Transform.Position) < 300)
                this.Transform.Position = Vector2.SmoothStep(this.Transform.Position, _player.Transform.Position, _speed);
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "player")
            {
                GetComponent<AudioSource>().Play();
                Destroy();
            }
        }
    }
}
