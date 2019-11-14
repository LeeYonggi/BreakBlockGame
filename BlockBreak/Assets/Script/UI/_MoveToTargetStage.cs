using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _MoveToTargetStage : MonoBehaviour
{
    private Text mText = null;

    public StageUIWarpContent content = null;

    // Start is called before the first frame update
    void Start()
    {
        mText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        content.MoveToStage(int.Parse(mText.text));
    }
}
