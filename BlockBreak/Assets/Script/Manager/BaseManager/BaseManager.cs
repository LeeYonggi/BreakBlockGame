using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Singleton<T> where T : class, new()
    {
        protected static T instance = null;

        public static T Instance 
        {
            get
            {
                if (instance == null)
                    instance = new T();

                return instance;
            }
        }

        public void DestroyInstance()
        {
            if (instance == null)
                return;

            instance = null;
        }
    }

    public interface BaseManager
    {
        void Awake();
        void Start();
        void Update();
        void FixedUpdate();
        void Destroy();
    }
}
