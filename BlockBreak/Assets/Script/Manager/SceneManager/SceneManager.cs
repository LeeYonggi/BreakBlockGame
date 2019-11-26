﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Manager
{
    class SceneManager : Singleton<SceneManager>
    {
        enum SCENE_KIND
        {
            MAIN_MENU = 0,
            INGAME    = 1,
            MAPTOOL   = 2
        }

        UnityEngine.SceneManagement.Scene nowScene = default;       // 현재 씬

        List<BaseManager> managerList = new List<BaseManager>();    // 관리할 매니져리스트

        bool isSceneLoaded = false;                                 // 씬이 로드되었는지

        public SceneManager()
        {
            nowScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        public void Awake()
        {
            AddManager((SCENE_KIND)nowScene.buildIndex);

            AwakeManager();
        }

        private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
        {
            nowScene = arg1;

            isSceneLoaded = true;
        }

        public void Start()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

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

                    break;
                case SCENE_KIND.INGAME:
                    managerList.Add(BallManager.Instance);
                    managerList.Add(StageManager.Instance);
                    managerList.Add(InGameManager.Instance);

                    break;
                case SCENE_KIND.MAPTOOL:
                    break;
            }
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
            if(isSceneLoaded)
                ChangeSceneAwake();

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

        public void ChangeScene(int buildNumber) // enum 값으로
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(buildNumber);
        }

        private void ChangeSceneAwake()
        {
            DestroyScenes();

            Awake();

            Start();

            isSceneLoaded = false;
        }

        private void DestroyScenes()    // find로 찾는 애들 깨짐
        {
            while(managerList.Count > 0)
            {
                managerList[0].Destroy();
                managerList.RemoveAt(0);
            }
        }
    }
}