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
                Debug.LogError("Ҫ�ϲ��ĸ����� m_combineGoRoot ����Ϊ�� ");
                return;
            }
            //Ҫ�ϲ�������
            MeshFilter[] filters = m_combineGoRoot.GetComponentsInChildren<MeshFilter>();
            //����ϲ�ʵ��
            CombineInstance[] combines = new CombineInstance[filters.Length];
            Debug.Log(" MeshFilter count " + filters.Length);
            //
            MeshRenderer[] renders = m_combineGoRoot.GetComponentsInChildren<MeshRenderer>();
            Debug.Log(" MeshRenderer count " + filters.Length);
            //�洢��ͬ�Ĳ��� 
            HashSet<Material> materialsHash = new HashSet<Material>();
            //�洢����Ҫ�ϲ�����ͼ
            Texture2D[] textures = new Texture2D[filters.Length];
            //�洢ģ�͵�uv
            List<Vector2[]> uvlist = new List<Vector2[]>();
            int uvcount = 0;
            for (int i = 0; i < filters.Length; i++)
            {
                combines[i].mesh = filters[i].sharedMesh;//ʹ�ñ༭��ִ��ֻ��ʹ��sharemesh
                ///��������ת��
                combines[i].transform = m_combineGoRoot.transform.worldToLocalMatrix * filters[i].transform.localToWorldMatrix;
                if (!materialsHash.Contains(renders[i].material)) materialsHash.Add(renders[i].material);
                uvlist.Add(filters[i].sharedMesh.uv);
                uvcount += filters[i].sharedMesh.uv.Length;
                ///ע��Ҫ��ȡ����ͼ����Ӧ������ _BaseMap ���ҵ�ǰshader�е���ͼ����
                textures[i] = renders[i].sharedMaterial.GetTexture("_BaseMap") as Texture2D;
                filters[i].gameObject.hideFlags = HideFlags.HideInHierarchy;
            }

            //�洢����
            Material[] materials = new Material[materialsHash.Count];
            int index = 0;
            foreach (var mat in materialsHash)
            {
                materials[index] = mat;
                index++;
            }
            ///�ϲ���ͼ 
            Texture2D combinetext = new Texture2D(1024, 1024);
            //���ÿ����ͼ������ͼ����ռ�ı��� ģ��uv����Ϊ 0~1
            Rect[] rects = combinetext.PackTextures(textures, 0);
            Debug.Log(rects[0].xMax);
            Vector2[] uvs = new Vector2[uvcount];
            int j = 0;
            //���� rects rects ����������filters ������
            for (int i = 0; i < filters.Length; i++)
            {
                //��������uv δ�ϲ�֮ǰ��uv
                foreach (Vector2 uv in uvlist[i])
                {
                    //���µ�uv���в�ֵ���� 
                    uvs[j].x = Mathf.Lerp(rects[i].xMin, rects[i].xMax, uv.x);
                    uvs[j].y = Mathf.Lerp(rects[i].yMin, rects[i].yMax, uv.y);
                    j++;
                }
            }
            #region 
            Material newmat = new Material(materials[0]);
            newmat.CopyPropertiesFromMaterial(materials[0]);
            //���ò�����ͼ ע�������ͼ��������
            newmat.SetTexture("_BaseMap", combinetext);
            newmat.name = "combineMat";
            #endregion

            //������������������
            MeshFilter filter = m_combineGoRoot.AddComponent<MeshFilter>();
            MeshRenderer renderer = m_combineGoRoot.AddComponent<MeshRenderer>();
            Mesh newmesh = new Mesh();
            filter.sharedMesh = newmesh;
            ///�ϲ��ղŵ�����mesh
            filter.sharedMesh.CombineMeshes(combines);
            filter.sharedMesh.uv = uvs;
            m_combineGoRoot.SetActive(true);
        }
    }


    /// <summary>
    /// ���ƴ���
    /// </summary>
    private void DrawWindow()
    {
        BeginWindows();
        EditorGUILayout.LabelField("�ϲ�������ڵ�:");

        m_combineGoRoot = (GameObject)EditorGUILayout.ObjectField(m_combineGoRoot, typeof(GameObject), true);
        this.m_click = GUILayout.Button("Combine");
        EndWindows();
    }
}