using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshCombineWindow : EditorWindow
{

    private GameObject m_combineGoRoot;
    private bool m_click;

    [MenuItem("ToolsEditor/Combine Mesh")]
    static void ShowEditor()
    {
        MeshCombineWindow combinewindow = GetWindow<MeshCombineWindow>();
        combinewindow.minSize = new Vector2(700, 500);
    }


    private void OnGUI()
    {
        this.DrawWindow();
        if (m_click)
        {
            this.m_click = false;
            if (m_combineGoRoot == null)
            {
                Debug.LogError("要合并的父物体 m_combineGoRoot 不能为空 ");
                return;
            }
            //要合并的网格
            MeshFilter[] filters = m_combineGoRoot.GetComponentsInChildren<MeshFilter>();
            //网格合并实例
            CombineInstance[] combines = new CombineInstance[filters.Length];
            Debug.Log(" MeshFilter count " + filters.Length);
            //
            MeshRenderer[] renders = m_combineGoRoot.GetComponentsInChildren<MeshRenderer>();
            Debug.Log(" MeshRenderer count " + filters.Length);
            //存储不同的材质 
            HashSet<Material> materialsHash = new HashSet<Material>();
            //存储所有要合并的贴图
            Texture2D[] textures = new Texture2D[filters.Length];
            //存储模型的uv
            List<Vector2[]> uvlist = new List<Vector2[]>();
            int uvcount = 0;
            for (int i = 0; i < filters.Length; i++)
            {
                combines[i].mesh = filters[i].sharedMesh;//使用编辑器执行只能使用sharemesh
                ///网格坐标转换
                combines[i].transform = m_combineGoRoot.transform.worldToLocalMatrix * filters[i].transform.localToWorldMatrix;
                if (!materialsHash.Contains(renders[i].material)) materialsHash.Add(renders[i].material);
                uvlist.Add(filters[i].sharedMesh.uv);
                uvcount += filters[i].sharedMesh.uv.Length;
                ///注意要获取的贴图所对应的属性 _BaseMap 是我当前shader中的贴图属性
                textures[i] = renders[i].sharedMaterial.GetTexture("_BaseMap") as Texture2D;
                filters[i].gameObject.hideFlags = HideFlags.HideInHierarchy;
            }

            //存储材质
            Material[] materials = new Material[materialsHash.Count];
            int index = 0;
            foreach (var mat in materialsHash)
            {
                materials[index] = mat;
                index++;
            }
            ///合并贴图 
            Texture2D combinetext = new Texture2D(1024, 1024);
            //存放每张贴图在新贴图中所占的比例 模型uv比例为 0~1
            Rect[] rects = combinetext.PackTextures(textures, 0);
            Debug.Log(rects[0].xMax);
            Vector2[] uvs = new Vector2[uvcount];
            int j = 0;
            //遍历 rects rects 的数量就是filters 的数量
            for (int i = 0; i < filters.Length; i++)
            {
                //遍历物体uv 未合并之前的uv
                foreach (Vector2 uv in uvlist[i])
                {
                    //对新的uv进行插值计算 
                    uvs[j].x = Mathf.Lerp(rects[i].xMin, rects[i].xMax, uv.x);
                    uvs[j].y = Mathf.Lerp(rects[i].yMin, rects[i].yMax, uv.y);
                    j++;
                }
            }
            #region 
            Material newmat = new Material(materials[0]);
            newmat.CopyPropertiesFromMaterial(materials[0]);
            //设置材质贴图 注意材质贴图属性名称
            newmat.SetTexture("_BaseMap", combinetext);
            newmat.name = "combineMat";
            #endregion

            //给父物体添加网格组件
            MeshFilter filter = m_combineGoRoot.AddComponent<MeshFilter>();
            MeshRenderer renderer = m_combineGoRoot.AddComponent<MeshRenderer>();
            Mesh newmesh = new Mesh();
            filter.sharedMesh = newmesh;
            ///合并刚才的所有mesh
            filter.sharedMesh.CombineMeshes(combines);
            filter.sharedMesh.uv = uvs;
            m_combineGoRoot.SetActive(true);
        }
    }


    /// <summary>
    /// 绘制窗口
    /// </summary>
    private void DrawWindow()
    {
        BeginWindows();
        EditorGUILayout.LabelField("合并网格根节点:");

        m_combineGoRoot = (GameObject)EditorGUILayout.ObjectField(m_combineGoRoot, typeof(GameObject), true);
        this.m_click = GUILayout.Button("Combine");
        EndWindows();
    }
}