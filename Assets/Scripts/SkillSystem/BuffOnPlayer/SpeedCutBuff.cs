using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 速度相关的buff
/// </summary>
public class SpeedCutBuff : Buff {
    private int changeSpeed;
    private GameObject player;
    private void Start()
    {
        buffId = 1;
    }
    public override void UseBuff(GameObject player)
    {
        if (BasePlayerAttribute.instance.isClear) return;//免疫减速
        this.player = player;
        changeSpeed = (int)(player.GetComponent<BaseCharacter>().speed * (buffPercentage / 100f));
        player.GetComponent<BaseCharacter>().speed -= changeSpeed;
        player.GetComponent<BaseCharacter>().buffList.Add(this);
    }

    public override void ReBuff()
    {
        player.GetComponent<BaseCharacter>().speed += changeSpeed;
        Destroy(this);
    }
}
