using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PauseMain : MonoBehaviour
{

    public GameObject resume = null;
    public GameObject restart = null;
    private GameObject exit = null;

    public GameObject Exit { get => exit; set => exit = value; }

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

        Vector3 nowScale = transform.localScale;

        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        transform.DOScale(nowScale, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);

        Time.timeScale = 0.0f;
    }

    public void OnClickResume()
    {
        Vector3 nowScale = transform.localScale;

        transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.InBack).OnComplete( 
            () => transform.localScale = nowScale
            ); 

        StartCoroutine(ActiveOffCoroutine(0.5f));

        Time.timeScale = InGameManager.Instance.GetStateToTime(InGameManager.Instance.TimeState);
    }
    
    public void OnClickExit()
    {
        gameObject.SetActive(false);

        Time.timeScale = InGameManager.Instance.GetStateToTime(InGameManager.Instance.TimeState);

        Manager.SceneManager.Instance.ChangeScene(0);
    }

    public void OnRePlayButton()
    {
        gameObject.SetActive(false);

        Time.timeScale = InGameManager.Instance.GetStateToTime(InGameManager.Instance.TimeState);

        Manager.SceneManager.Instance.ChangeScene(1);
    }

    IEnumerator ActiveOffCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }
}
