using Coldsteel;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheCanisIncident.Models;

namespace TheCanisIncident.Behaviors
{
    class HPBar : Behavior
    {
        private Player player;

        public HPBar(Player player)
        {
            this.player = player;
        }

        public override void Update(IGameTime gameTime)
        {
            var pec = (float)player.HP / (float)player.MaxHP;
            this.GetComponent<SpriteRenderer>().DestinationRectangle =
                new Rectangle(0, 0, (int)(300 * pec), 54);
        }
    }
}
