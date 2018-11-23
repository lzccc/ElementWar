using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 消息盒子
/// </summary>
public class MesBox : MonoBehaviour {

    [Header("显示文字的文本框")]
    public Text msgText;
    private Image msgBox;
    private Vector2 target;
    [Header("移动速度")]
    public float msgBoxSpeed=10;
    [Header("出现速度")]
    public float showSpeed = 0.8f;
    [Header("消失速度")]
    public float hideSpeed = 0.5f;
    [Header("时间缩放倍率")]
    public float scale = 1;
    bool up = false;//true表示开始消失
    bool trigger = false;
    private void Awake()
    {
        msgBox = this.GetComponent<Image>();
    }
    // Use this for initialization
    void Start () {
    }
    private void Update()
    {
        if (trigger)
        {
            msgBox.rectTransform.localPosition += new Vector3(0, msgBoxSpeed * Time.deltaTime,0);
            if (msgBox.rectTransform.localPosition.y >= 0)
            {
                StartCoroutine(waitDestory());
            }
        }
        if (msgText.color.a >= 1)
        {
            up = true;
        }
        if (up)
        {
            //msgBox.color = new Color(msgBox.color.r, msgBox.color.g, msgBox.color.b, msgBox.color.a - hideSpeed * Time.deltaTime);
            msgText.color = new Color(msgText.color.r, msgText.color.g, msgText.color.b, msgText.color.a - hideSpeed * Time.deltaTime);
        }
        else
        {
            //msgBox.color = new Color(msgBox.color.r, msgBox.color.g, msgBox.color.b, msgBox.color.a + showSpeed * Time.deltaTime);
            msgText.color = new Color(msgText.color.r, msgText.color.g, msgText.color.b, msgText.color.a + showSpeed * Time.deltaTime);
        }
    }

    public void StartAnim(string msg)
    {
        msgText.text = msg;
        trigger = true;
    }
    IEnumerator waitDestory()
    {
        yield return new WaitForSeconds(3f);
        DestoryThis();
    }
    public void DestoryThis()
    {
        GameObject.Destroy(this.gameObject);
    }
}
