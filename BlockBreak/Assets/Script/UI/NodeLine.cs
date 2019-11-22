using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LineStatus
{
    public int lineNumber;

    public LineStatus(int number)
    {
        lineNumber = number;
    }
}

public class NodeLine : MonoBehaviour
{
    private LineStatus lineStatus = new LineStatus(0);

    public LineStatus LineStatus { get => lineStatus; set => lineStatus = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
