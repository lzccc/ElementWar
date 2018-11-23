using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 控制每个能量球的脚本
/// </summary>
public class SkillPrefabController : MonoBehaviour {

    private Vector3 forward;//能量球的前进方向
    private float speed;//能量球的前进速度
    private Enemy enemy;//能量球的命中目标
    private int[] skillId;//技能ID
    private bool noNull;//是否有碰撞到物体s
    Skill skill;//当前能量球所带的技能脚本
    private Vector3 beginPos;//出发的地点，用来计算飞行距离
    [Header("追踪子弹或箭的距离（仅适用于风系2级技能）")]
    public float findArrowDis = 4;
    [Header("敌人追踪范围（仅适用于风系2级技能）")]
    public float findEnemyDis = 10;
    private void Awake()
    {
        skill = this.GetComponent<Skill>();
        beginPos = this.transform.position;
    }
    private void Start()
    {
        skillId = skill.skillId;
        speed = skill.flySpeed;
        //if (skill.isBeam)
        //{

        //}
         if (skill.isFollow)//特殊攻击寻路
        {
            CheckAutoEnemy_Skill();
        }
        else if (!skill.noFollow)
        {
            CheckAutoEnemy_Normal();
        }
    }
    /// <summary>
    /// 初始化前进方向与速度
    /// </summary>
    /// <param name="forward"></param>
    /// <param name="speed"></param>
    public void InitTarget(Vector3 forward)
    {
        this.forward = forward.normalized;
        gameObject.transform.forward = forward;
    }
    // Update is called once per frame
    void Update()
    {
        if (autoFindTarget)//判断是否需要自动扇形寻路
        {
            if (target!=null&&target.activeSelf)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
                //防bug判断，如果距离小于1则当作碰撞
                if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
                {
                    if (target.CompareTag(CharacterType.Enemy.ToString()))//如果是一个敌人单位
                    {
                        enemy = target.GetComponent<Enemy>();
                        skill.UseSkill(target, enemy, forward);
                        Destroy(transform.gameObject);
                    }
                    else if(target.CompareTag(CharacterType.Arrow.ToString()))
                    {
                        skill.UseSkill(target, null, forward);
                        Destroy(target);
                        Destroy(transform.gameObject);
                    }
                    else if (target.CompareTag(CharacterType.AttackWall.ToString()))
                    {
                        skill.UseSkill(target, null, forward);
                        Destroy(target);
                        Destroy(transform.gameObject);
                    }
                    else if(target.CompareTag(CharacterType.Wall.ToString()))
                    {
                        skill.UseSkill(target, null, forward);
                        Destroy(transform.gameObject);
                    }
                }
            }
            else
            {
                if (skill.isFollow)
                {
                    //若敌人提前死亡
                    FindEnemy();//再找一个敌人
                    if (target == null || !target.activeSelf)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            transform.position += forward * Time.deltaTime * speed;
        }
        //这是判断前进方向的
        RaycastHit hit;
        noNull = Physics.SphereCast(transform.position, skill.sizeScale, forward, out hit, 0.2f);
        if (noNull)//碰撞到物体
        {
            if (hit.collider.CompareTag(CharacterType.Enemy.ToString()))
            {
                enemy = hit.collider.gameObject.GetComponent<Enemy>();
                skill.UseSkill(hit.collider.gameObject, enemy, forward);
                Destroy(transform.gameObject);
            }
            else if (hit.collider.CompareTag(CharacterType.Wall.ToString()))
            {
                if (!(skill.isFollow))//风属性追踪不撞墙
                {
                    skill.UseSkill(hit.collider.gameObject, enemy, forward);
                    Destroy(transform.gameObject);
                }
            }
            else if (hit.collider.CompareTag(CharacterType.Arrow.ToString()))
            {
                if (skill.canAttackArrow)
                {
                    skill.UseSkill(hit.collider.gameObject, enemy, forward);
                    Destroy(hit.collider.gameObject);
                    Destroy(transform.gameObject);
                }
            }
            else if (hit.collider.CompareTag(CharacterType.AttackWall.ToString()))//打中可被击碎的物体
            {
                skill.UseSkill(hit.collider.gameObject, enemy, forward);
                Destroy(hit.collider.gameObject);
                Destroy(transform.gameObject);
            }
            //else if (!(hit.collider.CompareTag(CharacterType.SmallWall.ToString()))&&!(hit.collider.CompareTag(CharacterType.Trigger.ToString()))&&
            //    !(hit.collider.CompareTag(CharacterType.Player.ToString()))&& !(hit.collider.CompareTag(CharacterType.Drop.ToString()))
            //    && !(hit.collider.CompareTag(CharacterType.Terrain.ToString())))
            //{
            //    Destroy(transform.gameObject);
            //}
        }
        skill.flyTime -= Time.deltaTime;
        if (skill.flyTime<=0)//超过飞行距离自动销毁
        {
            Destroy(transform.parent.gameObject);
        }

    }

    [Tooltip("检查扇形的角度")]
    public int angle;
    [Tooltip("检查扇形的距离")]
    public float dis;
    
    private bool autoFindTarget = false;//是否有追踪目标
    private GameObject target;//追踪的目标
    /// <summary>
    /// 检查是否有目标在扇形范围内，会受到自动攻击
    /// 这是普通攻击的追踪
    /// </summary>
    public void CheckAutoEnemy_Normal()
    {
        
        //选择敌人
        GameObject[] enemy = GameObject.FindGameObjectsWithTag(CharacterType.Enemy.ToString());
        for (int i = 0; i < enemy.Length; i++)
        {
            bool isInDistance = Vector3.Distance(transform.position, enemy[i].transform.position) < dis;
            // 判断角度
            bool isInAngle = Vector3.Angle(transform.forward, enemy[i].transform.position - transform.position) < (angle / 2);
            //Debug.Log("距离：" + Vector3.Distance(transform.position, enemy[i].transform.position) + "  角度：" + Vector3.Angle(transform.forward, enemy[i].transform.position - transform.position));

            if (isInDistance && isInAngle)
            {
                target = enemy[i];
                forward = (target.transform.position - transform.position).normalized;
                autoFindTarget = true;
                BaseCharacter.player.transform.forward = forward;//将人物转向跟踪的方向
                break;
            }
        }
    }
    /// <summary>
    /// 自动寻找一个目标进行追踪
    /// 这是技能的追踪
    /// </summary>
    public void CheckAutoEnemy_Skill()
    {
        StartCoroutine(ChechAutoEnemy());
    }
    /// <summary>
    /// 等0.3秒后追踪敌人
    /// </summary>
    /// <returns></returns>
    IEnumerator ChechAutoEnemy()
    {
        yield return new WaitForSeconds(0.3f);
        FindEnemy();
    }
    public void FindEnemy()
    {
        //选择箭矢，如果可以选择的话
        if (skill.canAttackArrow)
        {
            GameObject[] arrow = GameObject.FindGameObjectsWithTag(CharacterType.Arrow.ToString());
            for (int i = 0; i < arrow.Length; i++)
            {
                bool isInDistance = Vector3.Distance(transform.position, arrow[i].transform.position) < findArrowDis;//子弹追踪距离
                if (isInDistance)
                {
                    target = arrow[i];
                    forward = (target.transform.position - transform.position).normalized;
                    autoFindTarget = true;
                    break;
                }
            }
        }
        if (target == null)//没选到箭矢再选敌人
        {
            GameObject[] enemy = GameObject.FindGameObjectsWithTag(CharacterType.Enemy.ToString());
            if (enemy.Length > 0)
            {
                for (int i = 0; i < enemy.Length; i++)//优先寻找近距离的敌人
                {
                    bool isInDistance = Vector3.Distance(transform.position, enemy[i].transform.position) < findEnemyDis;//子弹追踪距离
                    if (isInDistance)
                    {
                        target = enemy[i];
                        forward = (target.transform.position - transform.position).normalized;
                        autoFindTarget = true;
                        break;
                    }
                }
                if (target == null)//没找到近距离的敌人，全图查找
                {
                    int index = Random.Range(0, enemy.Length);
                    target = enemy[index];
                    forward = (target.transform.position - transform.position).normalized;
                    autoFindTarget = true;
                }
            }
        }
    }
}
