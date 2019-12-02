using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager;
using UnityEngine;

namespace UIForm
{
    class BallCountUI : NGUIForm
    {
        UILabel label = null;
        public override void Start()
        {
            base.Start();

            label = uiObject.GetComponent<UILabel>();
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

            if (BallManager.Instance.FirstBall)
            {
                Vector2 ballPos = BallManager.Instance.FirstBall.transform.position;
                const float hCorrection = -0.4f;

                ballPos.y += hCorrection;

                Vector2 screenPos = Camera.main.WorldToScreenPoint(ballPos);

                screenPos.x -= Screen.width * 0.5f;
                screenPos.y -= NGUIFormManager.Instance.NowUIRoot.activeHeight * 0.5f;

                uiObject.transform.localPosition = screenPos;

                label.text = $"*{BallManager.Instance.FindStopBallCount()}";
            }
            else
                label.text = string.Empty;
        }
    }
}
