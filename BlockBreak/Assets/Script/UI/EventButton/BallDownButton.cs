using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDownButton : MonoBehaviour
{
    private UIButton uiButton = null;

    // Start is called before the first frame update
    void Start()
    {
        uiButton = GetComponent<UIButton>();

        uiButton.onClick.Add(new EventDelegate(BallDown));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BallDown()
    {
        BallManager.Instance.BallReturn();
    }
}
