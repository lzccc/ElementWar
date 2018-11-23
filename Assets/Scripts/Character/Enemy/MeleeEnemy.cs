using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 近战敌人
/// </summary>
public class MeleeEnemy : Enemy {
    [Tooltip("冲刺时间")]
    public float SprintTime=2;
    [Tooltip("冲撞判断范围")]
    public float radius=3;
    [Tooltip("眩晕时间")]
    public float vertigoTime;
    private float SprintTimer;
    private GameObject targetPlayer;
    public override void Start()
    {
        SprintTimer = SprintTime;
        base.Start();
    }


    private void Update()
    {
        if (isDeathTrigger) return;
        OnUpdate();
        SprintTimer -= Time.deltaTime;
        if (attackTrigger)
        {
            transform.position += playerForward * moveSpeed * 3 * Time.deltaTime;
            float dis = Vector3.Distance(targetPlayer.transform.position, transform.position);
            if (dis <= radius)//玩家在范围内:被撞到
            {
                //眩晕
                VertigoBuff vb = targetPlayer.AddComponent<VertigoBuff>();
                vb.buffTime = vertigoTime;
                vb.UseBuff(targetPlayer);
                attackTrigger = false;//撞到玩家，退出冲撞
            }
        }
        if (SprintTimer <= 0)
        {
            attackTrigger = false;
            AI.SetAi(false);//恢复自动寻路
        }

    }
    bool attackTrigger = false;//冲刺标志位
    Vector3 playerForward;
    /// <summary>
    /// 怪物的攻击方法，在这里是冲刺
    /// </summary>
    /// <param name="target"></param>
    public override void Attack(GameObject target)
    {
        attackCoolTimer = attackCool;
        targetPlayer = target;
        playerForward = (target.transform.position - transform.position).normalized;
        StartCoroutine(BeginAttack());
    }
    IEnumerator BeginAttack()
    {
        AI.SetAi(true);//设置不自动寻路
        PBar.startToShow(1, "眩晕冲撞");
        yield return new WaitForSeconds(1);
        attackTrigger = true;
        SprintTimer = SprintTime;
    }
    public override void ReStart()
    {
        base.ReStart();
        SprintTimer = SprintTime;
        attackTrigger = false;
    }
    public override void OnDeath()
    {
        AudioManager.Instance.PlaySound(AudioType.EnemyDead);//播放自爆音效
        base.OnDeath();
    }
}
