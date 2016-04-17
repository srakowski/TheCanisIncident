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
        private static Random _random = new Random();

        private Vector2 _velocity;

        private bool[,] _layout;

        private bool _dead = false;

        private float _rotation;

        private bool _first = true;

        public Bullet(Vector2 direction, bool[,] layout, int ttl, int spread, float speed)
        {
            direction.Normalize();
            direction = Vector2.Transform(direction, Matrix.CreateRotationZ(MathHelper.ToRadians(_random.Next(-spread, spread + 1))));
            _rotation = (float)Math.Atan2(direction.X, -direction.Y);
            _velocity = direction * speed;
            StartCoroutine(TimeToLive(ttl));
            _layout = layout;
        }
        
        public override void Update(IGameTime gameTime)
        {
            if (_first)
            {
                Transform.Rotation = _rotation;
                _first = false;
            }

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
            {
                Destroy();
            }        
        }

        private IEnumerator DestroyNext()
        {
            yield return null;
            yield return null;
            Destroy();
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "enemy" && this.GameObject.Tag != "rainbow")
                Destroy();
            if (collision.GameObject.Tag == "player" && this.GameObject.Tag == "rainbow")
                Destroy();
        }

        private IEnumerator TimeToLive(int ttl)
        {
            yield return WaitMSecs(ttl);
            Destroy();
        }
    }
}
