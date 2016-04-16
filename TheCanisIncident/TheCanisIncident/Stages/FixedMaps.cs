using System;
using System.Collections.Generic;
using System.Text;

namespace TheCanisIncident.Stages
{
    public static class FixedMaps
    {
        public static string Lab1
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
                layout.AppendLine("#..............#");
                layout.AppendLine("#..............W");
                layout.AppendLine("#..............D");
                layout.AppendLine("################");
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
    }
}
