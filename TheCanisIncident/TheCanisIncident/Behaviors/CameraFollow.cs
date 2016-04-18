using Coldsteel;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Behaviors
{
    class CameraFollow : Behavior
    {
        public GameObject Subject { get; set; }

        public CameraFollow(GameObject subject)
        {
            this.Subject = subject;
        }

        public override void Update(IGameTime gameTime)
        {
            this.Transform.Position = Vector2.SmoothStep(this.Transform.Position, Subject.Transform.Position, 0.3f);
        }

        public void Shake()
        {
            if (_isShaking)
                return;

            _isShaking = true;
            StartCoroutine(DoShake());
        }

        private bool _isShaking = false;

        private Random _rand = new Random();

        private IEnumerator DoShake()
        {            
            for (int i = 60; i > 0; i -= 5)
            {
                Transform.Position += new Vector2((float)_rand.Next(-i, i) / 10f, (float)_rand.Next(-i, i) / 10f);
                yield return WaitMSecs(12);
            }            
            _isShaking = false;
        }
    }
}
