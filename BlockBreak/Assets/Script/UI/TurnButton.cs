using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButton : MonoBehaviour
{
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
        MapDataManager.Instance.selectData.ChangeDirection();
    }
}
