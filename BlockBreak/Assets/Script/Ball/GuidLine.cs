using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidLine : MonoBehaviour
{
    [SerializeField]
    private GameObject spriteMask = null;
    [SerializeField]
    private GameObject guidSprite = null;
    [SerializeField]
    private GameObject predictBall = null;

    private List<PredictBall> predictBalls = new List<PredictBall>();

    private SpriteRenderer spRenderer = null;  


    // Start is called before the first frame update
    void Start()
    {
        spRenderer = guidSprite.GetComponent<SpriteRenderer>();

        for(int i = 0; i < 1; i++)
            predictBalls.Add(GameObject.Instantiate(predictBall).GetComponent<PredictBall>());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (BallManager.Instance.FirstBall == null) return;

        predictBalls[0].gameObject.SetActive(true);
        predictBalls[0].StartPrediction();

        transform.position = BallManager.Instance.FirstBall.transform.position;

        Vector2 predictBallPos = predictBalls[0].transform.position;

        float predictDistance = Vector2.Distance(predictBallPos, transform.position);

        float spriteDistance = spRenderer.bounds.size.y;

        spriteMask.transform.localScale = new Vector2(1, predictDistance / spriteDistance);

        float angle = Mathf.Atan2(transform.position.y - predictBalls[0].transform.position.y,
                                  transform.position.x - predictBalls[0].transform.position.x) * Mathf.Rad2Deg + 90;

        Vector3 euler = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.Euler(euler);
    }

    private void OnDisable()
    {
        for (int i = 0; i < predictBalls.Count; i++)
            predictBalls[i].gameObject.SetActive(false);
    }
}
