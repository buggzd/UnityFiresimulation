using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShaderSetting : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Camera>().SetReplacementShader(Shader.Find("Custom / fasheye"), "RenderType");
    }
}
