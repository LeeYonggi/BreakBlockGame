using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NodeData
{
    public int count;
    public NODE_STATE state;
    public DIRECTION_STATE dirState;


    public enum NODE_STATE
    {
        NONE,
        NODE_BOX,
        NODE_TRIANGLE,
        NODE_ITEM
    }

    public enum DIRECTION_STATE
    {
        DEGREE_0    = 0,
        DEGREE_90   = 90,
        DEGREE_180  = 180,
        DEGREE_270  = 270
    }

    public NodeData(int _count, NODE_STATE _state, DIRECTION_STATE _dir)
    {
        count = _count;
        state = _state;
        dirState = _dir;
    }

    public void ChangeDirection()
    {
        switch (dirState)
        {
            case DIRECTION_STATE.DEGREE_0:
                dirState = DIRECTION_STATE.DEGREE_90;
                break;
            case DIRECTION_STATE.DEGREE_90:
                dirState = DIRECTION_STATE.DEGREE_180;
                break;
            case DIRECTION_STATE.DEGREE_180:
                dirState = DIRECTION_STATE.DEGREE_270;
                break;
            case DIRECTION_STATE.DEGREE_270:
                dirState = DIRECTION_STATE.DEGREE_0;
                break;
        }
    }
}