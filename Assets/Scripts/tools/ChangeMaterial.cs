using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ʹ��˵����
/// �Ѹýű��ŵ���Ҫ�滻���ʵ������ϣ����material���Խű��Ҽ����play
/// </summary>
[ExecuteInEditMode]
public class ChangeMaterial : MonoBehaviour
{
    public Material material;
    //List<GameObject>ObjList=new List<GameObject>();
    // Start is called before the first frame update
    [ContextMenu("Play")]
    void Play()
    {
        Debug.Log("ChangeMaterial");
        Transform[] temp = transform.GetComponentsInChildren<Transform>();
        foreach (var item in temp)
        {

            if (item.GetComponent<MeshRenderer>() != null)
            {
                item.GetComponent<MeshRenderer>().material = material;
                Debug.Log(item.name);
            }
        }
       
    }


}
