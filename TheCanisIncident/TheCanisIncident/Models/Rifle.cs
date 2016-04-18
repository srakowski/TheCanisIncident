using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Models
{
    class Rifle : Gun
    {
        public override int BulletsToSpawn
        {
            get
            {
                return 1;
            }
        }

        public override string GunTexture
        {
            get
            {
                return "sprites/rifle";
            }
        }

        public override int RateOfFire
        {
            get
            {
                return 80;
            }
        }

        public override float Speed
        {
            get
            {
                return 4f;
            }
        }

        public override int Spread
        {
            get
            {
                return 2;
            }
        }

        public override string Texture
        {
            get
            {
                return "sprites/bullet";
            }
        }

        public override int TTL
        {
            get
            {
                return 800;
            }
        }
    }
}
