using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTab : MonoBehaviour
{
    [SerializeField]
    private GameObject panelUI = null;

    private UISprite uiSprite = null;

    private readonly string[] spriteName = { "Bright", "Beveled Outline" };


    // Start is called before the first frame update
    void Start()
    {
        uiSprite = GetComponent<UISprite>();

    }

    // Update is called once per frame
    void Update()
    {
        if (panelUI.gameObject.activeSelf)
            uiSprite.spriteName = spriteName[1];
        else
            uiSprite.spriteName = spriteName[0];
    }
    
    /// <summary>
    /// 자신이 가지고 있는 패널을 활성화. 나머지 패널들을 비활성화 시킨다.
    /// </summary>
    public void OnClickActive()
    {
        var parentTransform = transform.parent.transform;

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            if (parentTransform.GetChild(i).gameObject == gameObject)
                continue;
            
            var tabObj = parentTransform.GetChild(i).GetComponent<ListTab>();

            if (tabObj == null)
                continue;

            tabObj.panelUI.SetActive(false);
            tabObj.uiSprite.spriteName = spriteName[0];
        }

        panelUI.SetActive(true);
        uiSprite.spriteName = spriteName[1];
    }
}
