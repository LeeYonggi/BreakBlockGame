using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelUpItem : MonoBehaviour
{
    [SerializeField]
    private GameObject outLine = null;
    [SerializeField]
    private GameObject center = null;

    // Start is called before the first frame update
    void Start()
    {
        outLine.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        outLine.transform.DOScale(new Vector3(1.2f, 1.2f, 1.0f), 1).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        GameManager.Instance.DestroyItem(gameObject);
    }
}
