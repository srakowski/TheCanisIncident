using Coldsteel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TheCanisIncident.Behaviors
{
    class CutScene : Behavior
    {
        private object _param;

        public CutScene(string stageName, object param)
        {
            _param = param;
            StartCoroutine(LoadNextStage(stageName));
        }

        private IEnumerator LoadNextStage(string stageName)
        {
            yield return WaitMSecs(5000);
            GameStageManager.LoadStage(stageName, _param);
        }
    }
}
