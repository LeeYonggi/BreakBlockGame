﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAngleButton : MonoBehaviour
{
    private UIButton uiButton = null;

    [SerializeField]
    private UILabel inputLabel = null;

    // Start is called before the first frame update
    void Start()
    {
        uiButton = GetComponent<UIButton>();

        uiButton.onClick.Add(new EventDelegate(ResetAngle));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetAngle()
    {
        BallManager.Instance.TransformAngle(inputLabel.text);
    }
}
