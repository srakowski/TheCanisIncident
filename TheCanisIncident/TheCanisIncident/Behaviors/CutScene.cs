using Coldsteel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TheCanisIncident.Behaviors
{
    class CutScene : Behavior
    {
        public CutScene(string stageName)
        {
            StartCoroutine(LoadNextStage(stageName));
        }

        private IEnumerator LoadNextStage(string stageName)
        {
            yield return WaitMSecs(5000);
            GameStageManager.LoadStage(stageName);
        }
    }
}
