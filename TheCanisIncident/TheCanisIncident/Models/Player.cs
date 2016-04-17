using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Models
{
    class Player
    {
        public int HP { get; set; } = 100;

        public int MaxHP { get; set; } = 100;

        public int PistolBullets { get; set; } = 17;

        public int ShotgunShells { get; set; } = 0;

        public int RifleBullets { get; set; } = 0;

        public int Rockets { get; set; } = 0;

        public Gun Gun { get; set; } = new Rifle();

        internal void ChangeGuns()
        {
            if (this.Gun is Shotgun)
                this.Gun = new Rifle();
            else if (this.Gun is Rifle)
                this.Gun = new Shotgun();
        }
    }
}
