using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人的吸收技能
/// </summary>
public class AssimilateSkillResult : SkillResult {

    [Tooltip("吸收技能启动特效")]
    public GameObject assimilateEffect;

    private GameObject go;
    private Enemy enemy;
    private void Start()
    {
        skillResultId = 100;
        skillKeepTime = skillKeepTime_Static;
    }

    public override void UseSkill(GameObject target, Enemy e, Vector3 skillForward)
    {
        enemy = e;
        //加持吸收特效
        go = Instantiate(assimilateEffect, e.transform);
        e.GetSkillById(skillResultId);
        e.SetAssimilate(true);
        e.skillList.Add(this);
    }
    public override void ReSkill()
    {
        enemy.SetAssimilate(false);
        Destroy(go);
        Destroy(this);
        base.ReSkill();
    }

}
