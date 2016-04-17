using System;
using System.Collections.Generic;
using System.Text;
using Coldsteel;
using TheCanisIncident.Behaviors.Enemies;

namespace TheCanisIncident.Models
{
    class NarwhalHorseSpawn : EnemySpawn
    {

        public override int Height
        {
            get
            {
                return 142;
            }
        }

        public override int MaxSpawnRate
        {
            get
            {
                return 3000;
            }
        }

        public override int MinSpawnRate
        {
            get
            {
                return 300;
            }
        }

        public override string Texture
        {
            get
            {
                return "sprites/nh";
            }
        }

        public override int Width
        {
            get
            {
                return 192;
            }
        }

        public override Behavior GetBehavior(GameObject player, bool[,] layout)
        {
            return new NarwhalHorse(player, layout);
        }
    }
}
