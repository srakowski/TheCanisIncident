using Coldsteel;
using Coldsteel.Renderers;
using Coldsteel.Transitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
using TheCanisIncident.Behaviors;

namespace TheCanisIncident.Stages
{
    class MainMenuStage : GameStage
    {
        private GameObject _camera;

        private GameObject _playStoryButton;

        private GameObject _playEndlessButton;

        protected override void LoadContent()
        {
            LoadContent<Texture2D>("sprites/title", "sprites/playendless", "sprites/playstory",
                "sprites/pointer");

            var so = LoadContent<Song>("audio/menu");
            MediaPlayer.Play(so);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Initialize()
        {
            var hudLayer = AddLayer("hud", 1);
            
            AddGameObject()
                .AddComponent(new Pointer(OnClick))
                .AddComponent(new SpriteRenderer(hudLayer, GetContent<Texture2D>("sprites/pointer")));

            _camera = AddGameObject()
                .AddComponent(new Camera().SkipLayer(hudLayer));
            
            AddGameObject()
                .SetPosition(0, -240)
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/title")));

            _playStoryButton = AddGameObject()
                .SetPosition(0, -40)
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/playstory")));

            _playEndlessButton = AddGameObject()
                .SetPosition(0, 100)
                .AddComponent(new SpriteRenderer(DefaultLayer, GetContent<Texture2D>("sprites/playendless")));
        }

        private void OnClick(Vector2 screenClickPos)
        {
            var cam = _camera.GetComponent<Camera>();
            var pointerPos = ScreenPosToLayerPos(screenClickPos, DefaultLayer).ToPoint();
            var pointerBounds = new Rectangle(pointerPos.X, pointerPos.Y, 1, 1);
            CheckButtonClick(pointerBounds, _playStoryButton, () => GameStageManager.LoadStage("Paper1Stage", new GameData()));
            CheckButtonClick(pointerBounds, _playEndlessButton, () => GameStageManager.LoadStage("EntranceStage", new GameData() { Endless = true }));
        }

        private void CheckButtonClick(Rectangle pointerBounds, GameObject button, Action doThis)
        {
            var position = button.Transform.Position.ToPoint();
            var texture = button.GetComponent<SpriteRenderer>().Texture;
            var bounds = new Rectangle(position.X - (texture.Width / 2), position.Y - (texture.Height / 2), texture.Width, texture.Height);
            if (bounds.Intersects(pointerBounds))
                doThis.Invoke();
        }
    }
}
