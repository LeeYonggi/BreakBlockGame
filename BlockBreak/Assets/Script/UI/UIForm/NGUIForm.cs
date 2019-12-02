using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Manager;

namespace UIForm
{
    public abstract class NGUIForm
    {
        protected GameObject uiObject = null;

        public delegate void OnEnableFunc();

        public event OnEnableFunc openEvent = null;     // form을 열어줄 때 호출하는 이벤트
        public event OnEnableFunc closeEvent = null;    // form을 닫아줄 때 호출하는 이벤트

        public bool IsOpen { get => uiObject.activeSelf; }

        public virtual void Start() 
        {
            string[] nameArr = this.ToString().Split('.');
            AddUIObjectFromPacks(nameArr[nameArr.Length - 1]); 
        }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Destroy() 
        {
            GameObject.Destroy(uiObject);
        }

        protected void CreateObject(string prefabPath, bool isFormRoot = false)
        {
            GameObject prefab = Resources.Load(prefabPath) as GameObject;

            if(isFormRoot)
                uiObject = GameObject.Instantiate(prefab);
            else
                uiObject = GameObject.Instantiate(prefab, NGUIFormManager.Instance.NowUIRoot.transform);
        }

        public void OpenForm()
        {
            uiObject.SetActive(true);

            openEvent?.Invoke();
        }
        public void CloseForm()
        {
            uiObject.SetActive(false);

            closeEvent?.Invoke();
        }

        public void AddChildClickEvent(string childPath, UIEventListener.StringDelegate func, string parameter)
        {
            var openLogButton = uiObject.transform.Find(childPath).gameObject;

            UIEventListener.Get(openLogButton).onClickString = func;
            UIEventListener.Get(openLogButton).parameter = parameter;
        }

        public void AddChildClickEvent(string childPath, UIEventListener.VoidDelegate func)
        {
            var openLogButton = uiObject.transform.Find(childPath).gameObject;

            UIEventListener.Get(openLogButton).onClick = func;
        }

        public void AddChildClickEvent(string childPath, UIEventListener.NOParamDelegate func)
        {
            var openLogButton = uiObject.transform.Find(childPath).gameObject;

            UIEventListener.Get(openLogButton).onClickVoid = func;
        }

        protected void AddUIObjectFromPacks(string name)
        {
            uiObject = NGUIFormManager.Instance.GetPrefabFromPacks(name);
        }
    }
}
