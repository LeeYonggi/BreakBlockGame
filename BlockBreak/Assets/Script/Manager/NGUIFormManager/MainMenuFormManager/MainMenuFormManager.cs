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
            NGUIFormManager.Instance.AddPrefabResource("Prefab/UI/MainMenu/StageSelectUI");
            NGUIFormManager.Instance.AddPrefabResource("Prefab/UI/MainMenu/ChatLogUI");

            NGUIFormManager.Instance.OpenWindow("UIForm.StageSelectUI");
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
