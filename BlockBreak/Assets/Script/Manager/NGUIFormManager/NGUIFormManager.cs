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
    public class NGUIFormManager : Singleton<NGUIFormManager>, BaseManager
    {
        protected Dictionary<string, NGUIForm> uiForms = new Dictionary<string, NGUIForm>();

        private UIRoot nowUIRoot = null;

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

        public virtual void Awake()
        {
            nowUIRoot = GameObject.FindObjectOfType<UIRoot>();
        }

        public void CreateUIRoot()
        {
            Debug.LogWarning($"UIroot is not found this scene. please check " +
                $"{SceneManager.GetActiveScene()} scene");

            GameObject temp = new GameObject("UIRoot");

            nowUIRoot = temp.AddComponent<UIRoot>();
            temp.AddComponent<UIPanel>();

            var objRb = temp.AddComponent<Rigidbody>();

            objRb.isKinematic = true;
            objRb.useGravity = false;
        }

        public void Destroy()
        {
            DestroyInstance();
        }

        public void FixedUpdate()
        {
            foreach(var forms in uiForms)
            {
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
                forms.Value.Update();
            }
        }

        public void OpenWindow(GameObject param, string className)
        {
            if (isFirstTouch == false)
                return;

            if (uiForms.ContainsKey(className) == false)
            {
                Type type = Type.GetType(className);
                NGUIForm uiForm = Activator.CreateInstance(type) as NGUIForm;

                uiForms.Add(className, uiForm);
                uiForms[className].Start();
            }

            uiForms[className].OpenForm();

            isFirstTouch = false;
        }

        public void CloseWindow(GameObject param, string className)
        {
            if (isFirstTouch == false)
                return;

            if (uiForms.ContainsKey(className) == false)
            {
                Debug.LogError($"{className} is not found from {param.ToString()}");
                return;
            }

            uiForms[className].CloseForm();

            isFirstTouch = false;
        }
    }
}
