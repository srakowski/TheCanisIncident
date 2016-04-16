using Coldsteel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Behaviors
{
    class PickupItem : Behavior
    {
        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "player")
            {
                GetComponent<AudioSource>().Play();
                Destroy();
            }
        }
    }
}
