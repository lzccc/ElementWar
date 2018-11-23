using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 远程敌人
/// </summary>
public class RemoteEnemy : Enemy {

    private GameObject targetPlayer;
    [Header("箭矢")]
    public GameObject arrow;
    [Tooltip("箭矢减速百分比")]
    public int arrowSpeedCut;
    [Tooltip("箭矢减速时间")]
    public float speedCutTime;
    [Tooltip("飞行速度")]
    public float flySpeed;
    [Tooltip("飞行时间")]
    public float flyTime;

    private EnemyAnimController anim;
    private void Awake()
    {
        anim = GetComponentInChildren<EnemyAnimController>();
    }
    GameObject areaGo;
    //new
    public override void Start()
    {
        Init();
        base.Start();
    }
    public void Init()
    {
        float chance = Random.Range(0, 1f);
        if (chance < mutatationProbability)
        {
            isMutatation = true;
            transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            areaGo = Instantiate(areaEffect, transform);//生成精英光环
            areaGo.transform.localPosition = Vector3.zero-new Vector3(0,0.5f,0);
        }
    }
    /// <summary>
    /// 死亡后调用恢复精英
    /// </summary>
    public void ReInit()
    {
        if (!isMutatation) return;
        isMutatation = false;
        transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
        Destroy(areaGo);
    }
    /// <summary>
    /// 缓冲池出来后的初始化
    /// </summary>
    public override void ReStart()
    {
        Init();
        base.ReStart();
    }

    GameObject gogo;//临时对象
    Vector3 arrowForward;
    Vector3 arrowPosition;
    public override void Attack(GameObject target)
    {
        attackCoolTimer = attackCool;//攻击冷却开启
        targetPlayer = target;
        arrowPosition = new Vector3(transform.position.x, targetPlayer.transform.position.y, transform.position.z);
        arrowForward = (targetPlayer.transform.position - arrowPosition).normalized;
        anim.SetAnimator(AnimType.Attack);
    }

    /// <summary>
    /// 攻击动画完成时调用
    /// </summary>
    public override void OnAttackAnimFinish()
    {
        gogo = GameObject.Instantiate(arrow, arrowPosition+arrowForward*0.4f, Quaternion.identity);
        gogo.GetComponent<AttackArrowAI>().InitForward(arrowForward, attack, arrowSpeedCut, speedCutTime, flySpeed, flyTime);//初始化生成箭矢的数据
        //精英怪释放技能
        if (isMutatation)
        {
            gogo = GameObject.Instantiate(arrow, transform.position, Quaternion.identity);
            gogo.GetComponent<AttackArrowAI>().InitForward(RotationMatrix(arrowForward, 20), attack, arrowSpeedCut, speedCutTime, flySpeed, flyTime);//初始化生成箭矢的数据
            gogo = GameObject.Instantiate(arrow, transform.position, Quaternion.identity);
            gogo.GetComponent<AttackArrowAI>().InitForward(RotationMatrix(arrowForward, -20), attack, arrowSpeedCut, speedCutTime, flySpeed, flyTime);//初始化生成箭矢的数据
            //UseSkill();
        }
    }
    public override void OnDeath()
    {
        ReInit();
        AudioManager.Instance.PlaySound(AudioType.EnemyDead);//播放自爆音效
        base.OnDeath();
    }
    /// <summary>
    /// 计算矩阵的角度公式
    /// </summary>
    /// <param name="v"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Vector3 RotationMatrix(Vector3 v, float angle)
    {
        float x = v.x;
        float y = v.z;
        float sin = Mathf.Sin(Mathf.PI * angle / 180);
        float cos = Mathf.Cos(Mathf.PI * angle / 180);
        float newX = x * cos + y * sin;
        float newY = x * -sin + y * cos;
        return new Vector3(newX, v.y, newY);
    }
}
