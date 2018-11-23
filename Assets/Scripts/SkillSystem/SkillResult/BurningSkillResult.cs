using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningSkillResult : SkillResult {

    [Tooltip("燃烧特效")]
    public GameObject effect;
    [Tooltip("燃烧伤害")]
    public int harmNum;//伤害值
    [Tooltip("命中敌人的脚本")]
    public Enemy enemy;
    private GameObject effectGo;
    private void Start()
    {
        skillResultId = 2;
        skillKeepTime = skillKeepTime_Static;
    }

    public override void UseSkill(GameObject target, Enemy e, Vector3 skillForward)
    {
        enemy = e;
        for (int i = 0; i < skillId.Length; i++)
        {
            if (enemy.attributeType == (ElementAttributeType)(skillId[i]))
            {
                return;
            }
        }
        enemy.GetSkillById(skillResultId);
        //加持燃烧特效
        effectGo = ObjPoolManager.objpoolmanager.GetPoolsForName(PoolType.burning_1.ToString()).Active();
        effectGo.name = effect.name;
        effectGo.transform.SetParent(e.transform);
        effectGo.transform.localPosition = Vector3.zero;
        //Instantiate(effect, e.transform);
        e.skillList.Add(this);
    }
    /// <summary>
    /// buff技能造成一次伤害
    /// </summary>
    public override void HarmBuffSkill()
    {
        enemy.HpChange(-harmNum, null);
    }

    public override void ReSkill()
    {
        ObjPoolManager.objpoolmanager.GetPoolsForName(effectGo.name).Deactive(effectGo);
        base.ReSkill();
    }
}
