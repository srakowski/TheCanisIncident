using System;
using System.Collections.Generic;
using System.Text;
using Coldsteel;
using TheCanisIncident.Behaviors.Enemies;

namespace TheCanisIncident.Models
{
    class BatKoalaSpawn : EnemySpawn
    {
        public override int Height
        {
            get
            {
                return 48;
            }
        }

        public override int MaxSpawnRate
        {
            get
            {
                return 2000;
            }
        }

        public override int MinSpawnRate
        {
            get
            {
                return 800;
            }
        }

        public override string Texture
        {
            get
            {
                return "sprites/kb";
            }
        }

        public override int Width
        {
            get
            {
                return 48;
            }
        }

        public override Behavior GetBehavior(GameObject player, bool[,] layout)
        {
            return new BatKoala(player, layout);
        }
    }
}
