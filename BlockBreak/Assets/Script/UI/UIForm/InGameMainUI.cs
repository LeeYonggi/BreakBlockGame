using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager;
using UnityEngine;

namespace UIForm
{
    class InGameMainUI : NGUIForm
    {
        public override void Start()
        {
            base.Start();
            

            AddChildClickEvent("TopLeft/Pause", OpenPause);

            AddChildClickEvent("TopRight/TimeFaster", TimeFasterClick);

            AddChildClickEvent("Bottom/DownButton", BallDownClick);
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Update()
        {
            base.Update();
        }

        void TimeFasterClick(GameObject gameObject)
        {
            InGameManager.Instance.TimeState = InGameManager.Instance.ChangeTimeState(InGameManager.Instance.TimeState);
            Time.timeScale = InGameManager.Instance.GetStateToTime(InGameManager.Instance.TimeState);
        }

        void BallDownClick(GameObject gameObject)
        {
            BallManager.Instance?.BallReturn();
        }

        void OpenPause()
        {
            NGUIFormManager.Instance.OpenWindow("UIForm.PauseUI");
        }
    }
}
