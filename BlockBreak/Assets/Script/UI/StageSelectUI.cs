﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        SceneManager.LoadScene(1);
        DataManager.Instance.ResourceMapPath = $"MapFile/map2/mapdata{label.text}";
        DataManager.Instance.NowStage = int.Parse(label.text);
    }
}
