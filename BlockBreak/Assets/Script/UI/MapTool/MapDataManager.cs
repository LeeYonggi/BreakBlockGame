using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapDataManager : MonoBehaviour
{
    private static MapDataManager instance = null;

    #region Property
    public static MapDataManager Instance { get => instance; set => instance = value; }
    public FileInfo NowFileInfo { get => nowFileInfo; set => nowFileInfo = value; }

    public List<Node[]> nodeDatas = new List<Node[]>();
    #endregion

    /// <summary>
    /// 맵 스크롤 관리 객체
    /// </summary>
    [SerializeField]
    private GameObject scrollView = null;

    [SerializeField]
    private GameObject nodePrefab = null;

    /// <summary>
    /// 파일 스크롤 및 파일 버튼들을 관리하고 있는 객체
    /// </summary>
    [SerializeField]
    private MapFileMaker fileMaker = null;

    [SerializeField]
    private UILabel countLabel = null;

    /// <summary>
    /// 현재 파일 정보
    /// </summary>
    private FileInfo nowFileInfo = null;

    public NodeData selectData = new NodeData(0, NodeData.NODE_STATE.NONE, NodeData.DIRECTION_STATE.DEGREE_0);

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            GameObject.Destroy(gameObject);

        nowFileInfo = new FileInfo("Empty.txt");
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            selectData.count = int.Parse(countLabel.text);
        }
        catch(Exception e) { }
    }

    /// <summary>
    /// 맵 전체 초기화
    /// </summary>
    public void InitializeMap(int mapSize)
    {
        for(int i = 0; i < nodeDatas.Count; i++)
        {
            for(int node = 0; node < nodeDatas[i].Length; node++)
            {
                if(nodeDatas[i][node])
                    Destroy(nodeDatas[i][node].gameObject);
            }
        }

        nodeDatas.Clear();

        scrollView.GetComponent<NodeLineMaker>().InitializeRangeLimit(mapSize);
    }

    /// <summary>
    /// 노드 리스트를 사이즈에 맞게 설정해 준다.
    /// </summary>
    /// <param name="nodeSize"></param>
    public void InitializeNodeData(int nodeSize)
    {
        int deltaSize = nodeSize - nodeDatas.Count;

        if (deltaSize > 0)
        {
            for (int i = 0; i < deltaSize; i++)
                nodeDatas.Add(new Node[9]);
        }
        else
        {
            for (int i = 0; i > deltaSize; i--)
                nodeDatas.RemoveAt(nodeDatas.Count - 1);
        }
    }

    /// <summary>
    /// 노드의 최대 줄 수 추가.
    /// </summary>
    /// <param name="lineCount"></param>
    public void ChangeLineObject(int lineCount)
    {
        scrollView.GetComponent<NodeLineMaker>().InitializeRangeLimit(lineCount);
    }

    public Node CreateNode(int line, int index, Transform parent)
    {
        string selectDataName = StateToSpriteName(selectData.state);

        // 상태가 없는 노드인지
        if (selectDataName == string.Empty)
        {
            // 상태가 없는 노드면 지우개로 간주하고 지운다.
            if (nodeDatas[line][index])
            {
                Destroy(nodeDatas[line][index].gameObject);
                nodeDatas[line][index] = null;
            }

            return null;
        }
        // 해당 칸에 노드가 있을 시 그 노드를 삭제해준다.
        else if (nodeDatas[line][index])
        {
            Destroy(nodeDatas[line][index].gameObject);
        }

        // 노드생성
        Node node = GameObject.Instantiate(nodePrefab, parent).GetComponent<Node>();

        node.NodeData = selectData;

        node.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (int)selectData.dirState));

        node.GetComponent<UISprite>().spriteName = selectDataName;

        nodeDatas[line][index] = node;

        return node;

        string StateToSpriteName(NodeData.NODE_STATE state)
        {
            switch (state)
            {
                case NodeData.NODE_STATE.NONE:
                    return string.Empty;
                case NodeData.NODE_STATE.NODE_BOX:
                    return "Box";
                case NodeData.NODE_STATE.NODE_TRIANGLE:
                    return "Box2";
                case NodeData.NODE_STATE.NODE_ITEM:
                    return "LevelUp_Center";
            }
            return string.Empty;
        }
    }

    public void OnChangeLine(UILabel text)
    {
        ChangeLineObject(int.Parse(text.text));
    }
    
    /// <summary>
    /// 현재 맵을 저장한다.
    /// </summary>
    public void OnClickSaveButton(UILabel label)
    {
        if (nowFileInfo != null)
        {
            MapDataParser.FTPMapDataSave(label.text + ".txt", nodeDatas);

            var isButtonActive = fileMaker.FileButtonTable.ContainsKey(label.text + ".txt");

            if(isButtonActive == false)
            {
                fileMaker.CreateFileButton(new FileInfo(label.text + ".txt"));
            }
        }
    }

    /// <summary>
    /// 현재 제작하고 있는 맵 파일을 다른파일로 교체한다.
    /// </summary>
    /// <param name="nextFileInfo"></param>
    /// 텍스트 파일 정보.
    public void ChangeNowFile(FileInfo nextFileInfo)
    {
        nowFileInfo = nextFileInfo;

        List<NodeData[]> nodeDataList = MapDataParser.GetFTPMapData(nextFileInfo.Name);

        InitializeMap(nodeDataList.Count);

        NodeInstantiate(nodeDataList);

        InputNodeParent();

        fileMaker.SetFileNameLabel(nextFileInfo.Name);
    }

    /// <summary>
    /// 노드 데이터가 있으면 실제 노드를 만들고 데이터를 넣어줍니다.
    /// </summary>
    /// <param name="nodeDataList"></param>
    void NodeInstantiate(List<NodeData[]> nodeDataList)
    {
        for (int line = 0; line < nodeDataList.Count; line++)
        {
            for (int index = 0; index < nodeDataList[line].Length; index++)
            {
                if (nodeDataList[line][index].state != NodeData.NODE_STATE.NONE)
                {
                    selectData = nodeDataList[line][index];

                    var node = CreateNode(line, index, null);

                    node.gameObject.SetActive(false);

                    selectData = new NodeData();
                }
            }
        }
    }

    /// <summary>
    /// 노드 버튼의 자식에 노드들을 연결시켜 줍니다.
    /// </summary>
    public void InputNodeParent()
    {
        var nodeLineMaker = scrollView.GetComponent<NodeLineMaker>();

        for (int i = 0; i < nodeLineMaker.transform.childCount; i++)
        {
            var nodeLine = nodeLineMaker.transform.GetChild(i);

            for (int nodeIndex = 0; nodeIndex < nodeLine.transform.childCount; nodeIndex++)
            {
                var node = nodeLine.transform.GetChild(nodeIndex);

                if (nodeDatas[i][nodeIndex] != null)
                {
                    NodeActive(i, node.GetComponent<NodeButton>());
                }
            }
        }
    }

    public void NodeActive(int lineIndex, NodeButton nodeButton)
    {
        // 노드 버튼에 있는 노드를 바꿔줌
        nodeButton.Node = nodeDatas[lineIndex][nodeButton.MIndex];

        if (nodeButton.Node)
        {
            nodeButton.Node.gameObject.SetActive(true);
            nodeButton.Node.transform.parent = nodeButton.gameObject.transform;
            nodeButton.Node.transform.localPosition = new Vector3(0, 0, 0);
            nodeButton.Node.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnClickGameStart()
    {
        DataManager.Instance.IsMapToolDirect = true;
        DataManager.Instance.NowStage = 1;
        DataManager.Instance.StageFileInfo = nowFileInfo;

        SceneManager.LoadScene(1);
    }
}