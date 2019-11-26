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

        
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Destroy() { }


    }
}
