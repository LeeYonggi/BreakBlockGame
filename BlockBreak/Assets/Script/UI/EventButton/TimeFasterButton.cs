using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFasterButton : MonoBehaviour
{
    private UIButton uiButton = null;

    // Start is called before the first frame update
    void Start()
    {
        uiButton = GetComponent<UIButton>();

        uiButton.onClick.Add(new EventDelegate(TimeFaster));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TimeFaster()
    {
        InGameManager.Instance.ChangeTimeState(InGameManager.Instance.TimeState);
    }
}
