using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance.IsMapToolDirect)
            CreateMapToolStage();
        else
            CreateStageMap();

        DataManager.Instance.IsMapToolDirect = false;
    }

    private void CreateStageMap()
    {
        string stagePath = DataManager.Instance.ResourceMapPath;

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
        FileInfo fileInfo = DataManager.Instance.StageFileInfo;

        List<BoxStatus[]> mapData = StageParser.CreateTestStageFromFile(fileInfo);

        for (int y = 0; y < mapData.Count; y++)
        {
            for (int x = 0; x < mapData[y].Length; x++)
            {
                CreateMapAccordingState(mapData[y][x]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMapAccordingState(BoxStatus mapData)
    {
        if (mapData.objState == BoxStatus.OBJECT_STATE.BOX)
            GameManager.Instance.CreateBox(mapData);
        if(mapData.objState == BoxStatus.OBJECT_STATE.TRIANGLE)
            GameManager.Instance.CreateTriangle(mapData);
        if (mapData.objState == BoxStatus.OBJECT_STATE.ITEM)
            GameManager.Instance.CreateItem(mapData);
    }
}
