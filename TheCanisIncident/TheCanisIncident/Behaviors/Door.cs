using Coldsteel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Behaviors
{
    class Door : Behavior
    {
        private int _index;
        private string[] _doorRegistry;
        public Door(string[] doorRegistry, int index)
        {
            this._doorRegistry = doorRegistry;
            this._index = index;
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.GameObject.Tag == "player")
            {
                GameStageManager.LoadStage(_doorRegistry[_index]);
            }
        }
    }
}
