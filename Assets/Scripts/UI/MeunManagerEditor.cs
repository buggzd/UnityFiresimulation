using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

[CustomEditor(typeof(MeunManager))]
public class MeunManagerEditor : Editor
{
    public int btnIndex;
    public string _text;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("btnName");
        _text = EditorGUILayout.TextArea(_text, GUILayout.MinHeight(100));
        EditorGUILayout.LabelField("IndexNum");
        btnIndex = EditorGUILayout.IntField(btnIndex, GUILayout.MinHeight(10));

        MeunManager _target = target as MeunManager;

        if (GUILayout.Button("addBtn"))
        {
            if (_text != "")
                _target.addMenuBtn(_text);
        }

        if (GUILayout.Button("addText"))
        {
            if ((btnIndex) < _target.btnList.Count && _text != "" && btnIndex > 0)
                _target.addMenuText(btnIndex - 1, _text);
        }
    }
}
