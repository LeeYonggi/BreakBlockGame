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

    [SerializeField]
    private List<ScreenRatioData> screenRatioList;

    private Dictionary<Vector2, float> screenRatioSize = new Dictionary<Vector2, float>();

    private Camera mCamera = null; 

    void DictionaryInit()
    {
        for(int i = 0; i < screenRatioList.Count; i++)
            screenRatioSize.Add(screenRatioList[i].screenRatio, screenRatioList[i].screenSize);
    }

    // Start is called before the first frame update
    void Start()
    {
        DictionaryInit();

        mCamera = GetComponent<Camera>();

        startPosition = transform.position;

        Vector2 screenRatio = new Vector2(Screen.width, Screen.height);

        //float size = screenRatioSize[screenRatio];

        if (screenRatioSize.TryGetValue(screenRatio, out float value))
            mCamera.orthographicSize = screenRatioSize[screenRatio];
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
