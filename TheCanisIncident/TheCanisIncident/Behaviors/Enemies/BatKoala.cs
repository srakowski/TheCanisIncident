using System;
using System.Collections.Generic;
using System.Text;
using Coldsteel;

namespace TheCanisIncident.Behaviors.Enemies
{
    class BatKoala : Enemy
    {
        public BatKoala(GameObject player, bool[,] layout) : base(player, layout, 1)
        {
            this.MovementSpeed = ((float)_rand.NextDouble() + 0.1f) * 0.5f;
        }      
    }
}
