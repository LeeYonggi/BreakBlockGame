using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeButton : MonoBehaviour
{
    private UILabel uILabel = null;     // 라벨

    private int mIndex = 0;             // 자신의 자식 인덱스

    private NodeLine nodeLine = null;   // 부모의 NodeLine컴포넌트
       
    private Node childNode = null;           // 자신이 갖고 있는 노드. 맵 데이터가 관리해 주기 때문에 절대 건드리면 안된다.

    public UILabel UILabel { get => uILabel; set => uILabel = value; }
    public int MIndex { get => mIndex; set => mIndex = value; }
    public Node Node { get => childNode; set => childNode = value; }



    // Start is called before the first frame update
    void Start()
    {
        nodeLine = GetComponentInParent<NodeLine>();

        uILabel = GetComponentInChildren<UILabel>();
    }

    // Update is called once per frame
    void Update()
    {
        UILabel.text = nodeLine.LineStatus.lineNumber.ToString();
    }

    public void OnClickButton()
    {
        childNode = MapDataManager.Instance.CreateNode(nodeLine.LineStatus.lineNumber, mIndex, transform);
    }
}
