using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PauseMain : MonoBehaviour
{

    public GameObject resume = null;
    public GameObject restart = null;
    public GameObject exit = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPause()
    {
        gameObject.SetActive(true);

        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        transform.DOScale(new Vector3(2, 2, 2), 0.7f).SetEase(Ease.OutBack);
    }

    public void OnClickResume()
    {
        transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.7f).SetEase(Ease.InBack);

        StartCoroutine(ActiveOffCoroutine(0.7f));
    }
    
    public void OnClickExit()
    {
        gameObject.SetActive(false);

        SceneManager.LoadScene(1);
    }

    IEnumerator ActiveOffCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }
}
