using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectMaker : MonoBehaviour
{
    [SerializeField]
    private int stageCount = 0;
    [SerializeField]
    private GameObject selectUI = null;

    private UIGrid uiGrid = null;

    private void Awake()
    {
        uiGrid = GetComponent<UIGrid>();

        for (int i = 1; i <= stageCount; i++)
        {
            GameObject obj = GameObject.Instantiate(selectUI, transform);

            UILabel uiLabel = obj.GetComponentInChildren<UILabel>();

            uiLabel.text = i.ToString();
        }

        uiGrid.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiGrid.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
