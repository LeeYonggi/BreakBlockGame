using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    public enum BALL_STATE
    {
        BALL_IDLE,
        BALL_STOP,
        BALL_MOVE,
        BALL_FOLLOW
    }
    private float m_MoveSpeed = 0.0f;                       // 이동속도
    
    private BALL_STATE ballState = BALL_STATE.BALL_STOP;    // 현재 공 상태


    #region Component
    private Rigidbody2D rb2D = null;
    
    private CircleCollider2D circleCollider2D = null;
    #endregion

    #region Property
    public BALL_STATE BallState { get => ballState; set => ballState = value; }
    public CircleCollider2D CircleCollider2D { get => circleCollider2D; set => circleCollider2D = value; }
    public Rigidbody2D Rb2D { get => rb2D; set => rb2D = value; }
    #endregion

    #region Bounce Relation
    /// <summary>
    /// 움직일 방향과 속도를 곱한 값
    /// </summary>
    private Vector2 velocity;
    /// <summary>
    /// 물체 충돌 필터
    /// </summary>
    private ContactFilter2D contactFilter;
    private RaycastHit2D[] hitBuffer = new RaycastHit2D[10];
    private float delayDistance = 0.0f;

    /// <summary>
    /// 공이 튕길 때 불러주는 함수
    /// </summary>
    /// <param name="collision"></param>
    /// 공이 튕길 때 맞은 물체
    /// <param name="isLoopBreak"></param>
    /// 물체에 튕긴 뒤 해당 프레임에서 한번 더 튕겨도 되는지 true: 안된다. false: 된다
    public delegate void OnBounceCollision(GameObject collision, out bool isLoopBreak);

    public event OnBounceCollision OnBounce;

    private const float minMoveDistance = 0.001f;
    private const float shellRadius = 0.01f;

    private GameObject preBounceObject = null;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        CircleCollider2D = GetComponent<CircleCollider2D>();

        OnBounce += BounceCollision;

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;

        m_MoveSpeed = circleCollider2D.radius * 2.0f * 50.0f;
    }

    // Update is called once per frame
    public void FixedUpdateMe()
    {
        switch (BallState)
        {
            case BALL_STATE.BALL_IDLE:
                ResetPrevBounceObj();

                break;
            case BALL_STATE.BALL_STOP:
                ResetPrevBounceObj();

                break;
            case BALL_STATE.BALL_MOVE:
                BallMovement();

                #region 무한 반복 방지 |현재 사용하지 않음|
                //if (rb2D.velocity.y <= 0.07f && rb2D.velocity.y >= -0.07f)
                //    rb2D.AddForce(new Vector2(0, -1));
                #endregion

                break;
            case BALL_STATE.BALL_FOLLOW:
                ResetPrevBounceObj();

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
    public void FireBall(Vector2 moveVector) // 함수 이름 변경
    {
        velocity = moveVector * m_MoveSpeed;

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
        if (BallManager.Instance.IsFirstTouch == false)
        {
            BallManager.Instance.FirstBallPosition = gameObject.transform.position;

            BallState = BALL_STATE.BALL_STOP;

            BallManager.Instance.IsFirstTouch = true;

            return;
        }

        FollowBallInit();
    }

    public void FollowBallInit()
    {
        BallState = BALL_STATE.BALL_FOLLOW;

        Vector2 firstBallPos = BallManager.Instance.FirstBallPosition;

        transform.DOMove(firstBallPos, 0.4f).SetEase(Ease.InCirc);

        StartCoroutine(FollowTimeCoroutine(0.7f));
    }

    IEnumerator FollowTimeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        BallState = BALL_STATE.BALL_STOP;
    }

    public void BallMovement()              // 함수 이름이 작동과 맞지 않음
    {
        Vector2 deltaPosition = velocity * Time.fixedDeltaTime;

        deltaPosition += velocity.normalized * delayDistance;
        delayDistance = 0;

        BounceUpdate(deltaPosition);
    }

    /// <summary>
    /// 물체를 움직이는 함수. 튕기는 물리처리를 해준다.
    /// </summary>
    /// <param name="move"></param>
    /// 움직일 방향
    private void BounceUpdate(Vector2 move)     // 함수 이름이 작동과 맞지 않음
    {
        // move 벡터의 크기를 distance에 저장
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            // Rigidbody2D cast로 충돌 물체를 받아옴
            int count = rb2D.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            bool isReflectEnd = false;

            for (int i = 0; i < count; i++)
            {
                // 이전에 충돌한 물체와 또다시 충돌 시 예외처리
                if (hitBuffer[i].collider.gameObject.Equals(preBounceObject))
                    continue;

                if (isReflectEnd == false)
                {
                    Vector2 currentNormal = hitBuffer[i].normal;

                    // distance 보정
                    float modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = GetModifiedDistance(distance, modifiedDistance);

                    // 반사 처리
                    // 해당 블록간의 거리가 0보다 작을 경우 반사될 시 문제가 발생한다.
                    if (distance > 0.0f)
                        velocity = ChangeDirection(velocity, currentNormal);

                    preBounceObject = hitBuffer[i].collider.gameObject;
                }

                // 튕기는 이벤트 처리
                OnBounce(hitBuffer[i].collider.gameObject, out bool isLoopBreak);

                if (isLoopBreak)
                    isReflectEnd = isLoopBreak;
            }
        }
        // 이동
        MoveRbPosition(move.normalized, distance);
    }


    /// <summary>
    /// 리지드바디 포지션을 움직여주는 함수.
    /// </summary>
    /// <param name="direction"></param>
    /// 움직일 방향
    /// <param name="speed"></param>
    /// 움직이는 속도
    private void MoveRbPosition(Vector2 direction, float distance)
    {
        rb2D.position = rb2D.position + direction * distance;
    }

    /// <summary>
    /// 오브젝트의 velocity를 닿은 면에 반사시켜주는 함수
    /// </summary>
    /// <param name="direction"></param>
    /// 방향
    /// <param name="normalVector"></param>
    /// 닿은 타겟의 법선 벡터
    public static Vector2 ChangeDirection(Vector2 direction, Vector2 normalVector)
    {
        //if (ReflexException(direction, normalVector))
        //    return direction;

        // http://rapapa.net/?p=673 - 반사공식 사이트
        // V - 2 * N * (V dot N)    - 반사공식
        float dot = Vector2.Dot(direction, normalVector);

        return direction - 2 * normalVector * dot;
    }

    /// <summary>
    /// 반사가 되면 안되는 상황에서 반사가 되었을시 true 반환
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="normalVector"></param>
    /// <returns></returns>
    static bool ReflexException(Vector2 direction, Vector2 normalVector)
    {
        if (direction.x < 0 && normalVector.x < 0)
            return true;
        if (direction.y < 0 && normalVector.y < 0)
            return true;
        if (direction.x > 0 && normalVector.x > 0)
            return true;
        if (direction.y > 0 && normalVector.y > 0)
            return true;

        return false;
    }

    /// <summary>
    /// 보정한 거리를 반환
    /// </summary>
    /// <param name="basicDistance"></param>
    /// 원래 거리
    /// <param name="modifiedDistance"></param>
    /// 보정할 거리
    /// <returns></returns>
    private float GetModifiedDistance(float basicDistance, float modifiedDistance)
    {
        if (modifiedDistance < basicDistance)
        {
            delayDistance += basicDistance - modifiedDistance;
            return modifiedDistance;
        }
        return basicDistance;
    }

    public void ResetPrevBounceObj()
    {
        preBounceObject = null;
    }
}
