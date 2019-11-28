using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Manager;
using UIForm;

namespace UIForm
{
    class ChatLogUI : NGUIForm
    {
        public ChatLogUI()
        {

        }

        public override void Start()
        {
            GameObject chatPrefab = Resources.Load("Prefab/UI/MainMenu/ChatLogUI") as GameObject;

            uiObject = GameObject.Instantiate(chatPrefab, NGUIFormManager.Instance.NowUIRoot.transform);

            var openLogButton = uiObject.transform.Find("ChatLogUIWindow/Close").gameObject;

            UIEventListener.Get(openLogButton).onClick = CloseLog;
        }

        public override void Update()
        {

        }

        public override void FixedUpdate()
        {

        }

        public override void Destroy()
        {

        }

        public void CloseLog(GameObject gameObject)
        {
            NGUIFormManager.Instance.CloseWindow(gameObject, "UIForm.ChatLogUI");
        }
    }
}