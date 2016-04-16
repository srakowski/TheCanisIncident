using Coldsteel;
using Coldsteel.Colliders;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TheCanisIncident.Behaviors;

namespace TheCanisIncident.Stages
{
    class GameplayStage : GameStage
    {
        private GameObject _player;

        protected override void LoadContent()
        {
            LoadContent<Texture2D>("sprites/wall", "sprites/ceiling", "sprites/floor", 
                "sprites/adi", "sprites/crosshair", "sprites/bullet", "sprites/enemy",
                "sprites/item", "sprites/kitty", "sprites/mushcontainer");

            LoadContent<SoundEffect>("audio/pickup", "audio/fire", "audio/hit");
        }

        protected override void Initialize()
        {
            BackgroundColor = new Color(12, 12, 12);
            DefaultLayer.SpriteSortMode = SpriteSortMode.FrontToBack;

            var floorLayer = AddLayer("floor", -2);
            var itemsLayer = AddLayer("items", -1);
            var ceilingLayer = AddLayer("ceiling", 1);
            var hudLayer = AddLayer("hud", 2);

            _player = AddPlayer(hudLayer);
                        
            Load(FixedMaps.Lab1);
        }

        protected void Load(string mapdesc)
        {
            mapdesc = LoadBuildingLayout(mapdesc);
            mapdesc = LoadConfigurations(mapdesc);
            mapdesc = LoadObjects(mapdesc);
        }

        private string LoadBuildingLayout(string mapdesc)
        {
            var x = 0;
            var y = 0;
            int i = 0;
            for (i = 0; i < mapdesc.Length; i++)
            {
                char c = mapdesc[i];
                if (c == ';')
                    break;            
                
                switch (c)
                {
                    case '.':
                    case 'D':
                        AddFloor(x, y, GetLayer("floor"));
                        if (c == 'D')
                            _player.SetPosition(x * 96, y * 96);
                        break;

                    case '#':
                        AddCeiling(x, y, GetLayer("ceiling"));
                        break;

                    case 'W':
                        AddWall(x, y, GetLayer("floor"));
                        break;

                    case '\n':
                        x = -1;
                        y++;
                        break;
                }
                x++;
            }
            return mapdesc.Substring(i + 1);
        }

        private string LoadConfigurations(string mapdesc)
        {
            var kv = new StringBuilder();
            int i = 0;
            for (i = 0; i < mapdesc.Length; i++)
            {
                char c = mapdesc[i];
                if (c == ';')
                {
                    if (kv.Length > 0)
                    {
                        var config = kv.ToString().Split('=');
                        ProcessConfig(config[0], config[1]);
                        kv.Clear();
                    }
                    break;
                }

                if (c == '\n' && kv.Length > 0)
                {
                    var config = kv.ToString().Split('=');
                    ProcessConfig(config[0], config[1]);
                    kv.Clear();
                }
                else if (c != '\n')
                {
                    kv.Append(c);
                }
            }
            return mapdesc.Substring(i + 1);
        }

        private string LoadObjects(string mapdesc)
        {
            var kv = new StringBuilder();
            int i = 0;
            for (i = 0; i < mapdesc.Length; i++)
            {
                char c = mapdesc[i];
                if (c == ';')
                {
                    if (kv.Length > 0)
                    {
                        var config = kv.ToString().Split(',');
                        ProcessObject(config[0], config[1], config[2]);
                        kv.Clear();
                    }
                    break;
                }

                if (c == '\n' && kv.Length > 0)
                {
                    var config = kv.ToString().Split(',');
                    ProcessObject(config[0], config[1], config[2]);
                    kv.Clear();
                }
                else if (c != '\n')
                {
                    kv.Append(c);
                }
            }
            return mapdesc.Substring(i + 1);
        }

        private void ProcessConfig(string key, string value)
        {
            if (key == "CanFire")
                _player.GetComponent<PlayerController>().CanFire = bool.Parse(value);
            if (key == "Position")
            {
                var coords = value.Split(',');
                _player.SetPosition(float.Parse(coords[0]), float.Parse(coords[1]));
            }
        }

        private void ProcessObject(string key, string x, string y)
        {
            Vector2 pos = new Vector2(float.Parse(x), float.Parse(y));

            if (key == "Kitty")
                AddGameObject()
                    .SetPosition(pos)
                    .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/kitty")));
            if (key == "MushContainer")
                AddGameObject()
                    .SetPosition(pos)
                    .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/mushcontainer")));
        }

        private GameObject AddPlayer(Layer hudLayer)
        {
            var crosshair = new GameObject("crosshair")
                .AddComponent(new SpriteRenderer(hudLayer, GetContent<Texture2D>("sprites/crosshair")));

            var obj = new GameObject("_player")
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/adi")))
                .AddComponent(new PlayerController(crosshair))
                .AddComponent(new BoxCollider(30, 72))
                .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/fire")))
                .AddChild(crosshair);

            AddGameObject(obj);

            var camera = new GameObject("camera")
                .AddComponent(new Camera())
                .AddComponent(new CameraFollow(obj));

            AddGameObject(camera);

            return obj;
        }

        private void AddEnemy(int x, int y, Layer defaultLayer)
        {
            var obj = new GameObject("enemy")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/enemy")))
                .AddComponent(new BoxCollider(80, 50))
                .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/hit")))
                .AddComponent(new Enemy(_player));

            AddGameObject(obj);
        }

        private void AddCeiling(int x, int y, Layer mapLayer)
        {
            var obj = new GameObject("ceiling")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new SpriteRenderer(mapLayer, GetContent<Texture2D>("sprites/ceiling")))
                .AddComponent(new BoxCollider(96, 48, new Vector2(0, 24)));

            obj.AddChild(new GameObject("ceiling-collider")
                .AddComponent(new BoxCollider(96, 48, new Vector2(0, -24))));

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

        private void AddPickupItem(int x, int y, Layer itemsLayer)
        {
            var obj = new GameObject("item")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new PickupItem())
                .AddComponent(new SpriteRenderer(itemsLayer, GetContent<Texture2D>("sprites/item")))
                .AddComponent(new BoxCollider(24, 12, new Vector2(0, 6)))
                .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/pickup")));

            AddGameObject(obj);
        }
    }
}
