using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Coldsteel;
using Microsoft.Xna.Framework;

namespace TheCanisIncident.Behaviors.Enemies
{
    class LittleRabitTortoise : Enemy
    {

        public LittleRabitTortoise(GameObject player, bool[,] layout) : base(player, layout, 1)
        {
            this.MovementSpeed = ((float)_rand.NextDouble() + 0.1f) * 0.6f;
        }


    }
}
