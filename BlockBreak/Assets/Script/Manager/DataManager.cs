using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    private static DataManager instance = null;

    private FileInfo stageFileInfo = null;

    private string resourceMapPath = string.Empty;
    
    private int nowStage = 0;

    private bool isMapToolDirect = false;

    public static DataManager Instance { get => instance; set => instance = value; }
    public FileInfo StageFileInfo { get => stageFileInfo; set => stageFileInfo = value; }
    public int NowStage { get => nowStage; set => nowStage = value; }
    public string ResourceMapPath { get => resourceMapPath; set => resourceMapPath = value; }
    public bool IsMapToolDirect { get => isMapToolDirect; set => isMapToolDirect = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance)
            GameObject.Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
