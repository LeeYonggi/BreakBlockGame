using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class MoveScene : MonoBehaviour
{
    [SerializeField]
    private UIButton uiButton = null;

    [SerializeField]
    private int nextScene = 0;

    // Start is called before the first frame update
    void Start()
    {
        uiButton.onClick.Add(new EventDelegate(MoveToScene));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToScene()
    {
        GameSceneManager.Instance.ChangeScene((GameSceneManager.SCENE_KIND)nextScene);
    }
}
