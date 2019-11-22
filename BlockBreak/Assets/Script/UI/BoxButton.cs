using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxButton : MonoBehaviour
{
    [SerializeField]
    private NodeData nodeData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        int count = MapDataManager.Instance.selectData.count;

        // 현재 박스의 정보를 단일 객체에 전달한다.
        MapDataManager.Instance.selectData = nodeData;

        MapDataManager.Instance.selectData.count = count;
    }
}
