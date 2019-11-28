using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIForm;

namespace Manager
{
    public class MainMenuFormManager : Singleton<MainMenuFormManager>, BaseManager
    {
        public void Awake()
        {
            NGUIFormManager.Instance.OpenWindow(null, "UIForm.StageSelectUI");

        }
        public void Start()
        {
        }

        public void Update()
        {

        }
        public void FixedUpdate()
        {
        }
        public void Destroy()
        {
        }
    }
}
