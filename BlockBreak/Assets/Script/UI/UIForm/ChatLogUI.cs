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
        public override void Start()
        {
            base.Start();

            AddChildClickEvent("ChatLogUIWindow/Close", CloseChatLog);
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

        void CloseChatLog()
        {
            NGUIFormManager.Instance.CloseWindow("UIForm.ChatLogUI");
        }
    }
}