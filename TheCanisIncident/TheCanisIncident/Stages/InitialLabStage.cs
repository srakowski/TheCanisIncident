using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Stages
{
    class InitialLabStage : GameplayStage
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
                layout.AppendLine("#.............--X#");
                layout.AppendLine("##################");
                return layout.ToString().Replace("\r", "");
            }
        }

        public InitialLabStage()
            : base (Lab)
        {
            NextStage = "Paper2Stage";
        }

        protected override void Initialize()
        {
            base.Initialize();
            var crosshair = CreateCrosshair();
            var player = AddPlayer(crosshair, false, false).SetPosition(800, 400);
            var camera = AddCamera(player).SetPosition(player.Transform.Position);
            AddInstruct();
            AddKitty(600, 400);
            AddProp(1280, 200, "sprites/mushcontainer");
        }
    }
}
