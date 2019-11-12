using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    private UILabel label = null;
    private UIButton uiButton = null;

    private int stageNumber = 1;
    public int StageNumber
    {
        get => stageNumber;
        set
        {
            stageNumber = value;
            if(label)
                label.text = stageNumber.ToString();
        }
    }



    // Start is called before the first frame update
    void Awake()
    {
        label = GetComponentInChildren<UILabel>();
        uiButton = GetComponent<UIButton>();

        uiButton.onClick.Add(new EventDelegate(OnClickUI));
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
