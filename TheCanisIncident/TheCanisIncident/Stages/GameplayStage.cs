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
    abstract class GameplayStage : GameStage
    {
        //private GameObject _player;

        protected string[] _doorRegistry = new string[10];

        private string _map;

        protected Vector2 PlayerStart { get; set; } = Vector2.Zero;

        public GameplayStage(string map)
        {
            _map = map;
            for (var i = 0; i < _doorRegistry.Length; i++)
                _doorRegistry[i] = "MainMenuStage";
        }

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

            //_player = AddPlayer(hudLayer);
                        
            Load(_map);
        }

        protected void Load(string mapdesc)
        {
            mapdesc = LoadMap(mapdesc);
            //mapdesc = LoadConfigurations(mapdesc);
            //mapdesc = LoadObjects(mapdesc);
        }

        private string LoadMap(string mapdesc)
        {
            int x = 0, y = 0, i = 0;
            for (i = 0; i < mapdesc.Length; i++)
            {
                char c = mapdesc[i];
                if (c == ';')
                    break;            
                               
                if (c == '.') AddFloor(x, y, GetLayer("floor"));
                if (c - '0' >= 0 && c - '0' <= 9) AddDoor(x, y, GetLayer("floor"), c);
                if (c == '#') AddCeiling(x, y, GetLayer("ceiling"));
                if (c == 'W') AddWall(x, y, GetLayer("floor"));
                if (c == '\n')
                {
                    x = -1;
                    y++;
                }
                                
                x++;
            }
            return mapdesc.Substring(i + 1);
        }

        //private string LoadConfigurations(string mapdesc)
        //{
        //    var kv = new StringBuilder();
        //    int i = 0;
        //    for (i = 0; i < mapdesc.Length; i++)
        //    {
        //        char c = mapdesc[i];
        //        if (c == ';')
        //        {
        //            if (kv.Length > 0)
        //            {
        //                var config = kv.ToString().Split('=');
        //                ProcessConfig(config[0], config[1]);
        //                kv.Clear();
        //            }
        //            break;
        //        }

        //        if (c == '\n' && kv.Length > 0)
        //        {
        //            var config = kv.ToString().Split('=');
        //            ProcessConfig(config[0], config[1]);
        //            kv.Clear();
        //        }
        //        else if (c != '\n')
        //        {
        //            kv.Append(c);
        //        }
        //    }
        //    return mapdesc.Substring(i + 1);
        //}

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

        //private void ProcessConfig(string key, string value)
        //{
        //    if (key == "CanFire")
        //        _player.GetComponent<PlayerController>().CanFire = bool.Parse(value);
        //    if (key == "Position")
        //    {
        //        var coords = value.Split(',');
        //        _player.SetPosition(float.Parse(coords[0]), float.Parse(coords[1]));
        //    }
        //}

        private void ProcessObject(string key, string x, string y)
        {
            //Vector2 pos = new Vector2(float.Parse(x), float.Parse(y));

            //if (key == "Kitty")
            //if (key == "MushContainer")

        }

        protected GameObject AddPlayer(GameObject crosshair)
        {
            var player = new GameObject("player")
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/adi")))
                .AddComponent(new PlayerController(crosshair))
                .AddComponent(new BoxCollider(30, 72).SetIsDynamic(true))
                .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/fire")))
                .AddChild(crosshair);
            AddGameObject(player);
            return player;
        }

        protected GameObject AddCamera(GameObject player)
        {
            var camera = new GameObject("camera")
                            .AddComponent(new Camera())
                            .AddComponent(new CameraFollow(player));

            AddGameObject(camera);
            return camera;
        }

        protected GameObject CreateCrosshair(Layer hudLayer)
        {
            return new GameObject("crosshair")
                .AddComponent(new SpriteRenderer(hudLayer, GetContent<Texture2D>("sprites/crosshair")));
        }

        protected GameObject AddKitty(float x, float y)
        {
            return AddGameObject()
                .SetPosition(x, y)
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/kitty")));
        }

        protected GameObject AddProp(float x, float y, string sprite)
        {
            return AddGameObject()
                .SetPosition(x, y)
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>(sprite)));
        }

        private void AddEnemy(int x, int y, Layer defaultLayer)
        {
            //var obj = new GameObject("enemy")
            //    .SetPosition(x * 96, y * 96)
            //    .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/enemy")))
            //    .AddComponent(new BoxCollider(80, 50))
            //    .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/hit")))
            //    .AddComponent(new Enemy(_player));

            //AddGameObject(obj);
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

        private void AddDoor(int x, int y, Layer mapLayer, char number)
        {
            if (number == '0')
                PlayerStart = new Vector2((x * 96) - 100, y * 96);

            var obj = new GameObject("door")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new BoxCollider(96, 96))
                .AddComponent(new Door(_doorRegistry, int.Parse("" + number)))
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
