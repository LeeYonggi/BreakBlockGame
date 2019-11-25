using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;

[Serializable]
public struct BoxStatus
{
    public enum OBJECT_STATE
    {
        NONE,
        BOX,
        TRIANGLE,
        ITEM
    }

    public Point pos;
    public Vector2 scale;
    public float rotation;
    public OBJECT_STATE objState;

    public BoxStatus(Point _point, Vector2 _scale, int _boxCount, OBJECT_STATE state)
    {
        boxCount = _boxCount;
        pos = _point;
        scale = _scale;
        objState = state;
        rotation = 0;
    }

    public static BoxStatus Zero()
    {
        return new BoxStatus(new Point(0, 0), new Vector2(1, 1), 1, OBJECT_STATE.NONE);
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
    BoxStatus boxStatus = BoxStatus.Zero();

    private bool isInvincibility = false;

    public BoxStatus BoxStatus { get => boxStatus; set => boxStatus = value; }
    public bool IsInvincibility { get => isInvincibility; set => isInvincibility = value; }


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
        if (isInvincibility) return;

        boxStatus.boxCount -= 1;
        countText.text = boxStatus.boxCount.ToString();

        if (boxStatus.boxCount < 1)
        {
            InGameManager.Instance.DestroyBox(gameObject);

            //Camera.main.GetComponent<CameraController>().CameraShake(0.05f, 0.6f);
        }
    }
}
