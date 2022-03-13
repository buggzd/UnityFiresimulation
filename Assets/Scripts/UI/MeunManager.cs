using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public struct btnContent
{
    public Button _btn;
    public Text _text;
    public List<string> _textList;
}

public class MeunManager : MonoBehaviour
{
    public GameObject tempMenuBtn;
    public GameObject tempText;
    public Transform btnTran;
    [SerializeField]
    public List<btnContent> btnList = new List<btnContent>();

    public Transform contextTran;

    public void addMenuBtn(string _context)
    {
        GameObject temp_obj = Instantiate(tempMenuBtn, btnTran);
        btnContent temp = new btnContent();
        temp_obj.transform.GetChild(0).GetComponent<Text>().text = _context;
        temp_obj.GetComponent<Button>().onClick.AddListener(() => { onclickReact(); });
        temp._btn = temp_obj.GetComponent<Button>();
        temp._text = temp_obj.GetComponent<Text>();
        temp._textList = new List<string>();
        btnList.Add(temp);
    }

    public void addMenuText(int index, string _context)
    {
        btnList[index]._textList.Add(_context);
    }

    public void onclickReact()
    {
        if(contextTran.childCount!=0)
            for (int i = 0; i < contextTran.childCount; i++)
            {
                Destroy(contextTran.GetChild(i).gameObject);
            } 
        int index = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        if (btnList[index]._textList.Count != 0)
            foreach (var item in btnList[index]._textList)
            {
                GameObject temp = Instantiate(tempText, contextTran);
                temp.GetComponent<Text>().text = item;
            }
    }
}
