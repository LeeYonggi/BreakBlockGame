using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class StageUIWarpContent : UIWrapContent
{
    private List<StageSelectLine> mUIChildren = new List<StageSelectLine>();
    private new LinkedList<StageSelectLine> mChildren = new LinkedList<StageSelectLine>();

    [ContextMenu("Sort Based on Scroll Movement")]
    public override void SortBasedOnScrollMovement()
    {
        if (!CacheScrollView()) return;

        // Cache all children and place them in order
        mChildren.Clear();

        List<Transform> objList = new List<Transform>();

        for (int i = 0; i < mTrans.childCount; ++i)
        {
            Transform t = mTrans.GetChild(i);
            if (hideInactive && !t.gameObject.activeInHierarchy) continue;
            objList.Add(t);
        }

        ResetChildPositions(objList);
        // Sort the list of children so that they are in order
        if (mHorizontal) objList.Sort(UIGrid.SortHorizontal);
        else objList.Sort(UIGrid.SortVertical);

        for (int i = 0; i < objList.Count; i++)
            mUIChildren.Add(objList[i].GetComponent<StageSelectLine>());

        for (int i = 0; i < objList.Count; i++)
            mChildren.AddLast(objList[i].GetComponent<StageSelectLine>());

        if(MainManager.Instance.StageInfo.nowStage != 0)
            MoveToStage(MainManager.Instance.StageInfo.nowStage);
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

        float min = corners[0].y - itemSize;
        float max = corners[2].y + itemSize;

        // 링크드리스트를 이용하여 y값을 정렬하였다.
        for (var node = mChildren.First; node != null;)
        {
            bool isDown = false;
            Transform t = node.Value.transform;
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

                    // Down Object
                    isDown = true;
                }
                else allWithinRange = false;
            }

            if (cullContent)
            {
                //distance += mPanel.clipOffset.y - mTrans.localPosition.y;
                //if (!UICamera.IsPressed(t.gameObject))
                //    NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
            }
            // 스테이지 번호 바꿈
            if (isDown)
            {
                UIStageDown(node.Value, mChildren.Last.Value);

                mChildren.AddLast(node.Value);
                
                var nextNode = node.Next;
                
                mChildren.Remove(node);
                node = nextNode;
            }
            else
            {
                node = node.Next;
            }
        }

        for (var node = mChildren.Last; node != null;)
        {
            bool isUp = false;
            Transform t = node.Value.transform;
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

                    // Up Object
                    isUp = true;
                }
                else allWithinRange = false;
            }

            if (cullContent)
            {
                //distance += mPanel.clipOffset.y - mTrans.localPosition.y;
                //if (!UICamera.IsPressed(t.gameObject))
                //    NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
            }
            // 스테이지 번호 바꿈
            if(isUp)
            {
                UIStageUp(node.Value, mChildren.First.Value);

                mChildren.AddFirst(node.Value);

                var nextNode = node.Previous;

                mChildren.Remove(node);
                node = nextNode;
            }
            else
            {
                node = node.Previous;
            }
        }

        //mScroll.restrictWithinPanel = !allWithinRange;
        mScroll.InvalidateBounds();
    }

    /// <summary>
    /// 스크롤바가 해당 스테이지 포지션으로 이동
    /// </summary>
    /// <param name="stageNumber"></param>
    public void MoveToStage(int stageNumber)
    {
        Vector3 targetPosition = mScroll.transform.localPosition;

        Vector3[] corners = mPanel.worldCorners;

        float scrollDistance = mScroll.GetComponent<UIPanel>().GetViewSize().y;
        float maxPos = (maxIndex - minIndex + 1) * itemSize - scrollDistance;
        targetPosition.y = ((stageNumber - 1) / 5) * itemSize;

        if (maxPos < targetPosition.y)
            targetPosition.y = maxPos;

        mScroll.transform.localPosition = targetPosition;
    }

    public void MoveToStageStr(string text)
    {
        MoveToStage(int.Parse(text));
    }

    void UIStageDown(StageSelectLine node, StageSelectLine prev)
    {
        int stageNumber = prev.GetStageMaxNumber();

        node.SetStageNumber(stageNumber + 1);
    }

    void UIStageUp(StageSelectLine node, StageSelectLine prev)
    {
        int stageNumber = prev.GetStageMinNumber();

        node.SetStageNumber(stageNumber - 5);
    }

}
