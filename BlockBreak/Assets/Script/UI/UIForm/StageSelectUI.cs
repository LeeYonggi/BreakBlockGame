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
            base.Start();
            
            // 함수를 통해 OpenWindow 호출
            AddChildClickEvent("LeftTop/ChatLogButton", OpenChatLog);
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

        public void OpenChatLog()
        {
            NGUIFormManager.Instance.OpenWindow("UIForm.ChatLogUI");
        }
    }
}
