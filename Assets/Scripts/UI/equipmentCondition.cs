using UnityEngine;
using UnityEngine.UI;

public class equipmentCondition : MonoBehaviour
{
    //进度条速度
    public float speed;
    //一个图片一个文字
    public Transform m_Image;
    public Transform m_Text;
    public Transform m_Image1;
    public Transform m_Text1;
    public Transform m_Image2;
    public Transform m_Text2;
    //进度控制
    public int targetProcess = 100;
    private float currentAmout = 100;
    private float currentAmout1=100 ;
    private float currentAmout2 =0;
    private void Start()
    {
        m_Image.GetComponent<Image>().color = new Color(0.8f, 0.3f, 0.5f, 0.5f);
        m_Image1.GetComponent<Image>().color = new Color(0.8f, 0.2f, 0.1f, 0.5f);
        m_Image2.GetComponent<Image>().color = new Color(0.1f, 0.3f, 0.8f, 0.5f);
    }
    void Update()
    {
        if (currentAmout2 < currentAmout)
        {
            currentAmout2 += speed;
            currentAmout1 = currentAmout - currentAmout2;
            if (currentAmout2 > currentAmout)
                currentAmout2 = currentAmout;
            m_Text.GetComponent<Text>().text = ((int)currentAmout).ToString();
            m_Image.GetComponent<Image>().fillAmount = currentAmout / 100.0f;
            m_Text1.GetComponent<Text>().text = ((int)currentAmout1).ToString();
            m_Image1.GetComponent<Image>().fillAmount = currentAmout1 / 100.0f;
            m_Text2.GetComponent<Text>().text = ((int)currentAmout2).ToString();
            m_Image2.GetComponent<Image>().fillAmount = currentAmout2 / 100.0f;
        }
    }
}