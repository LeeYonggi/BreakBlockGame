﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictBall : MonoBehaviour
{
    private Rigidbody2D rb2D = null;
    private CircleCollider2D circleCollider2D = null;


    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {

    }

    /// <summary>
    /// 공의 경로 예측을 시작합니다
    /// </summary>
    /// <param name="firstPosition"></param>
    /// 첫 좌표
    /// <param name="direction"></param>
    /// 예측할 방향
    /// <returns></returns>
    /// 반사 방향
    public Vector2 StartPrediction(Vector2 firstPosition, Vector2 direction)
    {
        transform.position = firstPosition;
        rb2D.position = transform.position;

        direction.Normalize();

        return MakePrediction(direction);
    }


    Vector2 MakePrediction(Vector2 direction)
    {
        RaycastHit2D[] hit = new RaycastHit2D[10];
        int result = rb2D.Cast(direction, hit, circleCollider2D.radius);

        for (int i = 0; i < result; i++)
        {
            if (hit[i].collider.gameObject.tag == "Wall")
            {
                GameObject wall = hit[i].collider.gameObject;

                BoxCollider2D collider = wall.GetComponent<BoxCollider2D>();

                transform.position = GetBoxColliderOutLine(hit[i].normal, collider.transform.position, collider);

                return PhysicsBounceObject.ChangeDirection(direction, hit[i].normal);
            }
            if (hit[i].collider.gameObject.tag.Equals("Floor"))
                return new Vector2(0, 0);
        }

        MoveDirection(direction);
        return MakePrediction(direction);
    }

    Vector2 GetBoxColliderOutLine(Vector2 colliderNormal, Vector2 wallPosition, BoxCollider2D collider)
    {
        wallPosition += collider.offset;

        Vector2 basicPosition = new Vector2(Mathf.Abs(colliderNormal.y), Mathf.Abs(colliderNormal.x)) * rb2D.transform.position;

        return colliderNormal * collider.bounds.size * 0.5f + wallPosition + basicPosition + colliderNormal * circleCollider2D.radius;
    }

    void MoveDirection(Vector2 direction)
    {
        Vector3 movePosition = new Vector3( 0, 0, 0 );

        movePosition.x = direction.x * circleCollider2D.radius * 2;
        movePosition.y = direction.y * circleCollider2D.radius * 2;

        transform.position = transform.position + movePosition;
        rb2D.position = transform.position;
    }

}
