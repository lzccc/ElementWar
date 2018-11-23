using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler{


    // 长按时间
    private float delay = 0.2f;
    // 是否按下
    private bool isDown = false;
    // 最后一次按住时间
    private float lastIsDownTime;

    public void Update()
    {
        // 如果按下且超过规定时间，显示buff介绍
        if (isDown)
        { 
            if (Time.time - lastIsDownTime > delay)
            {
                int buffId = int.Parse(transform.GetComponent<Button>().name);  //长按按钮对应的Buff ID
                Image img = BattlePanel.Instance.buffInfo;
                img.transform.localPosition = transform.GetComponent<Button>().transform.localPosition - new Vector3(-120, 40, 0); //根据长按的按钮的位置确定显示信息的位置
                img.transform.GetChild(0).GetComponent<Text>().text = BasePlayerAttribute.instance.GetExtraBuffNowInfo(buffId); //TODO 显示对应buff的等级和信息
                BattlePanel.Instance.buffInfo.gameObject.SetActive(true);

                lastIsDownTime = Time.time;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        lastIsDownTime = Time.time;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isDown = false;
        BattlePanel.Instance.buffInfo.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        BattlePanel.Instance.buffInfo.gameObject.SetActive(false);
    }
}
