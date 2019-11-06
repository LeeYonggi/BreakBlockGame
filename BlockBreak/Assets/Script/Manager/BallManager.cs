using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallManager : MonoBehaviour
{
    enum KEY_STATE
    {
        NONE,
        KEY_PRESS,
        KEY_UP
    }

    enum FIRE_STATE
    {
        IDLE,
        FIRE_ACTIVATION
    }

    private static BallManager instance = null;

    /// <summary>
    /// 클릭 시 마우스 좌표
    /// </summary>
    private Vector2 firstPosition = new Vector2(0, 0);  

    /// <summary>
    /// 손 땠을 시 마우스 좌표
    /// </summary>
    private Vector2 secondPosition = new Vector2(0, 0); 

    private float shootDelay = 0.05f;                   // 공 발사 딜레이
    private float shootDelayCount = 0.0f;               // 현재 공 발사 딜레이
    private float shootSpeed = 6.0f;                    // 공 스피드
    private Vector2 ballMoveVector = new Vector2(0, 0); // 공 방향벡터

    [SerializeField]
    private float limitDegree = 20.0f;                  // 발사 각도 제한

    /// <summary>
    /// 현재 리스트에서 발사되는 공 인덱스
    /// </summary>
    private int nowShootindex = 0;                      

    private KEY_STATE m_KeyState = KEY_STATE.NONE;      // 모바일 마우스 상태
    private KEY_STATE m_PcKeyState = KEY_STATE.NONE;    // pc 마우스 상태
    private FIRE_STATE m_FireState = FIRE_STATE.IDLE;   // 볼 발사 상태

    private List<Ball> ballPacks = new List<Ball>();    // 볼 관리 리스트

    /// <summary>
    /// 첫번째 공 좌표
    /// </summary>
    private readonly Vector2 firstBallPosition = new Vector2(0, -2.8f);

    public GameObject firstBall = null;                 // 첫번째 공

    [SerializeField]
    private GameObject predictBall = null;              // 예측하는 공

    [SerializeField]
    private GameObject ballPrefab = null;               // 공 프리펩

    [SerializeField]
    private GameObject arrow = null;                    // 화살표 오브젝트


    #region Property
    public static BallManager Instance { get => instance; set => instance = value; }
    public GameObject FirstBall { get => firstBall; set => firstBall = value; }
    public Vector2 FirstPosition { get => firstPosition; set => firstPosition = value; }
    public Vector2 SecondPosition { get => secondPosition; set => secondPosition = value; }
    #endregion

    public delegate void ActiveControll();
    
    // 마우스 땠을 시 호출하는 이벤트
    public event ActiveControll OnMouseUp;

    // 공 생성
    GameObject CreateBall()
    {
        GameObject ball = GameObject.Instantiate(ballPrefab, transform);

        ball.transform.position = firstBallPosition;

        ballPacks.Add(ball.GetComponent<Ball>());

        return ball;
    }

    // 턴이 끝나면 해주는 초기화, 주로 GameManager에서 사용
    public void StepInitialize()
    {
        FirstBall = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            GameObject.Destroy(gameObject);

        // 처음 공 생성
        for(int i = 0; i < 1; i++)
            CreateBall();

        FirstBall = ballPacks[0].gameObject;

        OnMouseUp += FireBallInitalize;
        OnMouseUp += DisableArrow;
        OnMouseUp += StepInitialize;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        switch (m_FireState)
        {
            case FIRE_STATE.IDLE:
                break;
            case FIRE_STATE.FIRE_ACTIVATION:
                MoveBallLoop();
                break;
            default:
                break;
        }
    }

    // 마우스 컨트롤
    public void MouseControll()
    {
        MobileTouch();
        PcTouch();
    }

    // 카메라 월드스크린 변환 함수
    Vector2 GetTouchPoint()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void ActiveArrowRotate(Vector2 v1, Vector2 v2)
    {
        if (IsMousePressPossible() == false) return;

        predictBall.SetActive(true);

        arrow.SetActive(true);

        Vector3 euler = new Vector3(0, 0, Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * Mathf.Rad2Deg - 90);
        arrow.transform.rotation = Quaternion.Euler(euler);

        arrow.transform.position = ballPacks[0].transform.position;
    }

    void DisableArrow()
    {
        arrow.SetActive(false);
        predictBall.SetActive(false);
    }

    bool IsMousePressPossible()
    {
        return (Vector2.Distance(firstPosition, secondPosition) > 0.2f);
    }

    // 모바일 마우스 좌표 함수
    void MobileTouch()
    {
        if (Input.touchCount > 0)
        {
            if (m_KeyState.Equals(KEY_STATE.NONE))
            {
                MouseDown();

                m_KeyState = KEY_STATE.KEY_PRESS;
            }
            else if (m_KeyState.Equals(KEY_STATE.KEY_PRESS))
            {
                MousePress();
            }
        }
        else if (m_KeyState.Equals(KEY_STATE.KEY_PRESS))
        {
            MouseUp(out m_KeyState);
        }
    }

    // pc 마우스 좌표 함수
    void PcTouch()
    {
        if (Input.GetMouseButtonDown(0) && m_PcKeyState.Equals(KEY_STATE.NONE))
        {
            MouseDown();

            m_PcKeyState = KEY_STATE.KEY_PRESS;
        }

        if (m_PcKeyState.Equals(KEY_STATE.KEY_PRESS))
        {
            MousePress();
        }

        if (Input.GetMouseButtonUp(0) &&
            m_PcKeyState.Equals(KEY_STATE.KEY_PRESS))
        {
            MouseUp(out m_PcKeyState);
        }
    }

    void MouseDown()
    {
        firstPosition = GetTouchPoint();
        secondPosition = GetTouchPoint();
    }

    void MousePress()
    {
        secondPosition = GetTouchPoint();

        Vector2 directionVector = secondPosition - firstPosition;
        directionVector.Normalize();
        float seta = Vector2.Dot(directionVector, new Vector2(1, 0));
        if (seta > Mathf.Cos(limitDegree * Mathf.Deg2Rad) || (directionVector.y < 0 && directionVector.x > 0))
        {
            secondPosition.x = firstPosition.x + Mathf.Cos(limitDegree * Mathf.Deg2Rad);
            secondPosition.y = firstPosition.y + Mathf.Sin(limitDegree * Mathf.Deg2Rad);
        }

        if(seta < Mathf.Cos((180 - limitDegree) * Mathf.Deg2Rad) || (directionVector.y < 0 && directionVector.x < 0))
        {
            secondPosition.x = firstPosition.x + Mathf.Cos((180 - limitDegree) * Mathf.Deg2Rad);
            secondPosition.y = firstPosition.y + Mathf.Sin((180 - limitDegree) * Mathf.Deg2Rad);
        }

        ActiveArrowRotate(firstPosition, secondPosition);
    }

    void MouseUp(out KEY_STATE keyState)
    {
        if (IsMousePressPossible())
        {
            OnMouseUp();

            keyState = KEY_STATE.NONE;

            firstPosition = new Vector2(0, 0);
        }
        else
        {
            keyState = KEY_STATE.NONE;

            firstPosition = new Vector2(0, 0);
        }
    }

    // 방향 백터를 구한 뒤, 공 발사 코르틴 루프를 돌려주는 함수.
    void FireBallInitalize()
    {
        ballMoveVector = secondPosition - firstPosition;

        ballMoveVector.Normalize();

        nowShootindex = 0;

        m_FireState = FIRE_STATE.FIRE_ACTIVATION;
    }

    // 공을 발사시켜 주는 루프
    void MoveBallLoop()
    {
        if(shootDelayCount <= 0.0f)
        {
            ballPacks[nowShootindex].BallMove(ballMoveVector, shootSpeed);

            nowShootindex += 1;

            if(nowShootindex >= ballPacks.Count)
            {
                shootDelayCount = 0.0f;

                m_FireState = FIRE_STATE.IDLE;
            }
            else
                shootDelayCount = shootDelay;
        }

        shootDelayCount -= Time.fixedDeltaTime;
    }

    // 모두 공이 멈추었는지 반환해주는 함수.
    public bool IsBallAllStop()
    {
        if (m_FireState.Equals(FIRE_STATE.FIRE_ACTIVATION)) return false;

        for(int i = 0; i < ballPacks.Count; i++)
        {
            if (ballPacks[i].BallState != Ball.BALL_STATE.BALL_STOP)
                return false;
        }
        return true;
    }
}
