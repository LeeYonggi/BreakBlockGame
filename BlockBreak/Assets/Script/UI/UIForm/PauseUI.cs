using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Manager;
using DG.Tweening;

namespace UIForm
{
    class PauseUI : NGUIForm
    {
        public override void Start()
        {
            base.Start();

            AddChildClickEvent("Center/PauseMain/Resume", OnClosePause);
            AddChildClickEvent("Center/PauseMain/Exit", OnExit);
            AddChildClickEvent("Center/PauseMain/Restart", OnReplay);

            openEvent += OnOpenPause;

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Update()
        {
            base.Update();
        }
        public override void Destroy()
        {
            base.Destroy();
        }

        void OnOpenPause()
        {
            Vector3 nowScale = uiObject.transform.localScale;

            uiObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            uiObject.transform.DOScale(nowScale, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);

            Time.timeScale = 0.0f;
        }

        void OnClosePause(GameObject gameObject)
        {
            Vector3 nowScale = uiObject.transform.localScale;

            uiObject.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBack).OnComplete(
                () => CloseEnd(nowScale)
                );

            Time.timeScale = InGameManager.Instance.GetStateToTime(InGameManager.Instance.TimeState);
        }

        void OnExit(GameObject gameObject)
        {
            Time.timeScale = InGameManager.Instance.GetStateToTime(InGameManager.Instance.TimeState);

            GameSceneManager.Instance.ChangeScene(GameSceneManager.SCENE_KIND.MAIN_MENU);
        }

        void CloseEnd(Vector3 nowScale)
        {
            uiObject.transform.localScale = nowScale;

            NGUIFormManager.Instance.CloseWindow("UIForm.PauseUI");
        }

        void OnReplay()
        {
            uiObject.SetActive(false);

            Time.timeScale = InGameManager.Instance.GetStateToTime(InGameManager.Instance.TimeState);

            Manager.GameSceneManager.Instance.ChangeScene(Manager.GameSceneManager.SCENE_KIND.INGAME);
        }
    }
}
