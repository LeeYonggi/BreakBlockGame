using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateStageMap();
    }

    private void CreateStageMap()
    {
        string stagePath = Application.dataPath + $"/MapFile/map2/mapdata{DataManager.Instance.NowStage}.txt";

        MapData[,] mapData = StageParser.CreateStageGridFromFile(stagePath);

        for(int y = 0; y < StageParser.MapSize.Y; y++)
        {
            for(int x = 0; x < StageParser.MapSize.X; x++)
            {
                if(mapData[y, x].objState != MapData.OBJECT_STATE.NONE)
                    GameManager.Instance.CreateBox(mapData[y,x]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
