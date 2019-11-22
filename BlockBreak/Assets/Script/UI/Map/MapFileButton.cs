using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapFileButton : MonoBehaviour
{
    // 파일 정보
    private FileInfo fileInfo = null;

    private UILabel childUILabel = null;

    public FileInfo FileInfo { get => fileInfo; set => fileInfo = value; }


    // Start is called before the first frame update
    void Start()
    {
        childUILabel = GetComponentInChildren<UILabel>();

        childUILabel.text = fileInfo.Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickOpen()
    {
        MapDataManager.Instance.ChangeNowFile(fileInfo);
    }
}
