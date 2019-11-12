using System.Collections;
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

    private void Awake()
    {
        uiGrid = GetComponent<UIGrid>();

        int count = 1;

        for (int i = 1; i <= stageCount; i++)
        {
            GameObject obj = GameObject.Instantiate(selectUI, transform);

            for (int child = 0; child < obj.transform.childCount; ++child)
            {
                StageSelectUI uiLabel = obj.transform.GetChild(child).GetComponent<StageSelectUI>();

                uiLabel.StageNumber = count;

                count += 1;
            }
        }

        //uiGrid.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //uiGrid.enabled = true;

        //wrapScript = gameObject.AddComponent<UIWrapContent>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
