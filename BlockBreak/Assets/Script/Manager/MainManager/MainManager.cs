using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private static MainManager instance = null;

    #region Property
    public static MainManager Instance { get => instance; set => instance = value; }
    public BallSettingData BallSettingData { get => ballSettingData; set => ballSettingData = value; }
    public StageData StageInfo { get => stageInfo; set => stageInfo = value; }
    #endregion

    [SerializeField]
    private Vector2 basicRatio = new Vector2(720, 1280);

    [SerializeField]
    private BallSettingData ballSettingData = new BallSettingData(0.05f, 6.0f, 20.0f, 50);

    private StageData stageInfo = new StageData();

    private Manager.GameSceneManager sceneManager = null;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        ScreenSetting();

        ManagerAwake();
    }

    private void ScreenSetting()
    {
        float width = Screen.width;
        float height = Screen.height;

        float ratio = height / width;
        Screen.SetResolution((int)basicRatio.x, (int)(basicRatio.x * ratio), true);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ManagerUpdate();
    }

    private void FixedUpdate()
    {
        ManagerFixedUpdate();
    }

    #region Manager management
    private void ManagerAwake()
    {
        sceneManager = Manager.GameSceneManager.Instance;
    }


    private void ManagerUpdate()
    {
        sceneManager.Update();
    }

    private void ManagerFixedUpdate()
    {
        sceneManager.FixedUpdate();
    }
    #endregion

}
