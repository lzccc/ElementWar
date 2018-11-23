using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 减速特效
/// </summary>
public class SpeedCutSkillResult : SkillResult {

    [Tooltip("减速特效")]
    public GameObject effect;
    [Tooltip("减速百分比")]
    public int speedCut;//伤害值
    private float cutNum;//被减去的数值
    [Tooltip("命中敌人的脚本")]
    public Enemy enemy;
    [Header("是否是一次性击退")]
    public bool isOnceRepel;

    private void Start()
    {
        skillResultId = 3;
        skillKeepTime = skillKeepTime_Static;
    }

    public override void UseSkill(GameObject target, Enemy e, Vector3 skillForward)
    {
        if (isOnceRepel) return;
        //加持减速特效
        enemy = e;
        for (int i = 0; i < skillId.Length; i++)//不是土系免疫
        {
            if (enemy.attributeType == (ElementAttributeType)(skillId[i]))
            {
                return;
            }
        }
        enemy.GetSkillById(skillResultId);
        cutNum = e.moveSpeed * (speedCut / 100f);
        e.SpeedChange(-cutNum);
        e.skillList.Add(this);
    }

    public override void ReSkill()
    {
        enemy.SpeedChange(cutNum);
        base.ReSkill();
    }
}
