using Coldsteel;
using Coldsteel.Colliders;
using Coldsteel.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TheCanisIncident.Behaviors;

namespace TheCanisIncident.Stages
{
    abstract class GameplayStage : GameStage
    {
        protected GameData Data { get { return Param as GameData; } }

        private string _map;

        private bool[,] TraversableLayout;

        protected Vector2 PlayerStart { get; set; } = Vector2.Zero;

        protected string NextStage { get; set; } = "MainMenuStage";

        private Queue<Vector2> _spawnLocations = new Queue<Vector2>();

        public GameplayStage(string map)
        {
            _map = map;
        }

        protected override void LoadContent()
        {
            LoadContent<Texture2D>("sprites/wall", "sprites/ceiling", "sprites/floor", 
                "sprites/adi", "sprites/crosshair", "sprites/bullet", "sprites/enemy",
                "sprites/item", "sprites/kitty", "sprites/mushcontainer",
                "sprites/healthbarfront", "sprites/healthbarback", "sprites/mush");

            LoadContent<SoundEffect>("audio/pickup", "audio/fire", "audio/hit",
                "audio/mush_xplode");
        }

        protected override void Initialize()
        {
            DefaultLayer.SpriteSortMode = SpriteSortMode.FrontToBack;

            var floorLayer = AddLayer("floor", -2);
            var itemsLayer = AddLayer("items", -1);
            var ceilingLayer = AddLayer("ceiling", 1);
            var chLayer = AddLayer("crosshair", 2);
            var hudLayer = AddLayer("hud", 3);            

            Load(_map);
        }

        protected void Load(string mapdesc)
        {
            LoadMap(mapdesc);
        }

        private void LoadMap(string mapdesc)
        {
            List<string> rows = new List<string>();
            var width = 0;
            var height = 0;

            string row = string.Empty;
            for (var i = 0; i < mapdesc.Length; i++)
            {
                char c = mapdesc[i];
                if (c == ';')
                    break;
                else if (c != '\n')
                    row += c;
                else
                {
                    if (row.Length > width)
                        width = row.Length;
                    height++;
                    rows.Add(row);
                    row = string.Empty;
                }
            }

            TraversableLayout = new bool[width, height];
            for (var tx = 0; tx < width; tx++)
                for (var ty = 0; ty < height; ty++)
                    TraversableLayout[tx, ty] = false;

            int x = 0, y = 0;
            foreach (var r in rows)
            {
                foreach (var c in r)
                {
                    if (c == '.')
                    {
                        AddFloor(x, y, GetLayer("floor"), true);
                        TraversableLayout[x, y] = true;
                    }
                    if (c == '-')
                    {
                        AddFloor(x, y, GetLayer("floor"), false);
                        TraversableLayout[x, y] = true;
                    }
                    if (c == 'X') 
                    {
                        AddExit(x, y, GetLayer("floor"));
                        TraversableLayout[x, y] = true;
                    }
                    if (c == 'E') 
                    {
                        AddEntry(x, y, GetLayer("floor"));
                        TraversableLayout[x, y] = true;
                    }
                    if (c == '#') AddCeiling(x, y, GetLayer("ceiling"));
                    if (c == 'W') AddWall(x, y, GetLayer("floor"));
                    x++;
                }
                x = 0;
                y++;
            }

            _spawnLocations = new Queue<Vector2>(_spawnLocations.OrderBy(_ => Guid.NewGuid()));
        }

        protected GameObject AddPlayer(GameObject crosshair)
        {
            var player = new GameObject("player")
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/adi")))
                .AddComponent(new PlayerController(crosshair, Data.Player, TraversableLayout))
                .AddComponent(new BoxCollider(30, 72).SetIsDynamic(true))
                .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/fire")))
                .AddChild(crosshair);
            AddGameObject(player);
            return player;
        }

        protected GameObject AddCamera(GameObject player)
        {
            var camera = new GameObject("camera")
                            .AddComponent(new Camera().SkipLayer(GetLayer("hud")))
                            .AddComponent(new CameraFollow(player));

            AddGameObject(camera);
            return camera;
        }

        protected GameObject CreateCrosshair()
        {
            return new GameObject("crosshair")
                .AddComponent(new SpriteRenderer(GetLayer("crosshair"), GetContent<Texture2D>("sprites/crosshair")));
        }

        protected void AddHud()
        {
            AddGameObject()
                .SetPosition(300, 160)
                .AddComponent(new SpriteRenderer(GetLayer("hud"), GetContent<Texture2D>("sprites/healthbarback")) { LayerDepth = 0 });
            AddGameObject()
                .SetPosition(300, 160)
                .AddComponent(new SpriteRenderer(GetLayer("hud"), GetContent<Texture2D>("sprites/healthbarfront")));
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

        private void AddCeiling(int x, int y, Layer mapLayer)
        {
            var obj = new GameObject("ceiling")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new SpriteRenderer(mapLayer, GetContent<Texture2D>("sprites/ceiling")));

            AddGameObject(obj);
        }

        private void AddWall(int x, int y, Layer mapLayer)
        {
            var obj = new GameObject("wall")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new SpriteRenderer(mapLayer, GetContent<Texture2D>("sprites/wall")));

            AddGameObject(obj);
        }

        private void AddFloor(int x, int y, Layer mapLayer, bool isPotentialSpawn)
        {
            var obj = new GameObject("floor")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new SpriteRenderer(mapLayer, GetContent<Texture2D>("sprites/floor")));

            AddGameObject(obj);
            if (isPotentialSpawn)
                this._spawnLocations.Enqueue(obj.Transform.Position);
        }

        private void AddExit(int x, int y, Layer mapLayer)
        {
            var obj = new GameObject("exit")
                .SetPosition(x * 96, y * 96)
                .AddComponent(new BoxCollider(96, 96))
                .AddComponent(new Exit(() => GameStageManager.LoadStage(this.NextStage, this.Param)))
                .AddComponent(new SpriteRenderer(mapLayer, GetContent<Texture2D>("sprites/floor")));

            AddGameObject(obj);
        }

        private void AddEntry(int x, int y, Layer mapLayer)
        {
            PlayerStart = new Vector2((x * 96) - 100, y * 96);

            var obj = new GameObject("entry")
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

        public void LoadMushPiles(GameObject player, int count)
        {
            for (var c = 0; c < count; c++)
                LoadMushPile(player, _spawnLocations.Dequeue());
        }

        private void LoadMushPile(GameObject player, Vector2 pos)
        {
            AddGameObject()
                .SetPosition(pos)
                .AddComponent(new MushPile(player, TraversableLayout))
                .AddComponent(new BoxCollider(80, 80))
                .AddComponent(new AudioSource(GetContent<SoundEffect>("audio/mush_xplode")))
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/mush")));
        }
    }
}
