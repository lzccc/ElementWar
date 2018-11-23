using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 击退效果
/// </summary>
public class RepelSkillResult : SkillResult {

    [Tooltip("击退速度")]
    public float repelSpeed;
    [Tooltip("击退时间")]
    public float repelTime;
    [Tooltip("命中敌人的脚本")]
    public Enemy enemy;
    [Header("是否是一次性击退")]
    public bool isOnceRepel;
    private void Start()
    {
        skillResultId = 6;
        skillKeepTime = skillKeepTime_Static;
    }

    public override void UseSkill(GameObject target, Enemy e, Vector3 forward)
    {
        if (isOnceRepel) return;
        enemy = e;
        //击退
        for (int i = 0; i < skillId.Length; i++)//不是土系免疫
        {
            if (enemy.attributeType == (ElementAttributeType)(skillId[i]))
            {
                return;
            }
        }
        enemy.BeRepel(forward, repelSpeed, repelTime);
    }

    public override void ReSkill()
    {
        base.ReSkill();
    }
}
