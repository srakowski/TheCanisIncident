using Coldsteel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Behaviors
{
    class CameraFollow : Behavior
    {
        public GameObject Subject { get; set; }

        public CameraFollow(GameObject subject)
        {
            this.Subject = subject;
        }

        public override void Update(IGameTime gameTime)
        {
            this.Transform.Position = Subject.Transform.Position;
        }
    }
}
