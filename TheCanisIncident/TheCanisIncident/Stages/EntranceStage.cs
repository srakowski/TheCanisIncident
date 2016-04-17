using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Stages
{
    class EntranceStage : GameplayStage
    {
        private static string Entrance
        {
            get
            {
                var layout = new StringBuilder();
                layout.AppendLine("   ############");
                layout.AppendLine("   #WWWWWWWWWW#");
                layout.AppendLine("   #..........#");
                layout.AppendLine("####..........#");
                layout.AppendLine("#WWW..........#");
                layout.AppendLine("#X---.........#");
                layout.AppendLine("####..........#");
                layout.AppendLine("   ############");
                layout.AppendLine(";");
                return layout.ToString().Replace("\r", "");
            }
        }

        public EntranceStage()
            : base(Entrance)
        {
            NextStage = "GeneratedLevelStage";
        }

        protected override void Initialize()
        {
            base.Initialize();
            var crosshair = CreateCrosshair();            
            var player = AddPlayer(crosshair).SetPosition(800, 400);
            AddHud();
            var camera = AddCamera(player).SetPosition(player.Transform.Position);
        }
    }
}
