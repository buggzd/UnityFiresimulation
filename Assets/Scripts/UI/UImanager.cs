using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    List<Toggle> LisToggle;
    public GameObject root;
    // Start is called before the first frame update
    void Start()
    {
        LisToggle = new List<Toggle>();
        Toggle[] temp=root.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in temp)
        {
            LisToggle.Add(toggle);
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
