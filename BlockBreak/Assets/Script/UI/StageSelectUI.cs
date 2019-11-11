using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    private UILabel label = null;

    // Start is called before the first frame update
    void Start()
    {
        label = GetComponentInChildren<UILabel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickUI()
    {
        SceneManager.LoadScene(0);
        DataManager.Instance.NowStage = int.Parse(label.text);
    }
}
