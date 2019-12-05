using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidLine : MonoBehaviour
{
    [SerializeField]
    private GameObject spriteMask = null;
    [SerializeField]
    private Sprite guidSprite = null;
    [SerializeField]
    private GameObject predictBallPrefab = null;
    [SerializeField]
    private GameObject guidLinePrefab = null;

    private PredictBall predictBall = null;

    private Vector2 guidDirection = new Vector2(0, 0);

    private float guidDistance = 0.0f;

    [SerializeField]
    private float maxLength = 1.2f;

    public float MaxLength { get => maxLength; set => maxLength = value; }
    public Vector2 GuidDirection { get => guidDirection; set => guidDirection = value; }

    private GuidLine nextGuidLine = null;


    // Start is called before the first frame update
    void Start()
    {
        guidDistance = guidSprite.bounds.size.y;

        predictBall = GameObject.Instantiate(predictBallPrefab).GetComponent<PredictBall>();

        GuidLineUpdate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        GuidLineUpdate();
    }

    private void GuidLineUpdate()
    {
        predictBall.gameObject.SetActive(true);

        Vector2 reflectVector = predictBall.StartPrediction(transform.position, guidDirection);

        Vector2 predictBallPos = predictBall.transform.position;

        spriteMask.transform.localScale = new Vector2(1, GetMaskDistance(predictBallPos, guidDistance));

        float angle = Mathf.Atan2(GuidDirection.y, GuidDirection.x) * Mathf.Rad2Deg - 90;

        Vector3 euler = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.Euler(euler);


        if (maxLength < 1.0f)
            predictBall.gameObject.SetActive(false);
        else
            ActiveNextGuidLine(predictBallPos, reflectVector);
    }

    private void ActiveNextGuidLine(Vector2 startPos, Vector2 direction)
    {
        if (direction.magnitude <= 0.0f) return;

        if (nextGuidLine == null)
        {
            nextGuidLine = GameObject.Instantiate(guidLinePrefab).GetComponent<GuidLine>();
            nextGuidLine.maxLength = maxLength - 1.0f;
        }

        nextGuidLine.GuidDirection = direction;
        nextGuidLine.transform.position = startPos;
    }

    float GetMaskDistance(Vector2 endPos, float spriteDistance)
    {
        float predictDistance = Vector2.Distance(endPos, transform.position) / spriteDistance;

        if (maxLength < 1.0f)
            return maxLength > predictDistance ? predictDistance : maxLength;

        return predictDistance;
    }

    private void OnDestroy()
    {
        if(predictBall)
            Destroy(predictBall.gameObject);

        if(nextGuidLine)
            Destroy(nextGuidLine.gameObject);
    }
}
