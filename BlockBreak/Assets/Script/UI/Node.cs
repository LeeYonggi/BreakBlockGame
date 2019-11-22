using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    private NodeData nodeData = new NodeData(0, NodeData.NODE_STATE.NODE_BOX, NodeData.DIRECTION_STATE.DEGREE_0);

    public NodeData NodeData { get => nodeData; set => nodeData = value; }

    private UILabel childLabel = null;


    // Start is called before the first frame update
    void Start()
    {
        childLabel = GetComponentInChildren<UILabel>();

        childLabel.text = nodeData.count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null)
            gameObject.SetActive(false);
    }
}
