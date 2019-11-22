using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    private GameSceneManager instance = null;

    public GameSceneManager Instance { get => instance; set => instance = value; }

    private List<Manager> managerList = new List<Manager>();

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance)
            GameObject.Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
