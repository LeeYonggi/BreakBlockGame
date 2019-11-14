using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBounceObject : MonoBehaviour
{
    /// <summary>
    /// 움직일 방향과 속도를 곱한 값
    /// </summary>
    private Vector2 velocity;             
    /// <summary>
    /// 물체 충돌 필터
    /// </summary>
    private ContactFilter2D contactFilter;
    private Rigidbody2D rb2D = null;
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

    private const float minMoveDistance = 0.001f;
    private const float shellRadius = 0.01f;

    public Vector2 Velocity { get => velocity; set => velocity = value; }

    public event OnBounceCollision OnBounce;

    private GameObject preBounceObject = null;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 deltaPosition = velocity * Time.deltaTime;

        deltaPosition += velocity * delayDistance * Time.deltaTime;
        delayDistance = 0;

        Movement(deltaPosition);
    }

    /// <summary>
    /// 물체를 움직이는 함수. 튕기는 물리처리를 해준다.
    /// </summary>
    /// <param name="move"></param>
    /// 움직일 방향
    private void Movement(Vector2 move)
    {
        // move 벡터의 크기를 distance에 저장
        float distance = move.magnitude;

        if(distance > minMoveDistance)
        {
            // Rigidbody2D cast로 충돌 물체를 받아옴
            int count = rb2D.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            bool isReflectEnd = false;

            for(int i = 0; i < count; i++)
            {
                // 이전에 충돌한 물체와 또다시 충돌 시 예외처리
                if (hitBuffer[i].collider.gameObject.Equals(preBounceObject))
                    continue;

                if (isReflectEnd == false)
                {
                    Vector2 currentNormal = hitBuffer[i].normal;

                    // 반사 처리
                    velocity = ChangeDirection(velocity, hitBuffer[i].normal);

                    // distance 보정
                    float modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = GetModifiedDistance(distance, modifiedDistance);

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
    private void MoveRbPosition(Vector2 direction, float speed)
    {
        rb2D.position = rb2D.position + direction * speed;
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
        // http://rapapa.net/?p=673 - 반사공식 사이트
        // V - 2 * N * (V dot N)    - 반사공식
        float dot = Vector2.Dot(direction, normalVector);

        return direction - 2 * normalVector * dot;
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
        if(modifiedDistance < basicDistance)
        {
            delayDistance += basicDistance - modifiedDistance;
            return modifiedDistance;
        }
        delayDistance = 0;
        return basicDistance;
    }

    private void OnEnable()
    {
        preBounceObject = null;
    }
}
