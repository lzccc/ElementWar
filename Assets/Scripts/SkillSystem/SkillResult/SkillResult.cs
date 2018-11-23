using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 技能效果的基类
/// </summary>
public class SkillResult : MonoBehaviour {

    [Tooltip("效果ID")]
    public int skillResultId;
    [Tooltip("别动这个")]
    public float skillKeepTime;
    [Tooltip("技能buff持续时间(不变的标准999为无)")]
    public float skillKeepTime_Static=999;
    public bool IsInUse = true;
    [Header("技能元素的ID数组")]
    public int[] skillId;
    private void Awake()
    {
        skillKeepTime = skillKeepTime_Static;
    }
    public virtual void UseSkill(GameObject target, Enemy e, Vector3 skillForward)
    {
    }


    /// <summary>
    /// buff技能造成一次伤害
    /// </summary>
    public virtual void HarmBuffSkill()
    {

    }

    public virtual void ReSkill()
    {
        //Destroy(this);
    }

    /// <summary>
    /// 减去时间，若时间归零则返回true表示buff结束
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool SubtractTime(float time)
    {
        skillKeepTime -= time;
        if (skillKeepTime <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
