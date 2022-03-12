using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraPropert {
    GameObject _camera_obj;
    Camera _camera;
    string name;
}

public class CameraDisplay : MonoBehaviour
{
    public int cameraNum = 0;
    [SerializeField]
    public List<CameraPropert> cameraList = new List<CameraPropert>();
}
