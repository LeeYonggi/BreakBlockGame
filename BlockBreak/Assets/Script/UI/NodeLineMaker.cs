using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLineMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject nodeLinePrefab = null;

    [SerializeField]
    private int lineCount = 10;

    [SerializeField]
    private int rangeLimit = 10;

    private MapWarpContent warpContent = null;

    public int RangeLimit { get => rangeLimit; set => rangeLimit = value; }

    void Awake()
    {
        for (int i = 0; i < lineCount; i++)
        {
            NodeLine nodeLine = GameObject.Instantiate(nodeLinePrefab, transform).GetComponent<NodeLine>();

            nodeLine.LineStatus = new LineStatus(i);

            for(int child = 0; child < nodeLine.transform.childCount; child++)
            {
                NodeButton button = nodeLine.transform.GetChild(child).GetComponent<NodeButton>();

                button.MIndex = child;
            }
        }

    }

    // Start is called before the first frame update
    private void Start()
    {
        warpContent = GetComponent<MapWarpContent>();

        InitializeRangeLimit(rangeLimit);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeRangeLimit(int range)
    {
        warpContent.minIndex = 0;
        warpContent.maxIndex = range - 1;

        MapDataManager.Instance.InitializeNodeData(range);
    }
}
