using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UIForm;
using UnityEngine.SceneManagement;

namespace Manager
{
    internal class NGUIFormManager : Singleton<NGUIFormManager>, BaseManager
    {
        private Dictionary<string, NGUIForm> uiForms = new Dictionary<string, NGUIForm>();
        private Dictionary<string, GameObject> prefabPacks = new Dictionary<string, GameObject>();

        private UIRoot nowUIRoot = null;
        private Camera camera = null;

        bool isFirstTouch = true;

        public UIRoot NowUIRoot 
        {
            get
            {
                if (nowUIRoot == null)
                    CreateUIRoot();

                return nowUIRoot;
            }
        }

        public Camera Camera { get => camera; set => camera = value; }

        public NGUIFormManager()
        {
            nowUIRoot = GameObject.FindObjectOfType<UIRoot>();
            camera = nowUIRoot?.GetComponentInChildren<Camera>();
        }

        public virtual void Awake()
        {
        }

        public void AddPrefabResource(string path)
        {
            GameObject prefab = Resources.Load(path) as GameObject;

            prefab = GameObject.Instantiate(prefab, NowUIRoot.transform);
            prefab.SetActive(false);

            string[] name = path.Split('/');
            prefabPacks.Add(name[name.Length - 1], prefab);
        }

        /// <summary>
        /// UI Root를 생성
        /// </summary>
        public void CreateUIRoot()
        {
            GameObject temp = Resources.Load("Prefab/UI/UIRoot") as GameObject;

            temp = GameObject.Instantiate(temp);
            nowUIRoot = temp.AddComponent<UIRoot>();

            camera = nowUIRoot?.GetComponentInChildren<Camera>();
        }

        public void Destroy()
        {
            foreach(var form in uiForms)
            {
                form.Value.Destroy();
            }

            uiForms.Clear();
            prefabPacks.Clear();

            nowUIRoot = null;
            DestroyInstance();
        }

        public void FixedUpdate()
        {
            foreach(var forms in uiForms)
            {
                if(forms.Value.IsOpen)
                    forms.Value.FixedUpdate();
            }
        }

        public void Start()
        {
        }

        public void Update()
        {
            isFirstTouch = true;

            foreach(var forms in uiForms)
            {
                if (forms.Value.IsOpen)
                    forms.Value.Update();
            }
        }

        /// <summary>
        /// 클래스 정보로 UI를 오픈
        /// </summary>
        /// <param name="gameObject"></param>
        /// 클릭한 UI 정보
        /// <param name="className"></param>
        /// 오픈시킬 UIForm의 클래스 이름
        public void OpenWindow(string className)
        {
            if (uiForms.ContainsKey(className) == false)
            {
                Type type = Type.GetType(className);

                if (type == null)
                    Debug.LogError($"{className} not class name");

                try 
                { 
                    NGUIForm uiForm = Activator.CreateInstance(type) as NGUIForm;

                    uiForms.Add(className, uiForm);
                    uiForms[className].Start();
                }
                catch(Exception e)
                {
                    Debug.LogError($"NGUIForm cannot casting {className}");
                }
            }

            uiForms[className].OpenForm();
        }

        /// <summary>
        /// 클래스 정보로 UI를 닫음
        /// </summary>
        /// <param name="gameObject"></param>
        /// 클릭한 UI 정보
        /// <param name="className"></param>
        /// class 이름
        public void CloseWindow(string className)
        {
            if (isFirstTouch == false)
                return;

            if (uiForms.ContainsKey(className) == false)
            {
                Debug.LogError($"{className} is not found");
                return;
            }

            uiForms[className].CloseForm();

            isFirstTouch = false;
        }

        /// <summary>
        /// 프리팹 테이블에서 키값에 맞는 오브젝트를 반환합니다.
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public GameObject GetPrefabFromPacks(string objName)
        {
            return prefabPacks[objName];
        }
    }
}
