using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家的buff，需要注意要主动销毁
/// </summary>
public class Buff : MonoBehaviour {

    public int buffId;
    [Header("buff持续时间")]
    public float buffTime;
    [Header("buff百分比")]
    public int buffPercentage;
    [Header("击退速度，仅限于击退技能")]
    public float repelSpeed;

    /// <summary>
    /// 给玩家使用buff
    /// </summary>
    /// <param name="player">玩家或发出技能的敌人</param>
    public virtual void UseBuff(GameObject targer)
    {
        
    }
    public virtual void ReBuff()
    {

    }

    /// <summary>
    /// buff技能造成一次伤害
    /// </summary>
    public virtual void HarmBuffSkill()
    {

    }
    /// <summary>
    /// 减去时间，若时间归零则返回true表示buff结束
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool SubtractTime(float time)
    {
        buffTime -= time;
        if (buffTime <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
