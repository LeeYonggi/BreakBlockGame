using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallButton : MonoBehaviour
{
    private UIButton uiButton = null;

    // Start is called before the first frame update
    void Start()
    {
        uiButton = GetComponent<UIButton>();

        uiButton.onClick.Add(new EventDelegate(FireBall));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireBall()
    {
        BallManager.Instance.OnUIBallFire();
    }
}
