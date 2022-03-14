using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class AlarmSignUI : MonoBehaviour
{
    public Dictionary<GameObject, GameObject> alarmList = new Dictionary<GameObject, GameObject>();
    public GameObject tempAlarmSign;
    public Transform alarmTran;
    public Vector2 _offset;


    private void Start()
    {
        if (GameObject.FindGameObjectsWithTag("alarm").Length != 0)
            foreach (var item in GameObject.FindGameObjectsWithTag("alarm"))
            {
                GameObject tempSign = Instantiate(tempAlarmSign, alarmTran);
                alarmList[item] = tempSign;
            }
    }

    private void Update()
    {
        if (alarmList.Count != 0)
            foreach (var item in alarmList)
            {
                if (InView(item.Key.transform.position))
                {
                    Vector2 vec2 = Camera.main.WorldToScreenPoint(item.Key.transform.position);
                    item.Value.SetActive(true);
                    item.Value.transform.position = vec2 + _offset;
                }
                else
                {
                    item.Value.SetActive(false);
                }
            }
    }

    bool InView(Vector3 pos)
    {
        Transform camTran = Camera.main.transform;
        Vector2 viewPos = Camera.main.WorldToViewportPoint(pos);
        Vector3 dir = (pos - camTran.position).normalized;
        float dot = Vector3.Dot(camTran.forward, dir);
        if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            return true;
        else return false;
    }

}
