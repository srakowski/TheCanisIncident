using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Models
{
    abstract class Gun
    {
        public abstract int RateOfFire { get; }
        public abstract string Texture { get; }
        public abstract int TTL { get; }
        public abstract int BulletsToSpawn { get; }
        public abstract int Spread { get; }
        public abstract float Speed { get; }
        public abstract string GunTexture { get; }
    }
}
