using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager instance = null;

    static public GameManager Instance { get => instance; set => instance = value; }
    public GAME_STEP GameStep { get => gameStep; set => gameStep = value; }
    public TIME_STATE TimeState { get => timeState; set => timeState = value; }

    [SerializeField]
    private GameObject boxPrefab = null;        // 박스 프리펩
    [SerializeField]
    private GameObject triangleBoxPrefab = null;// 삼각형 박스 프리펩
    [SerializeField]
    private GameObject levelUpItem = null;      // 레벨업 아이템

    [SerializeField]
    int nowCount = 20;                           // 박스 hp

    public enum TIME_STATE
    {
        ATTACH_NONE,
        ATTACH_DOUBLE,
        ATTACH_QUADRUPLE
    }

    private TIME_STATE timeState = TIME_STATE.ATTACH_NONE;

    private static readonly int MAX_LINEBOX = 10;

    private List<GameObject> boxPacks = new List<GameObject>();

    private List<GameObject> itemPacks = new List<GameObject>();

    private GameObject[] nowBoxLine = new GameObject[MAX_LINEBOX];

    private int nextAddBall = 0;

    public enum GAME_STEP
    {
        IDLE,
        PLAY
    }

    private GAME_STEP gameStep = GAME_STEP.IDLE;

    private void Awake()
    {
        GameObject[] objectBuffer = GameObject.FindGameObjectsWithTag("Box");

        for (int i = 0; i < objectBuffer.Length; i++)
            boxPacks.Add(objectBuffer[i]);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            GameObject.Destroy(gameObject);

        nowCount += 1;

        BallManager.Instance.OnMouseUp += ChangePlay;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStep)
        {
            case GAME_STEP.IDLE:
                BallManager.Instance.MouseControll();

                break;
            case GAME_STEP.PLAY:
                if (BallManager.Instance.IsBallAllStop())
                    ChangeNextLevel();
                break;
            default:
                break;
        }
    }

    // 턴 종료. 다음 턴 준비.
    void ChangeNextLevel()
    {
        for (int i = 0; i < boxPacks.Count; i++)
            boxPacks[i].transform.Translate(new Vector2(0, -0.6f));

        for (int i = 0; i < itemPacks.Count; i++)
            itemPacks[i].transform.Translate(new Vector2(0, -0.6f));

        for (int i = 0; i < nowBoxLine.Length; i++)
            nowBoxLine[i] = null;

        for (int i = 0; i < nextAddBall; i++)
            BallManager.Instance.AddBall();

        nextAddBall = 0;

        int random = Random.Range(1, 7);

        for (int i = 0; i < random; i++)
            CreateBox();

        CreateItem();

        GameStep = GAME_STEP.IDLE;

        nowCount += 1;
    }

    void ChangePlay()
    {
        GameStep = GAME_STEP.PLAY;
    }

    void CreateBox()
    {
        int random = Random.Range(1, MAX_LINEBOX);

        if (nowBoxLine[random] != null)
        {
            CreateBox();
            return;
        }

        int boxRandom = Random.Range(1, 10);
        GameObject box = null;

        if (boxRandom < 10)
            box = GameObject.Instantiate(boxPrefab, transform);
        else
            box = GameObject.Instantiate(triangleBoxPrefab, transform);

        box.transform.position = new Vector2(random * 0.6f - 2.4f, 3.3f);

        boxPacks.Add(box);

        box.GetComponent<Box>().BoxStatus = new BoxStatus(nowCount);

        nowBoxLine[random] = box;
    }

    void CreateItem()
    {
        int random = Random.Range(1, MAX_LINEBOX);

        if (nowBoxLine[random] != null)
        {
            CreateItem();
            return;
        }

        GameObject item = GameObject.Instantiate(levelUpItem);

        item.transform.position = new Vector2(random * 0.6f - 2.4f, 3.3f);

        itemPacks.Add(item);

        nowBoxLine[random] = item;
    }

    public void DestroyBox(GameObject box)
    {
        boxPacks.Remove(box);
        Destroy(box);
    }

    public void DestroyItem(GameObject item)
    {
        itemPacks.Remove(item);
    }

    public void AddBallCount()
    {
        nextAddBall += 1;
    }

    public void GameSpeedUp()
    {
        Time.timeScale = ChangeTimeState();
    }

    private float ChangeTimeState()
    {
        if(timeState.Equals(TIME_STATE.ATTACH_NONE))
        {
            timeState = TIME_STATE.ATTACH_DOUBLE;
            return 2.0f;
        }
        if (timeState.Equals(TIME_STATE.ATTACH_DOUBLE))
        {
            timeState = TIME_STATE.ATTACH_QUADRUPLE;
            return 4.0f;
        }
        if (timeState.Equals(TIME_STATE.ATTACH_QUADRUPLE))
        {
            timeState = TIME_STATE.ATTACH_NONE;
            return 1.0f;
        }
        return 1.0f;
    }
}
