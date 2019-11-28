using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UIForm
{
    public abstract class NGUIForm
    {
        protected GameObject uiObject = null;

        public delegate void OnEnableFunc();

        public event OnEnableFunc openEvent = null;

        
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Destroy() { }


        public void OpenForm()
        {
            uiObject.SetActive(true);

            openEvent?.Invoke();
        }
        public void CloseForm()
        {
            uiObject.SetActive(false);

            openEvent?.Invoke();
        }
    }
}
