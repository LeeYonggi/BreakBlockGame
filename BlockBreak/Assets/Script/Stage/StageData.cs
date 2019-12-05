using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Manager;
using System;

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