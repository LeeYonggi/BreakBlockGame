using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager instance = null;

    static public GameManager Instance { get => instance; set => instance = value; }
    public GAME_STEP GameStep { get => gameStep; set => gameStep = value; }

    [SerializeField]
    private GameObject boxPrefab = null;        // 박스 프리펩
    [SerializeField]
    private GameObject triangleBoxPrefab = null;// 삼각형 박스 프리펩

    int nowCount = 1;

    private static readonly int MAX_LINEBOX = 9;

    private List<GameObject> boxPacks = new List<GameObject>();

    private GameObject[] nowBoxLine = new GameObject[MAX_LINEBOX];

    public enum GAME_STEP
    {
        IDLE,
        PLAY
    }

    private GAME_STEP gameStep = GAME_STEP.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            GameObject.Destroy(gameObject);

        CreateBox();
        nowCount += 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStep)
        {
            case GAME_STEP.IDLE:
                BallManager.Instance.StepInitialize();
                BallManager.Instance.MouseControll();
                BallManager.Instance.OnMouseUp += ChangePlay;

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

        for (int i = 0; i < nowBoxLine.Length; i++)
            nowBoxLine[i] = null;

        int random = Random.Range(1, 7);

        for (int i = 0; i < random; i++)
            CreateBox();

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

        GameObject box = GameObject.Instantiate(boxPrefab, transform);

        box.transform.position = new Vector2(random * 0.6f - 2.45f, 3.15f);

        boxPacks.Add(box);

        box.GetComponent<Box>().BoxStatus = new BoxStatus(nowCount);

        nowBoxLine[random] = box;
    }

    public void DestroyBox(GameObject box)
    {
        boxPacks.Remove(box);
        Destroy(box);
    }
}
