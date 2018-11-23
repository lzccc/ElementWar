using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 控制敌人头顶的血条
/// </summary>
public class HpUIController : MonoBehaviour {
    
    private Image blood;
    private Transform t;
    [Header("血条出现的时间")]
    public float showTime=2;
    [Header("血条消失所需的时间")]
    public float hideSpeed=1;
    [Header("是否需要血条自动隐藏")]
    public bool autoHide = true;
    private Transform parent;
    /// <summary>
    /// 血条出现时间的计数器
    /// </summary>
    private float showTimer;
    Color startColor;//显示的颜色
    Color endColor;//隐藏的颜色
    Color downcolor;//每帧减去的颜色
    private void Awake()
    {
        blood = GetComponent<Image>();
        parent = transform.parent;
        t = transform;
        showTimer = 0;
        startColor = new Color(255, 255, 255, 1);
        endColor = new Color(255, 255, 255, 0);
        downcolor = new Color(0, 0, 0, 1/hideSpeed);
    }
    void Update()
    {
        if (autoHide)
        {
            if (showTimer > 0)
            {
                t.rotation = Camera.main.transform.rotation; //使血条朝向摄像机
                parent.rotation = Camera.main.transform.rotation; //使血条朝向摄像机
                showTimer -= Time.deltaTime;
            }
            else if (blood.color.a > 0)
            {
                blood.color -= downcolor * Time.deltaTime;
                t.rotation = Camera.main.transform.rotation; //使血条朝向摄像机
                parent.rotation = Camera.main.transform.rotation; //使血条朝向摄像机
            }
        }
        else
        {
            t.rotation = Camera.main.transform.rotation; //使血条朝向摄像机
            parent.rotation = Camera.main.transform.rotation; //使血条朝向摄像机
        }
    }
    /// <summary>
    /// 更新血条,并显示血条
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="maxHp"></param>
    public void UpdateHp(int hp, int maxHp)
    {
        if (autoHide)
        {
            blood.color = startColor;
            showTimer = showTime;
        }
        float fillAmount = hp / (float)maxHp;
        blood.fillAmount = fillAmount;
    }
}
