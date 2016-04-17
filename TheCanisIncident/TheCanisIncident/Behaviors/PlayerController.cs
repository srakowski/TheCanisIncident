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
using TheCanisIncident.Models;

namespace TheCanisIncident.Behaviors
{
    class PlayerController : Behavior
    {
        private Player _player;

        private float _speed = 0.4f;

        private Vector2 _previousPosition;

        private GameObject _crosshair;

        public bool CanFire { get; set; } = true;

        private int _rateOfFire = 100;

        private bool[,] _layout;

        private Vector2 _offset = new Vector2(48, 48);

        public PlayerController(GameObject crosshair, Player player, bool[,] layout)
        {
            _crosshair = crosshair;
            _player = player;
            _layout = layout;
        }

        public override void OnCollision(Collision collision)
        {
        }

        public override void Update(IGameTime gameTime)
        {
            _previousPosition = this.Transform.Position;

            if (Input.GetControl<ButtonControl>("MoveUp").IsDown())
                this.Transform.Position += new Vector2(0, -1) * _speed * gameTime.Delta;

            if (Input.GetControl<ButtonControl>("MoveDown").IsDown())
                this.Transform.Position += new Vector2(0, 1) * _speed * gameTime.Delta;

            AdjustWorldCollision();

            if (Input.GetControl<ButtonControl>("MoveLeft").IsDown())
                this.Transform.Position += new Vector2(-1, 0) * _speed * gameTime.Delta;

            if (Input.GetControl<ButtonControl>("MoveRight").IsDown())
                this.Transform.Position += new Vector2(1, 0) * _speed * gameTime.Delta;

            _previousPosition.Y = this.Transform.Position.Y;
            AdjustWorldCollision();

            var aimDirection = Input.GetControl<DirectionalControl>("Aim").Direction();
            _crosshair.Transform.LocalPosition = aimDirection;

            if (Input.GetControl<ButtonControl>("Fire").IsDown())
                StartCoroutine(SpawnBullet());
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

        private IEnumerator SpawnBullet()
        {
            if (!CanFire)
                yield break;

            CanFire = false;
            GetComponent<AudioSource>().Play();
            var bullet = new GameObject("bullet")
                .SetPosition(Transform.Position)
                .AddComponent(new Bullet(_crosshair.Transform.LocalPosition, _layout))
                .AddComponent(new BoxCollider(20, 20).SetIsDynamic(true))
                .AddComponent(new SpriteRenderer(GetLayer("items"), GetContent<Texture2D>("sprites/bullet")));
            AddGameObject(bullet);
            yield return WaitMSecs(_rateOfFire);
            CanFire = true;
        }
    }
}
