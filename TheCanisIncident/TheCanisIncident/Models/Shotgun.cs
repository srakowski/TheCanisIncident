using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Models
{
    class Shotgun : Gun
    {
        static Random _rand = new Random();

        public override int BulletsToSpawn
        {
            get
            {
                return 8;
            }
        }

        public override string GunTexture
        {
            get
            {
                return "sprites/shotgun";
            }
        }

        public override int RateOfFire
        {
            get
            {
                return 320;
            }
        }

        public override float Speed
        {
            get
            {
                return ((float)_rand.NextDouble() * 2f) + 3;
            }
        }

        public override int Spread
        {
            get
            {
                return 7;
            }
        }

        public override string Texture
        {
            get
            {
                return "sprites/fragbullet";
            }
        }

        public override int TTL
        {
            get
            {
                return 180;
            }
        }

       
    }
}
