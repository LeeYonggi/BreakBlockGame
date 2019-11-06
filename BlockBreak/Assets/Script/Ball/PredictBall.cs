using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictBall : MonoBehaviour
{
    private Rigidbody2D rb2D = null;
    private CircleCollider2D circleCollider2D = null;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BallManager.Instance.FirstBall == null) return;

        Vector2 firstPosition = BallManager.Instance.FirstBall.transform.position;
        transform.position = firstPosition;
        rb2D.position = transform.position;

        Vector2 direction = BallManager.Instance.SecondPosition - BallManager.Instance.FirstPosition;

        direction.Normalize();

        MakePrediction(direction);
    }


    void MakePrediction(Vector2 direction)
    {
        //Ray2D ray = new Ray2D(transform.position, new Vector2(0, 0));
        //RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction, 0);

        RaycastHit2D[] hit = new RaycastHit2D[10];
        int result = rb2D.Cast(direction, hit, circleCollider2D.radius);

        for (int i = 0; i < result; i++)
        {
            if (hit[i].collider.gameObject.tag == "Wall")
            {
                //Debug.Log("It's end line!");
                //Debug.Log(transform.position);
                return;
            }
        }

        MoveDirection(direction);
        MakePrediction(direction);
        return;
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
