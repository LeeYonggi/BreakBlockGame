using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Manager;

public class StageData
{
    public FileInfo stageFileInfo = null;               // 스테이지 파일 정보
    
    public string resourceMapPath = string.Empty;       // 스테이지 파일 경로
    
    public int nowStage = 1;                            // 현재 스테이지
    
    public bool isMapToolDirect = false;                // 맵툴 정보를 불러 왔는지

    public StageData(FileInfo _stageFileInfo, string _path, int _stage, bool _isMapToolDirect)
    {
        stageFileInfo = _stageFileInfo;
        resourceMapPath = _path;
        nowStage = _stage;
        isMapToolDirect = _isMapToolDirect;
    }

    public StageData()
    {

    }
}

public class StageManager : Singleton<StageManager>, BaseManager
{
    public void Awake()
    {

    }

    public void Start()
    {
        if (MainManager.Instance.StageInfo.isMapToolDirect)
            CreateMapToolStage();
        else
            CreateStageMap();

        MainManager.Instance.StageInfo.isMapToolDirect = false;
    }

    private void CreateStageMap()
    {
        string stagePath = MainManager.Instance.StageInfo.resourceMapPath;

        BoxStatus[,] mapData = StageParser.CreateStageGridFromFile(stagePath);

        for(int y = 0; y < StageParser.MapSize.Y; y++)
        {
            for(int x = 0; x < StageParser.MapSize.X; x++)
            {
                CreateMapAccordingState(mapData[y, x]);
            }
        }
    }

    private void CreateMapToolStage()
    {
        FileInfo fileInfo = MainManager.Instance.StageInfo.stageFileInfo;

        List<BoxStatus[]> mapData = StageParser.CreateTestStageFromFile(fileInfo);

        for (int y = 0; y < mapData.Count; y++)
        {
            for (int x = 0; x < mapData[y].Length; x++)
            {
                CreateMapAccordingState(mapData[y][x]);
            }
        }
    }

    void CreateMapAccordingState(BoxStatus mapData)
    {
        if (mapData.objState == BoxStatus.OBJECT_STATE.BOX)
            InGameManager.Instance.CreateBox(mapData);
        if(mapData.objState == BoxStatus.OBJECT_STATE.TRIANGLE)
            InGameManager.Instance.CreateTriangle(mapData);
        if (mapData.objState == BoxStatus.OBJECT_STATE.ITEM)
            InGameManager.Instance.CreateItem(mapData);
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {

    }

    public void Destroy()
    {
        instance = null;
    }

}
