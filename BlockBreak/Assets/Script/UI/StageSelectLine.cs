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

    /// <summary>
    /// 줄에 있는 ui에 스테이지 숫자를 넣음
    /// </summary>
    /// <param name="minNumber"></param>
    /// 줄에 있는 ui 맨 왼쪽의 숫자. 왼쪽부터 오른쪽까지 1씩 증가시켜 넣어줌. 
    public void SetStageNumber(int minNumber)
    {
        for(int i = 0; i < stageUIList.Count; i++)
        {
            stageUIList[i].StageNumber = minNumber + i;
        }
    }

    /// <summary>
    /// 맨 오른쪽 ui에 있는 스테이지 숫자 반환
    /// </summary>
    /// <returns></returns>
    public int GetStageMaxNumber()
    {
        return stageUIList[stageUIList.Count - 1].StageNumber;
    }

    /// <summary>
    /// 맨 왼쪽 ui에 있는 스테이지 숫자 반환
    /// </summary>
    /// <returns></returns>
    public int GetStageMinNumber()
    {
        return stageUIList[0].StageNumber;
    }
}
