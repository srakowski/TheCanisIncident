using Coldsteel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Behaviors
{
    class Exit : Behavior
    {
        private Action _onTrigger;
                
        public Exit(Action onTrigger)
        {
            _onTrigger = onTrigger;
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "player")
                _onTrigger.Invoke();
        }
    }
}
