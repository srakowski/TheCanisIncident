using Coldsteel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Coldsteel.Controls;

namespace TheCanisIncident.Behaviors
{
    class CutScene : Behavior
    {
        private object _param;

        bool _skip = false;
        string _stageName;

        public CutScene(string stageName, object param)
        {
            _param = param;
            _stageName = stageName;
            StartCoroutine(LoadNextStage(stageName));
        }

        public override void Update(IGameTime gameTime)
        {
            if (Input.GetControl<ButtonControl>("MoveUp").IsDown() || Input.GetControl<ButtonControl>("MoveDown").IsDown() || 
                    Input.GetControl<ButtonControl>("MoveLeft").IsDown() || Input.GetControl<ButtonControl>("MoveRight").IsDown() ||
                Input.GetControl<ButtonControl>("Fire").IsDown() || Input.GetControl<ButtonControl>("MoveRight").IsDown())
            {
                _skip = true;
                GameStageManager.LoadStage(_stageName, _param);
            }
        }

        private IEnumerator LoadNextStage(string stageName)
        {
            yield return WaitMSecs(5000);
            if (_skip)
                yield break;

            GameStageManager.LoadStage(stageName, _param);
        }
    }
}
