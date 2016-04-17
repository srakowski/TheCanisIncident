using System;
using System.Collections.Generic;
using System.Text;
using Coldsteel;

namespace TheCanisIncident.Models
{
    public abstract class EnemySpawn
    {
        public abstract string Texture { get; }
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract int MaxSpawnRate { get; }
        public abstract int MinSpawnRate { get; }

        public abstract Behavior GetBehavior(GameObject player, bool[,] layout);
    }
}
