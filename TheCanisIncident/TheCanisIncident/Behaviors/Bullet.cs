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

        private bool[,] _layout;

        private bool _dead = false;

        public Bullet(Vector2 direction, bool[,] layout)
        {
            direction.Normalize();
            _velocity = direction * _speed;
            StartCoroutine(TimeToLive());
            _layout = layout;
        }
        
        public override void Update(IGameTime gameTime)
        {
            Transform.Position += _velocity * gameTime.Delta;
            try
            {
                if (!_dead)
                {
                    var pos = this.Transform.Position;
                    var px = (int)pos.X / 96;
                    var py = (int)pos.Y / 96;
                    if (!_layout[px, py])
                    {
                        _dead = true;
                        this.GetComponent<Collider>().Enabled = false;
                        StartCoroutine(DestroyNext());
                    }
                }
            }
            catch
            { }        
        }

        private IEnumerator DestroyNext()
        {
            yield return null;
            yield return null;
            Destroy();
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "enemy")
                Destroy();
        }

        private IEnumerator TimeToLive()
        {
            yield return WaitMSecs(2000);
            Destroy();
        }
    }
}
