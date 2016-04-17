using System;
using System.Collections.Generic;
using System.Text;
using TheCanisIncident.Behaviors;
using TheCanisIncident.Models;

namespace TheCanisIncident.Stages
{
    class GameData
    {
        public bool Endless { get; set; } = false;
        public int Level { get; set; } = 0;
        public Player Player { get; internal set; }
        public readonly int MaxLevels = 8;
        public GameData()
        {
            this.Player = new Player();
        }
    }
}
