using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MapFileMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject fileButtonPrefab = null;

    private UIGrid grid = null;

    private Dictionary<string, MapFileButton> fileButtonTable = new Dictionary<string, MapFileButton>();

    [SerializeField]
    private UILabel fileNameUI = null;

    public Dictionary<string, MapFileButton> FileButtonTable { get => fileButtonTable; set => fileButtonTable = value; }

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<UIGrid>();

        List<FileInfo> fileList = GetFileList();

        for(int i = 0; i < fileList.Count; i++)
        {
            CreateFileButton(fileList[i]);
        }

        SetFileNameLabel(MapDataManager.Instance.NowFileInfo.Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<FileInfo> GetFileList()
    {
        List<FileInfo> fileList = FTPParse.GetFTPFileList();

        return fileList;
    }

    /// <summary>
    /// 맵 버튼 제작
    /// </summary>
    /// <param name="fileInfo"></param>
    public void CreateFileButton(FileInfo fileInfo)
    {
        GameObject button = GameObject.Instantiate(fileButtonPrefab, transform);

        var fileButton = button.GetComponent<MapFileButton>();

        fileButton.FileInfo = fileInfo;

        fileButtonTable.Add(fileInfo.Name, fileButton);

        grid.enabled = true;
    }


    public void SetFileNameLabel(string text)
    {
        string fileName = text;
        fileNameUI.text = fileName.Substring(0, fileName.Length - 4);
    }
}
