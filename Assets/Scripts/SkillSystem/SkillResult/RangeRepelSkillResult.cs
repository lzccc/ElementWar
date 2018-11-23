using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRepelSkillResult : SkillResult {

    [Tooltip("击退速度")]
    public float repelSpeed;
    [Tooltip("击退时间")]
    public float repelTime;
    [Tooltip("击退触发范围")]
    public float radius;
    public Vector3 pos;//自身所在的坐标
    private void Start()
    {
        skillResultId = 7;
        skillKeepTime = skillKeepTime_Static;
    }
    Enemy enemy;
    public override void UseSkill(GameObject target, Enemy e, Vector3 forward)
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(target.transform.position, radius, pos.normalized*0.1f);
        if (hits == null) return;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag(CharacterType.Enemy.ToString()))
            {
                enemy = hits[i].collider.gameObject.GetComponent<Enemy>();
                for (int j = 0; j < skillId.Length; j++)//不是土系免疫
                {
                    if (enemy.attributeType == (ElementAttributeType)(skillId[j]))
                    {
                        return;
                    }
                }
                enemy.BeRepel(forward, repelSpeed, repelTime);
            }
        }
    }

    public override void ReSkill()
    {
        base.ReSkill();
    }
}
