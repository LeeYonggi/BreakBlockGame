using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum BALL_STATE
    {
        BALL_STOP,
        BALL_MOVE,
        BALL_FOLLOW
    }
    private Vector2 m_MoveVector = new Vector2(0, 0);
    private float m_MoveSpeed = 0.0f;
    private BALL_STATE ballState = BALL_STATE.BALL_STOP;
    private Rigidbody2D rb2D = null;
    private CircleCollider2D circleCollider2D = null;
    private float collisionRadius = 0.0f;

    public BALL_STATE BallState { get => ballState; set => ballState = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        collisionRadius = circleCollider2D.radius;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        switch (BallState)
        {
            case BALL_STATE.BALL_STOP:

                break;
            case BALL_STATE.BALL_MOVE:
                BallMoveUpdate();
                // 가로로 무한히 반복 방지
                //if (rb2D.velocity.y <= 0.07f && rb2D.velocity.y >= -0.07f)
                //    rb2D.AddForce(new Vector2(0, -1));
                break;
            case BALL_STATE.BALL_FOLLOW:
                FollowFirstBall();

                break;
            default:
                break;
        }
    }
    private void BallMoveUpdate()
    {
        Vector2 rayDirection = m_MoveVector;

        RaycastHit2D[] hit = new RaycastHit2D[2];
        int count = rb2D.Cast(rayDirection, hit, collisionRadius);

        for(int i = 0; i < count; i++)
        {
            if (hit[i].collider.gameObject.tag.Equals("Wall"))
            {
                ChangeBallDirection(hit[i].normal);
                break;
            }

            if (hit[i].collider.gameObject.tag.Equals("Box"))
            {
                ChangeBallDirection(hit[i].normal);
                hit[i].collider.GetComponent<Box>().BoxHit();
                break;
            }
        }

        transform.Translate(m_MoveVector * m_MoveSpeed * Time.fixedDeltaTime);
    }

    public void BallMove(Vector2 moveVector, float moveSpeed)
    {
        m_MoveSpeed = moveSpeed;
        m_MoveVector = moveVector;

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
        if (collision.gameObject.tag.Equals("Floor"))
            CollisionFloor(collision.gameObject);
    }

    private void CollisionFloor(GameObject collision)
    {
        BallState = BALL_STATE.BALL_FOLLOW;
        m_MoveVector = new Vector2(0, 0);
        if (BallManager.Instance.FirstBall == null)
        {
            BallManager.Instance.FirstBall = gameObject;
            BallState = BALL_STATE.BALL_STOP;
        }
    }

    /// <summary>
    /// 첫번째로 바닥에 닿은 공을 따라갑니다.
    /// </summary>
    private void FollowFirstBall()
    {
        if(BallManager.Instance.FirstBall)
        {
            GameObject firstBall = BallManager.Instance.FirstBall;
            transform.position = Vector2.Lerp(transform.position, firstBall.transform.position, Time.deltaTime * 8);

            if(Vector2.Distance(transform.position, firstBall.transform.position) < 0.2f)
            {
                transform.position = firstBall.transform.position;

                BallState = BALL_STATE.BALL_STOP;
            }
        }
    }

    /// <summary>
    /// 공의 방향을 바꾸는 함수
    /// </summary>
    /// <param name="normalVector"></param>
    /// 닿은 타겟의 법선 벡터
    private void ChangeBallDirection(Vector2 normalVector)
    {
        // http://rapapa.net/?p=673 - 반사공식 사이트
        // V - 2 * N * (V dot N)    - 반사공식
        m_MoveVector = Vector2.Reflect(m_MoveVector, normalVector);
    }
}
