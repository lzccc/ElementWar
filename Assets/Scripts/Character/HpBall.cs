using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 杀怪掉落的回血球
/// </summary>
public class HpBall : MonoBehaviour {
    [Header("血球存在时间")]
    public float liftTime;
    [Header("血球回血百分比")]
    public int receiveRate;
    [Header("自动寻找玩家的距离")]
    public float findPlayerDis=4;
    [Header("自动飞行速度")]
    public float flySpeed=4;
    [Header("爆落物浮动速度")]
    public float floatSpeed=1;
    private float dis;
    private Transform player;
    private bool isUp=true;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(CharacterType.Player.ToString()).transform;
    }
    private void Update()
    {
        liftTime -= Time.deltaTime;
        if (liftTime <= 0)
        {
            Destroy(gameObject);
        }
        dis = Vector3.Distance(player.position, transform.position);
        if (dis < findPlayerDis)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, flySpeed * Time.deltaTime);
        }
        if (isUp)//上下浮动
        {
            transform.position += new Vector3(0, Time.deltaTime * floatSpeed, 0);
            if (transform.localPosition.y > 1.7)
            {
                isUp = false;
            }
        }
        else
        {
            transform.position -= new Vector3(0, Time.deltaTime * floatSpeed, 0);
            if (transform.localPosition.y < 1.5)
            {
                isUp = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int receiveNum = (int)(BasePlayerAttribute.instance.maxHealth * (receiveRate / 100f));
        BaseCharacter.player.Health += receiveNum;
        Destroy(gameObject);
    }
}
