using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance = null;

    private int nowStage = 0;

    public static DataManager Instance { get => instance; set => instance = value; }
    public int NowStage { get => nowStage; set => nowStage = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance)
            GameObject.Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
