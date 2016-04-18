using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Coldsteel;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework;
using Coldsteel.Colliders;
using Microsoft.Xna.Framework.Graphics;

namespace TheCanisIncident.Behaviors.Enemies
{
    class BigFrigginKitty : Enemy
    {
        public BigFrigginKitty(GameObject player, bool[,] layout) : base(player, layout, 60)
        {
            MovementSpeed = 0f;
            StartCoroutine(EnableHit());
            this.IsBoss = true;
        }

        private IEnumerator EnableHit()
        {
            yield return WaitMSecs(500);
            this.GetComponent<Collider>().Enabled = true;
            StartCoroutine(FireEyes());
        }

        public override void WhenKilled()
        {
        }

        private IEnumerator FireEyes()
        {
            while (true)
            {
                var bullet = new GameObject("eyebullet")
                    .SetPosition(Transform.Position + new Vector2(-105, -80))
                    .AddComponent(new Bullet(Player.Transform.Position + new Vector2(20, 20) - this.Transform.Position, _layout, 2000, 10, 2f))
                    .AddComponent(new BoxCollider(30, 30).SetIsDynamic(true))
                    .AddComponent(new SpriteRenderer(GetLayer("player"), GetContent<Texture2D>("sprites/eyebullet")));
                AddGameObject(bullet);

                var bullet2 = new GameObject("eyebullet")
                    .SetPosition(Transform.Position + new Vector2(-55, -80))
                    .AddComponent(new Bullet(Player.Transform.Position + new Vector2(20, 20) - this.Transform.Position, _layout, 2000, 10, 2f))
                    .AddComponent(new BoxCollider(30, 30).SetIsDynamic(true))
                    .AddComponent(new SpriteRenderer(GetLayer("player"), GetContent<Texture2D>("sprites/eyebullet")));
                AddGameObject(bullet2);

                yield return WaitMSecs(300);
            }
        }
    }
}
