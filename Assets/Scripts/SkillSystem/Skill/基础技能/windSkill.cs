using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 风系技能
/// </summary>
public class windSkill : Skill {

    private void Awake()
    {
        isFollow = true;
    }

    /// <summary>
    /// 初始化技能等级
    /// </summary>
    /// <param name="lv"></param>
    public override void InitLv(int lv)
    {
        Level = lv;
        if (Level > 1)//如果等级为1则无法开启二级特效
        {
            canAttackArrow = true;
        }
    }
}
