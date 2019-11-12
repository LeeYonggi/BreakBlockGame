using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectLine : MonoBehaviour
{
    private List<StageSelectUI> stageUIList = new List<StageSelectUI>();

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            stageUIList.Add(transform.GetChild(i).GetComponent<StageSelectUI>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStageNumber(int minNumber)
    {
        for(int i = 0; i < stageUIList.Count; i++)
        {
            stageUIList[i].StageNumber = minNumber + i;
        }
    }

    public int GetStageMaxNumber()
    {
        return stageUIList[stageUIList.Count - 1].StageNumber;
    }

    public int GetStageMinNumber()
    {
        return stageUIList[0].StageNumber;
    }
}
