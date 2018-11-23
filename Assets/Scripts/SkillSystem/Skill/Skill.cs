using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

    [Header("技能ID,组合元素的id为组合技能ID+两个基础技能id一共三个")]
    public int[] skillId;
    [Header("伤害值")]
    public int harmNum;//伤害值
    [Header("技能冷却时间")]
    public float coolTime;
    [Header("飞行存活时间")]
    public float flyTime=5;
    [Header("飞信速度")]
    public float flySpeed=10;
    [Header("能量球的大小缩放")]
    public float sizeScale=1;//能量球的大小缩放
    [Header("自动全追踪")]
    public bool isFollow = false;
    [Header("全不追踪")]
    public bool noFollow = false;
    [Header("是激光")]
    public bool isBeam = false;
    [Header("可以攻击箭矢")]
    public bool canAttackArrow = false;
    [Header("可以穿透")]
    public bool canThrough = false;
    [Header("命中敌人的脚本")]
    public Enemy enemy;
    [Header("需要哪些效果，拖进来")]
    public List<SkillResult> resultList = new List<SkillResult>();
    [Header("技能等级，初始为1，最高为2")]
    [SerializeField]private int level = 1;

    [Header("技能命中特效预制体")]
    public GameObject colliderEffect;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    GameObject go;
    public virtual void UseSkill(GameObject target, Enemy e, Vector3 skillForward)
    {
        go = ObjPoolManager.objpoolmanager.GetPoolsForName(colliderEffect.name).Active();
        go.name = colliderEffect.name;
        if (e == null)
        {
            go.transform.position = transform.position;
        }
        else
        {
            go.transform.position = e.transform.position;
            //扣血
            enemy = e;
            e.HpChange(-harmNum, skillId);
            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i].IsInUse)//如果这个特效已经开启
                {
                    resultList[i].UseSkill(target, enemy, skillForward);
                }
            }
        }
    }
    public virtual void ReSkill()
    {

    }
    /// <summary>
    /// 初始化融合技能的基础技能等级
    /// 顺序为：
    /// 火土木风为1234
    /// 火土
    /// 火木
    /// 火风
    /// 土木
    /// 土风
    /// 木风
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public virtual void InitSkillResult(int a, int b)
    {

    }

    public virtual void InitLv(int lv)
    {
    }

}
