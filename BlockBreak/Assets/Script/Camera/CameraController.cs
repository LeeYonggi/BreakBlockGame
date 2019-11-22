using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct ScreenRatioData
{
    public Vector2 screenRatio;
    public float screenSize;
}

public class CameraController : MonoBehaviour
{
    private Vector3 startPosition = new Vector3(0, 0, 0);

    private Camera mCamera = null;

    [SerializeField]
    private Vector2 pivotScreen = new Vector2(720, 1280);


    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        mCamera = GetComponent<Camera>();

        startPosition = transform.position;

        float pivotRatio = pivotScreen.y / pivotScreen.x;
        float screenRatio = Screen.height / Screen.width;

        float tempRatio = screenRatio - pivotRatio;

        if(tempRatio > 0.0f)
            mCamera.orthographicSize = (1.0f + tempRatio) * mCamera.orthographicSize;
    }                                                               

    // Update is called once per frame
    void Update()
    {
    }

    public void CameraShake(float scale, float endTime)
    {
        StartCoroutine(ShakeCoroutine(scale, endTime));
    }

    IEnumerator ShakeCoroutine(float scale, float endTime)
    {
        float startTime = Time.time;

        while(startTime > Time.time - endTime)
        {
            Vector3 randomVec2 = new Vector3(0, 0, 0);

            randomVec2.x = UnityEngine.Random.Range(-scale, scale);
            randomVec2.y = UnityEngine.Random.Range(-scale, scale);

            transform.Translate(randomVec2);

            yield return new WaitForSeconds(0.05f);
        }

        while(Vector2.Distance(startPosition, transform.position) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime);

            yield return new WaitForSeconds(0.01f);
        }

        transform.position = startPosition;
    }
}
