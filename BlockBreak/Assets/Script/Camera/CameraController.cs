using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 startPosition = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
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

            randomVec2.x = Random.Range(-scale, scale);
            randomVec2.y = Random.Range(-scale, scale);

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
