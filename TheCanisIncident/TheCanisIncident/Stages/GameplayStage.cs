using Coldsteel;
using Coldsteel.Colliders;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TheCanisIncident.Behaviors;

namespace TheCanisIncident.Stages
{
    class GameplayStage : GameStage
    {
        protected override void LoadContent()
        {
            LoadContent<Texture2D>("sprites/wall", "sprites/ceiling", "sprites/floor", "sprites/adi", "sprites/crosshair");
        }

        protected override void Initialize()
        {
            BackgroundColor = new Color(12, 12, 12);

            var map = "###########\n#WWW####WW#\n#...WWW#..#\n#......#..#\n##.#####.##\n#W.WWWWW.W#\n#...S.....#\n#.........#\n#.........#\n###########";
            LoadMap(map);


        }

        protected void LoadMap(string mapdesc)
        {
            var floorLayer = AddLayer("floor", -1);
            var ceilingLayer = AddLayer("ceiling", 1);
            var hudLayer = AddLayer("hud", 2);
            var x = 0;
            var y = 0;
            foreach (var c in mapdesc)
            {
                switch (c)
                {
                    case '.':
                    case 'S':
                        AddFloor(x, y, floorLayer);
                        if (c == 'S')
                            AddPlayer(x, y, hudLayer);
                        break;

                    case '#':
                        AddCeiling(x, y, ceilingLayer);
                        break;

                    case 'W':
                        AddWall(x, y, floorLayer);
                        break;

                    case '\n':
                        x = -1;
                        y++;
                        break;
                }

                x++;
            }
        }

        private void AddPlayer(int x, int y, Layer hudLayer)
        {
            var crosshair = new GameObject("crosshair")
                .AddComponent(new SpriteRenderer(hudLayer, GetContent<Texture2D>("sprites/crosshair")));

            var obj = new GameObject("player")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/adi")))
                .AddComponent(new PlayerController(crosshair))
                .AddComponent(new BoxCollider(30, 72))
                .AddChild(crosshair);

            AddGameObject(obj);

            var camera = new GameObject("camera")
                .AddComponent(new Camera())
                .AddComponent(new CameraFollow(obj));

            AddGameObject(camera);
        }

        private void AddCeiling(int x, int y, Layer mapLayer)
        {
            var obj = new GameObject("ceiling")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new SpriteRenderer(mapLayer, GetContent<Texture2D>("sprites/ceiling")))
                .AddComponent(new BoxCollider(96, 48, new Vector2(0, 24)));

            AddGameObject(obj);
        }

        private void AddWall(int x, int y, Layer mapLayer)
        {
            var obj = new GameObject("wall")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new SpriteRenderer(mapLayer, GetContent<Texture2D>("sprites/wall")))
                .AddComponent(new BoxCollider(96, 48, new Vector2(0, -24)));

            AddGameObject(obj);
        }

        private void AddFloor(int x, int y, Layer mapLayer)
        {
            var obj = new GameObject("floor")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new SpriteRenderer(mapLayer, GetContent<Texture2D>("sprites/floor")));

            AddGameObject(obj);
        }
    }
}
