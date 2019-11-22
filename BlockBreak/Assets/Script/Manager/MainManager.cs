using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private static MainManager instance = null;

    public static MainManager Instance { get => instance; set => instance = value; }

    [SerializeField]
    private Vector2 basicRatio = new Vector2(720, 1280);

    private void Awake()
    {
        float width = Screen.width;
        float height = Screen.height;

        float ratio = height / width;
        Screen.SetResolution((int)basicRatio.x, (int)(basicRatio.x * ratio), true);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
