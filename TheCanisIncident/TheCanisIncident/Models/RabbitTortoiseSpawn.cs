using Coldsteel;
using System;
using System.Collections.Generic;
using System.Text;
using TheCanisIncident.Behaviors.Enemies;

namespace TheCanisIncident.Models
{
    class RabbitTortoiseSpawn : EnemySpawn
    {
        public override int Height
        {
            get
            {
                return 64;
            }
        }

        public override int MaxSpawnRate
        {
            get
            {
                return 9000;
            }
        }

        public override int MinSpawnRate
        {
            get
            {
                return 1000;
            }
        }

        public override string Texture
        {
            get
            {
                return "sprites/bigtr";
            }
        }

        public override int Width
        {
            get
            {
                return 96;
            }
        }

        public override Behavior GetBehavior(GameObject player, bool[,] layout)
        {
            return new RabbitTortoise(player, layout);
        }
    }
}

