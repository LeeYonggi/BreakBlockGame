using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    class GameSceneManager : Singleton<GameSceneManager>
    {
        public enum SCENE_KIND
        {
            MAIN_MENU = 0,
            INGAME    = 1,
            MAPTOOL   = 2
        }

        Scene nowScene = default;                                   // 현재 씬

        List<BaseManager> managerList = new List<BaseManager>();    // 관리할 매니져리스트

        bool isSceneLoaded = false;                                 // 씬이 로드되었는지

        public GameSceneManager()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            nowScene = arg0;

            isSceneLoaded = true;

            ChangeSceneAwake();

            Debug.Log($"LoadSceneMode : {arg1}");
        }

        public void Awake()
        {
            DestroyScenes();

            AddManager((SCENE_KIND)nowScene.buildIndex);

            AwakeManager();

            StartManager();
        }

        public void StartManager()
        {
            foreach (var manager in managerList)
            {
                manager.Start();
            }
        }

        private void AddManager(SCENE_KIND sceneKind)
        {
            switch (sceneKind)
            {
                case SCENE_KIND.MAIN_MENU:
                    managerList.Add(MainMenuFormManager.Instance);

                    break;
                case SCENE_KIND.INGAME:
                    managerList.Add(BallManager.Instance);
                    managerList.Add(StageManager.Instance);
                    managerList.Add(InGameManager.Instance);

                    break;
                case SCENE_KIND.MAPTOOL:
                    break;
            }

            managerList.Add(NGUIFormManager.Instance);
        }


        private void AwakeManager()
        {
            foreach(var manager in managerList)
            {
                manager.Awake();
            }
        }

        public void Update()
        {
            foreach (var manager in managerList)
            {
                manager.Update();
            }
        }

        public void FixedUpdate()
        {
            foreach (var manager in managerList)
            {
                manager.FixedUpdate();
            }
        }

        public void ChangeScene(SCENE_KIND scene) 
        {
            GameObject loadScreen = Resources.Load("Prefab/UI/Loading/LoadScreen") as GameObject;

            loadScreen = GameObject.Instantiate(loadScreen);

            MainManager.Instance.StartCoroutine(LoadScene(scene));
        }

        IEnumerator LoadScene(SCENE_KIND sceneNumber)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneNumber);

            asyncOperation.allowSceneActivation = false;

            DestroyScenes();    // find로 찾는 애들 깨질 수 있으니 바로 삭제시켜줌

            while(!asyncOperation.isDone)
            {
                Debug.Log($"Load progress : {asyncOperation.progress}");

                if (asyncOperation.progress >= 0.9f)
                    asyncOperation.allowSceneActivation = true;

                yield return null;
            }
        }

        private void ChangeSceneAwake()
        {
            Awake();

            isSceneLoaded = false;
        }

        private void DestroyScenes()    
        {
            while(managerList.Count > 0)
            {
                managerList[0].Destroy();
                managerList.RemoveAt(0);
            }
        }
    }
}