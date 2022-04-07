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

    public string _tag = "Windows";
    
    //List<GameObject>ObjList=new List<GameObject>();
    // Start is called before the first frame update

    [ContextMenu("Set Material")]
    public void SetMaterial()
    {
        
            
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(_tag);
        if(gameObjects.Length <= 0)
        {
            Debug.Log("there is no obj'_tag named " + _tag);
        }
        else
        {
            foreach (var item in gameObjects)
            {

                if (item.transform.GetComponent<MeshRenderer>() != null)
                {
                    item.transform.GetComponent<MeshRenderer>().material = material;
                    Debug.Log(item.name);
                }
            }
        }
            
        

       
    }
    /// <summary>
    /// ��tagΪ_tag������������������tag������Ϊ_tag
    /// </summary>
    [ContextMenu("Set Childrens'Tag as them father")]
    public void SetChildrenTag()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(_tag);
        foreach (var item in gameObjects)
        {
            foreach(var item2 in item.GetComponentsInChildren<Transform>())
            {
                SetGameObjectTag(item2,_tag);
            }
            
        }
    }

    //����trans��tagΪT
    public static void SetGameObjectTag(Transform trans, string T)
    {
        if (!UnityEditorInternal.InternalEditorUtility.tags.Equals(T)) //���tag�б���û�����tag
        {
            UnityEditorInternal.InternalEditorUtility.AddTag(T); //��tag�б���������tag
        }

        trans.tag = T;
    }


}
