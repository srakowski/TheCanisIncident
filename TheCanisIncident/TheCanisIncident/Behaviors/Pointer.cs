using Coldsteel;
using Coldsteel.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Behaviors
{
    class Pointer : Behavior
    {
        private Action<Vector2> _onClick;

        public Pointer(Action<Vector2> onClick)
        {
            _onClick = onClick;
        }

        public override void Update(IGameTime gameTime)
        {
            this.Transform.Position = Input.GetControl<PositionalControl>("Pointer").GetPosition();
            if (Input.GetControl<ButtonControl>("PointerClick").WasPressed())
                _onClick.Invoke(this.Transform.Position);
        }
    }
}
