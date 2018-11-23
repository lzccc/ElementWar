using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 爆炸效果
/// </summary>
public class ExplosionSkillResult : SkillResult {
    [Tooltip("一个圈形的范围特效")]
    public GameObject explosionEffect;
    [Tooltip("爆炸半径")]
    public float radius;
    [Tooltip("爆炸伤害值")]
    public int harmNum;//伤害值
    public Transform pos;//自身所在的坐标
    private void Start()
    {
        skillResultId = 5;
        skillKeepTime = skillKeepTime_Static;
    }
    GameObject go;
    public override void UseSkill(GameObject target, Enemy e, Vector3 skillForward)
    {
        //go = ObjPoolManager.objpoolmanager.GetPoolsForName(explosionEffect.name).Active();
        //go.transform.position = e.transform.position;
        //以下是爆炸减伤
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(e.transform.position, radius, skillForward/*pos.forward*/, 0.1f);
        if (hits == null) return;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag(CharacterType.Enemy.ToString()))
            {
                hits[i].collider.gameObject.GetComponent<Enemy>().HpChange(-harmNum,skillId);//1表示火系
            }
        }
    }

    public override void ReSkill()
    {
        base.ReSkill();
    }
}
