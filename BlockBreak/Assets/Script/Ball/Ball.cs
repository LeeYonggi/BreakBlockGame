using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    public enum BALL_STATE
    {
        BALL_STOP,
        BALL_MOVE,
        BALL_FOLLOW
    }
    private float m_MoveSpeed = 0.0f;                       // 이동속도
    
    private BALL_STATE ballState = BALL_STATE.BALL_STOP;    // 현재 공 상태


    #region Component
    private Rigidbody2D rb2D = null;
    
    private CircleCollider2D circleCollider2D = null;

    private PhysicsBounceObject bounceComponent = null;
    #endregion

    #region Property
    public BALL_STATE BallState { get => ballState; set => ballState = value; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        circleCollider2D = GetComponent<CircleCollider2D>();

        bounceComponent = GetComponent<PhysicsBounceObject>();

        bounceComponent.OnBounce += BounceCollision;
    }

    // Update is called once per frame
    public void FixedUpdateMe()
    {
        switch (BallState)
        {
            case BALL_STATE.BALL_STOP:
                bounceComponent.ResetPrevBounceObj();

                break;
            case BALL_STATE.BALL_MOVE:
                bounceComponent.FixedUpdateMe();

                #region 무한 반복 방지 |현재 사용하지 않음|
                //if (rb2D.velocity.y <= 0.07f && rb2D.velocity.y >= -0.07f)
                //    rb2D.AddForce(new Vector2(0, -1));
                #endregion

                break;
            case BALL_STATE.BALL_FOLLOW:
                bounceComponent.ResetPrevBounceObj();

                //FollowFirstBall();

                break;
            default:
                break;
        }
    }

    public void BounceCollision(GameObject bounceObject, out bool isLoopBreak)
    {
        if (bounceObject.tag.Equals("Wall"))
        {
            isLoopBreak = true;
            return;
        }
        if (bounceObject.tag.Equals("Box"))
        { 
             bounceObject.GetComponent<Box>().BoxHit();
             isLoopBreak = true;
             return;
        }
        isLoopBreak = false;
    }

    /// <summary>
    /// 공 이동 함수. 한번만 불러주면 알아서 이동함.
    /// </summary>
    /// <param name="moveVector"></param>
    /// 방향벡터
    /// <param name="moveSpeed"></param>
    /// 이동속도
    public void BallMove(Vector2 moveVector, float moveSpeed)
    {
        bounceComponent.Velocity = moveVector * moveSpeed;

        BallState = BALL_STATE.BALL_MOVE;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        #region IsTrigger을 쓰지 않을 때 사용
        //if (collision.gameObject.tag.Equals("Floor"))
        //    CollisionFloor(collision.gameObject);

        //if (collision.gameObject.tag.Equals("Wall"))
        //{
        //    ChangeBallDirection(collision.contacts[0].normal);
        //}

        //if (collision.gameObject.tag.Equals("Box"))
        //{
        //    ChangeBallDirection(collision.contacts[0].normal);
        //}
        #endregion
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 바닥에 닿았을 시
        if (collision.gameObject.tag.Equals("Floor"))
            CollisionFloor(collision.gameObject);

        if (collision.gameObject.tag.Equals("Item"))
        {
            InGameManager.Instance.AddBallCount();
            Destroy(collision.gameObject);
        }
    }

    private void CollisionFloor(GameObject collision)
    {
        if (BallManager.Instance.FirstBall == null)
        {
            BallManager.Instance.FirstBall = gameObject;

            BallState = BALL_STATE.BALL_STOP;

            return;
        }

        FollowBallInit();
    }

    public void FollowBallInit()
    {
        BallState = BALL_STATE.BALL_FOLLOW;

        Vector2 firstBallPos = BallManager.Instance.FirstBallPosition;

        if (BallManager.Instance.FirstBall)
            firstBallPos = BallManager.Instance.FirstBall.transform.position;

        transform.DOMove(firstBallPos, 0.4f).SetEase(Ease.InCirc);

        StartCoroutine(FollowTimeCoroutine(0.7f));
    }

    IEnumerator FollowTimeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        BallState = BALL_STATE.BALL_STOP;
    }

    /// <summary>
    /// 첫번째로 바닥에 닿은 공을 따라갑니다.
    /// </summary>
    private void FollowFirstBall()
    {
        Vector2 firstBallPos = BallManager.Instance.FirstBallPosition;

        if (BallManager.Instance.FirstBall)
            firstBallPos = BallManager.Instance.FirstBall.transform.position;

        if (Vector2.Distance(transform.position, firstBallPos) < 0.2f)
        {
            transform.position = firstBallPos;

        }
    }
}
