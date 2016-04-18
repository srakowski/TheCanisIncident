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
using Microsoft.Xna.Framework.Audio;
using TheCanisIncident.Behaviors.Enemies;

namespace TheCanisIncident.Behaviors
{
    class PlayerController : Behavior
    {
        private static Random _rand = new Random();

        private Player _player;

        private float _speed = 0.5f;

        private Vector2 _previousPosition;

        private GameObject _crosshair;

        public bool CanFire { get; set; } = true;

        private bool[,] _layout;

        private Vector2 _offset = new Vector2(48, 48);

        public Gun Gun
        {
            get { return _player.Gun; }
        }

        bool _moving = false;

        private GameObject gun;

        IGameTime _gameTime;

        public PlayerController(GameObject crosshair, GameObject gun, Player player, bool[,] layout)
        {
            this.gun = gun;
            _crosshair = crosshair;
            _player = player;
            _layout = layout;
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "rainbow" || collision.GameObject.Tag == "enemy")
            {
                this._player.HP -= 6;
                if (this._player.HP < 0)
                {
                    GameStageManager.LoadStage("FailStage");
                }
            }
            if (collision.GameObject.Tag == "pickup")
            {
                this._player.HP++;
                if (this._player.HP > this._player.MaxHP)
                    this._player.HP = this._player.MaxHP;
            }
            if (collision.GameObject.Tag == "guts")
            {
                _player.HasGuts = true;
                collision.GameObject.Destroy();
            }
            if (collision.GameObject.Tag == "hatch" && _player.HasGuts)
            {
                _player.HasGuts = false;
                CanFire = true;
                StartCoroutine(MoveTo(new Vector2(300, 700)));
                var container = collision.GameObject.Parent;
                container.GetComponent<SpriteRenderer>().Texture = GetContent<Texture2D>("sprites/mush");
                collision.GameObject.Destroy();
                AddGameObject("enemy")
                    .SetPosition(container.Transform.Position + new Vector2(-96, 96))
                    .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/bk")))
                    .AddComponent(new BoxCollider(200, 200) { IsDynamic = false, Enabled = false })
                    .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/hit")))
                    .AddComponent(new BigFrigginKitty(this.GameObject, _layout));
            }
        }

        private IEnumerator MoveTo(Vector2 pos)
        {
            _moving = true;
            float time = 0;
            while (time < 500f)
            {
                time += _gameTime?.Delta ?? 0;
                var t = time / 500f;
                this.Transform.Position = Vector2.SmoothStep(this.Transform.Position, pos, t);
                yield return null;
            }
            this.Transform.Position = pos;
            yield return WaitMSecs(100);
            _moving = false;
        }

        public override void Update(IGameTime gameTime)
        {
            _gameTime = gameTime;

            if (_moving)
                return;

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
            this.GetComponent<SpriteRenderer>().SpriteEffects = _crosshair.Transform.LocalPosition.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            this.gun.SetRotation((float)Math.Atan2(aimDirection.Y, aimDirection.X));            
            if (Input.GetControl<ButtonControl>("ChangeWeapons").WasPressed())
            {
                _player.ChangeGuns();
                var rend = this.gun.GetComponent<SpriteRenderer>();
                if (rend != null)
                    rend.Texture = GetContent<Texture2D>(_player.Gun.GunTexture);
            }
            else if (Input.GetControl<ButtonControl>("Fire").IsDown())
            {
                StartCoroutine(FireGun(Gun));
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

        private IEnumerator FireGun(Gun gun)
        {
            if (!CanFire)
                yield break;

            CanFire = false;
            GetComponent<AudioSource>().Play();
            for (var i = 0; i < gun.BulletsToSpawn; i++)
            {
                var bullet = new GameObject("bullet")
                    .SetPosition(Transform.Position)
                    .AddComponent(new Bullet(_crosshair.Transform.LocalPosition, _layout, gun.TTL, gun.Spread, gun.Speed))
                    .AddComponent(new BoxCollider(20, 20).SetIsDynamic(true))
                    .AddComponent(new SpriteRenderer(GetLayer("items"), GetContent<Texture2D>(gun.Texture)));
                AddGameObject(bullet);
            }            
            yield return WaitMSecs(gun.RateOfFire);
            CanFire = true;
        }
    }
}
