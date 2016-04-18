using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Stages
{
    class FinalLabStage : GameplayStage
    {
        private static string Lab
        {
            get
            {
                var layout = new StringBuilder();
                layout.AppendLine("################");
                layout.AppendLine("#WWWWWWWWWWWWWW#");
                layout.AppendLine("#..............#");
                layout.AppendLine("#..............#");
                layout.AppendLine("#####..........#");
                layout.AppendLine("#WWWW..........#");
                layout.AppendLine("#..............###");
                layout.AppendLine("#..............WW#");
                layout.AppendLine("#.............--E#");
                layout.AppendLine("##################");
                return layout.ToString().Replace("\r", "");
            }
        }

        public FinalLabStage()
            : base(Lab)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            var crosshair = CreateCrosshair();
            var player = AddPlayer(crosshair, false).SetPosition(PlayerStart);
            AddHud();
            var camera = AddCamera(player).SetPosition(player.Transform.Position);
            AddKitty(600, 400, true);
            AddMushContainer(1280, 200);
        }
    }
}
