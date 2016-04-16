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
                layout.AppendLine("#..............##");
                layout.AppendLine("#..............WW");
                layout.AppendLine("#...............1");
                layout.AppendLine("#################");
                layout.AppendLine(";");
                layout.AppendLine("CanFire=false");
                layout.AppendLine("Position=900,384");
                layout.AppendLine(";");
                layout.AppendLine("Kitty,768,384");
                layout.AppendLine("MushContainer,1280,200");
                layout.AppendLine(";");
                return layout.ToString().Replace("\r", "");
            }
        }

        public InitialLabStage()
            : base (Lab)
        {
            _doorRegistry[1] = "Paper2Stage";
        }

        protected override void Initialize()
        {
            base.Initialize();
            var crosshair = CreateCrosshair(GetLayer("hud"));
            var player = AddPlayer(crosshair).SetPosition(800, 400);
            var camera = AddCamera(player).SetPosition(player.Transform.Position);
            AddKitty(600, 400);
            AddProp(1280, 200, "sprites/mushcontainer");
        }
    }
}
