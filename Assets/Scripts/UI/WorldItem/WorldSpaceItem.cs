using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class WorldSpaceItem : MonoBehaviour
{
    // Start is called before the first frame update
    public  float MaxSize=1;
    [Range(0,1),Tooltip("参数百分比")]
    public float value;
    [Tooltip("标题")]
    public string title_context;
    [Tooltip("参数名")]
    public string content_context;

    private Text title;
    private Text content;
    private Text valueText;
    private Slider slider;
    void Start()
    {
        GameObject go = new GameObject();
        go = GameObject.Find("title");
        title = go.GetComponent<Text>();
        go=GameObject.Find("content");
        content = go.GetComponent<Text>();
   
        go = GameObject.Find("Slider");
        slider = go.GetComponent<Slider>();
        valueText=go.GetComponentInChildren<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        slider.value = value;
        title.text = title_context;
        content.text = content_context;
        valueText.text = value * MaxSize + "/" + MaxSize;
    }
}
