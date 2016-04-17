using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Models
{
    class Pistol : Gun
    {
        public override int BulletsToSpawn
        {
            get
            {
                return 1;
            }
        }

        public override int RateOfFire
        {
            get
            {
                return 160;
            }
        }

        public override float Speed
        {
            get
            {
                return 3f;
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
