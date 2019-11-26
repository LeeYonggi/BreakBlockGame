using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleBoxButton : MonoBehaviour
{
    private UIButton uiButton = null;

    // Start is called before the first frame update
    void Start()
    {
        uiButton = GetComponent<UIButton>();

        uiButton.onClick.Add(new EventDelegate(ActiveInvincibillity));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActiveInvincibillity()
    {
        InGameManager.Instance.OnInvincibleBox();
    }
}
