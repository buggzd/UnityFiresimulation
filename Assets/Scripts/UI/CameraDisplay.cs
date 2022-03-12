using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public struct CameraPropert {
    public GameObject _camera_obj;
    public Camera _camera;
    public string name;
}

public class CameraDisplay : MonoBehaviour
{
    public int cameraNum = 0;
    [SerializeField]
    public List<CameraPropert> cameraList = new List<CameraPropert>();
    public GameObject ImagePrefab;
    public Transform ImageTran;
    public List<RawImage> imageList = new List<RawImage>();

    private void Start()
    {
        for (int i = 0; i < cameraList.Count; i++)
        {
            CameraPropert temp = cameraList[i];
            temp._camera = cameraList[i]._camera_obj.GetComponent<Camera>();
            cameraList[i] = temp;
            GameObject tempImage = GameObject.Instantiate(ImagePrefab, ImageTran);
            tempImage.GetComponent<Button>().onClick.AddListener(()=> { this.GetComponent<CameraDisplay>().turnView(); });
            imageList.Add(tempImage.GetComponent<RawImage>());

            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 0);
            cameraList[i]._camera.targetTexture = rt;
            imageList[i].texture = rt;
        }
    }

    public void turnView()
    {
        int index = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        Camera.main.transform.position = cameraList[index]._camera_obj.transform.position;
        Camera.main.transform.rotation = cameraList[index]._camera_obj.transform.rotation;
    }
}
