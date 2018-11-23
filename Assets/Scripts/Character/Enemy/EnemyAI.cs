using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 怪物的AI
/// </summary>
public class EnemyAI : AI {
    
    private EnemyAnimController anim;
    public override void Awake()
    {
        anim = GetComponentInChildren<EnemyAnimController>();
        base.Awake();
    }

    private void Update()
    {
        if (!aiTrigger || stopTrigger) return;
        //transform.forward = (player.transform.position - transform.position).normalized;
        if (Vector3.Distance(player.transform.position,transform.position)>enemy.attackRange)
        {
            if (agent.isOnNavMesh)
            {
                //这里继续寻路
                if (agent.isStopped)
                    agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                if (anim != null)
                    anim.SetAnimator(AnimType.Run);
            }
            else
            {
                ObjPoolManager.objpoolmanager.GetPoolsForName(gameObject.name).Deactive(gameObject);
            }
        }
        else
        {
            transform.forward = (player.transform.position - transform.position).normalized;
            if (enemy.attackCoolTimer == 0)//冷却时间
            {
                agent.isStopped = true;
                enemy.Attack(player);
                enemy.attackCoolTimer = enemy.attackCool;
            }
        }
    }
}
