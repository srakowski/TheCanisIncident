using System;
using System.Collections.Generic;
using System.Text;
using Coldsteel;

namespace TheCanisIncident.Behaviors.Enemies
{
    class BigFrigginKitty : Enemy
    {
        public BigFrigginKitty(GameObject player, bool[,] layout) : base(player, layout, 30)
        {
        }

        public override void WhenKilled()
        {

        }
    }
}
