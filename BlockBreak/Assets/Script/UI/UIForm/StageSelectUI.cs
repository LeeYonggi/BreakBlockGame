using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UIForm;
using Manager;

namespace UIForm
{
    class StageSelectUI : NGUIForm
    {
        public override void Start()
        {
            GameObject uiPrefab = Resources.Load("Prefab/UI/MainMenu/StageSelectUI") as GameObject;

            uiObject = GameObject.Instantiate(uiPrefab);

            var openLogButton = uiObject.transform.Find("LeftTop/ChatLogButton").gameObject;

            UIEventListener.Get(openLogButton).onClick = OpenLog;

            base.Start();
        }

        public void OpenLog(GameObject gameObject)
        {
            NGUIFormManager.Instance.OpenWindow(gameObject, "UIForm.ChatLogUI");
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}
