using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TheCanisIncident.Stages
{
    class GeneratedLevelStage : GameplayStage
    {
        public GeneratedLevelStage()
            : base(GenerateMap())
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            var crosshair = CreateCrosshair(GetLayer("hud"));
            var player = AddPlayer(crosshair).SetPosition(PlayerStart);
            var camera = AddCamera(player).SetPosition(player.Transform.Position);
        }

        private static string GenerateMap()
        {
            var rand = new Random();

            // create rooms
            var rooms = new List<Rectangle>();
            for (var i = 0; i < 5; i++)
            {
                var w = rand.Next(6, 13);
                var h = rand.Next(6, 13);
                rooms.Add(new Rectangle(-w / 2, -h / 2, w, h));
            }

            // rotate directon and move until no overlap
            var x = 0f;
            var xdir = 1f;
            var y = -1f;
            var ydir = 1f;
            for (var i = 1; i < rooms.Count; i++)
            {
                var direction = new Vector2(x, y);
                x += xdir;
                if (x >= 1f || x <= -1f) xdir *= -1;
                y += ydir;
                if (y >= 1f || y <= -1f) ydir *= -1;
                direction *= 3;
                bool intersects = true;
                while (intersects)
                {
                    var newLoc = rooms[i].Location + direction.ToPoint();
                    rooms[i] = new Rectangle(newLoc.X, newLoc.Y, rooms[i].Width, rooms[i].Height);
                    for (var j = 0; j < rooms.Count; j++)
                    {
                        if (j == i)
                            continue;

                        intersects = false;
                        if (rooms[i].Intersects(rooms[j]))
                        {
                            intersects = true;
                            break;
                        }
                    }
                }
            }

            // randomize rooms for connectivity
            rooms = rooms.OrderBy(r => Guid.NewGuid()).ToList();

            // determine map size
            var tl = Vector2.Zero;
            var br = Vector2.Zero;
            foreach (var room in rooms)
            {
                if (room.X < tl.X) tl.X = room.X - 3;
                if (room.Y < tl.Y) tl.Y = room.Y - 3;
                if (room.X + room.Width > br.X) br.X = room.X + room.Width + 6;
                if (room.Y + room.Height > br.Y) br.Y = room.Y + room.Height + 6;
            }
            var width = (int)(br.X - tl.X);
            var height = (int)(br.Y - tl.Y);
            var map = new char[width, height];          
            for (var i = 0; i < rooms.Count; i++)
            {
                var room = rooms[i];
                for (var rx = 0; rx < room.Width; rx++)
                    for (var ry = 0; ry < room.Height; ry++)
                    {
                        var dx = room.X + rx - (int)tl.X;
                        var dy = room.Y + ry - (int)tl.Y;
                        map[dx, dy] = '.';
                    }

                var sp = new Vector2(room.X + (room.Width / 2) - (int)tl.X,
                    room.Y + (room.Height / 2) - (int)tl.Y).ToPoint();

                var troom = rooms[(i + 1) % rooms.Count];
                var ep = new Vector2(troom.X + (troom.Width / 2) - (int)tl.X,
                    troom.Y + (troom.Height / 2) - (int)tl.Y).ToPoint();

                Connect(map, sp, ep);
            }

            var door = new Point(width - 3, rand.Next(3, height - 3));
            var aroom = rooms.First();
            var end = new Vector2(aroom.X + (aroom.Width / 2) - (int)tl.X,
                aroom.Y + (aroom.Height / 2) - (int)tl.Y).ToPoint();
            Connect(map, door, end, false);
            map[door.X, door.Y] = '0';

            StringBuilder sb = new StringBuilder();
            for (var my = 0; my < height; my++)
            {
                for (var mx = 0; mx < width; mx++)
                {
                    if (my < height - 1)
                        if (map[mx, my] == '\0' && (map[mx, my + 1] == '.' || map[mx, my + 1] == '0'))
                            map[mx, my] = 'W';
                    
                    sb.Append(map[mx, my] == '\0' ? '#' : map[mx, my]);
                }
                sb.AppendLine();
            }

            sb.AppendLine(";");

            return sb.ToString();
        }

        private static void Connect(char[,] map, Point sp, Point ep, bool wide = true)
        {
            while (sp.X > ep.X)
            {
                map[sp.X, sp.Y] = '.';
                if (wide)
                {
                    map[sp.X, sp.Y + 1] = '.';
                    map[sp.X, sp.Y - 1] = '.';
                }
                sp.X--;
            }

            while (sp.X < ep.X)
            {
                map[sp.X, sp.Y] = '.';
                if (wide)
                {
                    map[sp.X, sp.Y + 1] = '.';
                    map[sp.X, sp.Y - 1] = '.';
                }
                sp.X++;
            }

            while (sp.Y > ep.Y)
            {
                map[sp.X, sp.Y] = '.';
                if (wide)
                {
                    map[sp.X + 1, sp.Y] = '.';
                    map[sp.X - 1, sp.Y] = '.';
                }
                sp.Y--;
            }

            while (sp.Y < ep.Y)
            {
                map[sp.X, sp.Y] = '.';
                if (wide)
                {
                    map[sp.X + 1, sp.Y] = '.';
                    map[sp.X - 1, sp.Y] = '.';
                }
                sp.Y++;
            }
        }
    }
}
