﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectMaker : MonoBehaviour
{
    [SerializeField]
    private int stageCount = 0;
    [SerializeField]
    private GameObject selectUI = null;

    private UIWrapContent wrapScript = null;

    private UIGrid uiGrid = null;

    private BoxCollider mBoxCollider = null;

    public Vector2 BoxSize { get => mBoxCollider.size; }

    private void Awake()
    {
        uiGrid = GetComponent<UIGrid>();
        mBoxCollider = selectUI.GetComponent<BoxCollider>();

        int count = 1;

        for (int i = 1; i <= stageCount; i++)
        {
            GameObject obj = GameObject.Instantiate(selectUI, transform);

            for (int child = 0; child < obj.transform.childCount; ++child)
            {
                StageSelectButton uiLabel = obj.transform.GetChild(child).GetComponent<StageSelectButton>();

                uiLabel.StageNumber = count;

                count += 1;
            }
        }

        //uiGrid.enabled = false;
        //uiGrid.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {

        //wrapScript = gameObject.AddComponent<UIWrapContent>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
