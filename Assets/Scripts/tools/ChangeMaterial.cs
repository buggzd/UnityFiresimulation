using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 使用说明：
/// 把该脚本放到需要替换材质的物体上，添加material，对脚本右键点击play
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
    /// 把tag为_tag的物体的所有子物体的tag都设置为_tag
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

    //更改trans的tag为T
    public static void SetGameObjectTag(Transform trans, string T)
    {
        if (!UnityEditorInternal.InternalEditorUtility.tags.Equals(T)) //如果tag列表中没有这个tag
        {
            UnityEditorInternal.InternalEditorUtility.AddTag(T); //在tag列表中添加这个tag
        }

        trans.tag = T;
    }


}
