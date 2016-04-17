using System;
using System.Collections.Generic;
using System.Text;
using TheCanisIncident.Behaviors.Weapons;

namespace TheCanisIncident.Models
{
    class Player
    {
        public int HP { get; set; } = 15;

        public int PistolBullets { get; set; } = 17;

        public int ShotgunShells { get; set; } = 0;

        public int RifleBullets { get; set; } = 0;

        public int Rockets { get; set; } = 0;

        public Weapon EquippedWeapon { get; set; }

        public List<Weapon> AvailableWeapons { get; set; }
    }
}
