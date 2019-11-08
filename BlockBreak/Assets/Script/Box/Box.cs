using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct BoxStatus
{
    public BoxStatus(int _boxCount)
    {
        boxCount = _boxCount;
    }
    public int boxCount;
}
public class Box : MonoBehaviour
{
    [SerializeField]
    private Text countText = null;
    [SerializeField]
    private Canvas canvas = null;

    [SerializeField]
    BoxStatus boxStatus = new BoxStatus(1);

    public BoxStatus BoxStatus { get => boxStatus; set => boxStatus = value; }


    // Start is called before the first frame update
    void Start()
    {
        countText.text = BoxStatus.boxCount.ToString();
        canvas.worldCamera = Camera.main;

        if (transform.localScale.x < 0)
        {
            Vector3 scale = canvas.transform.localScale;
            canvas.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
        if (transform.localScale.y < 0)
        {
            Vector3 scale = canvas.transform.localScale;
            canvas.transform.localScale = new Vector3(scale.x, -scale.y, scale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Ball"))
        {
            BoxHit();
        }
    }

   public void BoxHit()
    {
        boxStatus.boxCount -= 1;
        countText.text = boxStatus.boxCount.ToString();

        if (boxStatus.boxCount < 1)
        {
            GameManager.Instance.DestroyBox(gameObject);

            //Camera.main.GetComponent<CameraController>().CameraShake(0.05f, 0.6f);
        }
    }
}
