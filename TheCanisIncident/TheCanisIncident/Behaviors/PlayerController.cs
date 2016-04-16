using Coldsteel;
using Coldsteel.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework.Graphics;
using Coldsteel.Colliders;

namespace TheCanisIncident.Behaviors
{
    class PlayerController : Behavior
    {
        private float _speed = 0.2f;

        private Vector2 _previousPosition;

        private GameObject _crosshair;

        private bool _canFire = true;

        private int _rateOfFire = 100;

        public PlayerController(GameObject crosshair)
        {
            _crosshair = crosshair;
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "ceiling" || collision.GameObject.Tag == "wall")
                this.Transform.Position = _previousPosition;
        }

        public override void Update(IGameTime gameTime)
        {
            _previousPosition = this.Transform.Position;

            if (Input.GetControl<ButtonControl>("MoveUp").IsDown())
                this.Transform.Position += new Vector2(0, -1) * _speed * gameTime.Delta;

            if (Input.GetControl<ButtonControl>("MoveDown").IsDown())
                this.Transform.Position += new Vector2(0, 1) * _speed * gameTime.Delta;

            if (Input.GetControl<ButtonControl>("MoveLeft").IsDown())
                this.Transform.Position += new Vector2(-1, 0) * _speed * gameTime.Delta;

            if (Input.GetControl<ButtonControl>("MoveRight").IsDown())
                this.Transform.Position += new Vector2(1, 0) * _speed * gameTime.Delta;

            var aimDirection = Input.GetControl<DirectionalControl>("Aim").Direction();
            _crosshair.Transform.LocalPosition = aimDirection;
            if (Vector2.Distance(_crosshair.Transform.Position, Transform.Position) > 200f)
            {
                aimDirection.Normalize();
                aimDirection *= 200f;
                _crosshair.Transform.LocalPosition = aimDirection;
            }

            if (Input.GetControl<ButtonControl>("Fire").IsDown())
                StartCoroutine(SpawnBullet());
        }

        private IEnumerator SpawnBullet()
        {
            if (!_canFire)
                yield break;

            _canFire = false;
            GetComponent<AudioSource>().Play();
            var bullet = new GameObject("bullet")
                .SetPosition(Transform.Position)
                .AddComponent(new Bullet(_crosshair.Transform.LocalPosition))
                .AddComponent(new BoxCollider(20, 20))
                .AddComponent(new SpriteRenderer(GetLayer("items"), GetContent<Texture2D>("sprites/bullet")));
            AddGameObject(bullet);
            yield return WaitMSecs(_rateOfFire);
            _canFire = true;
        }
    }
}
