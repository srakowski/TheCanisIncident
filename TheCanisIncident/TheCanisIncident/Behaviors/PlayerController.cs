using Coldsteel;
using Coldsteel.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Behaviors
{
    class PlayerController : Behavior
    {
        private float _speed = 0.2f;

        private Vector2 _previousPosition;

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
        }
    }
}
