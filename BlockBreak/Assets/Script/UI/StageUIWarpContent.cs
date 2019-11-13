using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class StageUIWarpContent : UIWrapContent
{
    private Transform[] uiTable = null;

    [ContextMenu("Sort Based on Scroll Movement")]
    public override void SortBasedOnScrollMovement()
    {
        if (!CacheScrollView()) return;

        // Cache all children and place them in order
        mChildren.Clear();
        
        for (int i = 0; i < mTrans.childCount; ++i)
        {
            Transform t = mTrans.GetChild(i);
            if (hideInactive && !t.gameObject.activeInHierarchy) continue;
            mChildren.Add(t);
        }

        uiTable = new Transform[mTrans.childCount];

        ResetChildPositions();
        // Sort the list of children so that they are in order
        if (mHorizontal) mChildren.Sort(UIGrid.SortHorizontal);
        else mChildren.Sort(UIGrid.SortVertical);
    }

    public override void WrapContent()
    {
        float extents = itemSize * mChildren.Count * 0.5f;
        Vector3[] corners = mPanel.worldCorners;

        for (int i = 0; i < 4; ++i)
        {
            Vector3 v = corners[i];
            v = mTrans.InverseTransformPoint(v);
            corners[i] = v;
        }

        Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
        bool allWithinRange = true;
        float ext2 = extents * 2f;

        if (mHorizontal)
        {
            float min = corners[0].x - itemSize;
            float max = corners[2].x + itemSize;

            for (int i = 0, imax = mChildren.Count; i < imax; ++i)
            {
                Transform t = mChildren[i];
                float distance = t.localPosition.x - center.x;

                if (distance < -extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.x += ext2;
                    distance = pos.x - center.x;
                    int realIndex = Mathf.RoundToInt(pos.x / itemSize);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);
                    }
                    else allWithinRange = false;
                }
                else if (distance > extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.x -= ext2;
                    distance = pos.x - center.x;
                    int realIndex = Mathf.RoundToInt(pos.x / itemSize);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);
                    }
                    else allWithinRange = false;
                }
                else if (mFirstTime) UpdateItem(t, i);

                if (cullContent)
                {
                    distance += mPanel.clipOffset.x - mTrans.localPosition.x;
                    if (!UICamera.IsPressed(t.gameObject))
                        NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
                }
            }
        }
        else
        {
            float min = corners[0].y - itemSize;
            float max = corners[2].y + itemSize;

            ClearUITable();
            for (int i = 0, imax = mChildren.Count; i < imax; ++i)
            {
                Transform t = mChildren[i];
                float distance = t.localPosition.y - center.y;
                
                if (distance > extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.y -= ext2;
                    distance = pos.y - center.y;
                    int realIndex = Mathf.RoundToInt(pos.y / itemSize);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);

                        // Down Object
                        uiTable[i] = t;
                    }
                    else allWithinRange = false;
                }
                else if (mFirstTime) UpdateItem(t, i);

                if (cullContent)
                {
                    distance += mPanel.clipOffset.y - mTrans.localPosition.y;
                    if (!UICamera.IsPressed(t.gameObject))
                        NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
                }
            }

            DownTable();

            ClearUITable();
            for (int i = mChildren.Count - 1; i >= 0; --i)
            {
                Transform t = mChildren[i];
                float distance = t.localPosition.y - center.y;

                if (distance < -extents)
                {
                    Vector3 pos = t.localPosition;
                    pos.y += ext2;
                    distance = pos.y - center.y;
                    int realIndex = Mathf.RoundToInt(pos.y / itemSize);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
                    {
                        t.localPosition = pos;
                        UpdateItem(t, i);

                        // Up Object
                        uiTable[i] = t;
                    }
                    else allWithinRange = false;
                }

                if (cullContent)
                {
                    distance += mPanel.clipOffset.y - mTrans.localPosition.y;
                    if (!UICamera.IsPressed(t.gameObject))
                        NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
                }
            }

            UpTable();
        }
        //mScroll.restrictWithinPanel = !allWithinRange;
        mScroll.InvalidateBounds();
    }

    void ClearUITable()
    {
        for(int i = 0; i < uiTable.Length; i++)
            uiTable[i] = null;
    }

    void PushUITable(Transform t, int index)
    {
        uiTable[index] = t;
    }

    delegate void UITransform(Transform t, int index);

    void DownTable()
    {
        for(int i = 0; i < uiTable.Length; i++)
        {
            if(uiTable[i])
            {
                RecursiveDownFuc(i);
            }
        }
    }

    void RecursiveDownFuc(int index)
    {
        int nextIndex = index - 1;
        nextIndex = (nextIndex < 0) ? mChildren.Count - 1 : nextIndex;

        if (uiTable[nextIndex])
        {
            RecursiveDownFuc(nextIndex);
            DownUITransform(uiTable[index], nextIndex);
        }
        else
        {
            DownUITransform(uiTable[index], nextIndex);
        }
        uiTable[index] = null;
    }

    void UpTable()
    {
        for (int i = uiTable.Length - 1; i >= 0; --i)
        {
            if (uiTable[i])
            {
                RecursiveUpFuc(i);
            }
        }
    }

    void RecursiveUpFuc(int index)
    {
        int nextIndex = index + 1;
        nextIndex = (nextIndex >= mChildren.Count) ? 0 : nextIndex;

        if(uiTable[nextIndex])
        {
            RecursiveUpFuc(nextIndex);
            UpUITransform(uiTable[index], nextIndex);
        }
        else 
        {
            UpUITransform(uiTable[index], nextIndex);
        }
        uiTable[index] = null;
    }

    void DownUITransform(Transform t, int nextIndex)
    {
        StageSelectLine line = mChildren[nextIndex].transform.GetComponent<StageSelectLine>();

        int stageNumber = line.GetStageMaxNumber();

        t.GetComponent<StageSelectLine>().SetStageNumber(stageNumber + 1);

        Debug.Log(stageNumber);
    }

    void UpUITransform(Transform t, int nextIndex)
    {
        StageSelectLine line = mChildren[nextIndex].transform.GetComponent<StageSelectLine>();

        int stageNumber = line.GetStageMinNumber();

        t.GetComponent<StageSelectLine>().SetStageNumber(stageNumber - 5);

        Debug.Log(stageNumber);
    }
}
