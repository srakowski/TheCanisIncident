using Coldsteel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TheCanisIncident.Behaviors
{
    class Bullet : Behavior
    {
        private float _speed = 2f;

        private Vector2 _velocity;

        public Bullet(Vector2 direction)
        {
            direction.Normalize();
            _velocity = direction * _speed;
            StartCoroutine(TimeToLive());
        }
        
        public override void Update(IGameTime gameTime)
        {
            Transform.Position += _velocity * gameTime.Delta;
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag != "player")
                Destroy();
        }

        private IEnumerator TimeToLive()
        {
            yield return WaitMSecs(2000);
            Destroy();
        }
    }
}
