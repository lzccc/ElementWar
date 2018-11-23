using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : AI {
    
    private BossAnimController anim;
    public override void Awake()
    {
        anim = GetComponent<BossAnimController>();
        base.Awake();
    }
    private void OnEnable()
    {
        AudioManager.Instance.PlaySound(AudioType.BossYellSound);//boss出场音效
        AudioManager.Instance.ChangeMusic(AudioType.Battle);//boss战音效

    }
    private void Update()
    {
        if (!aiTrigger||stopTrigger) return;
        if (Vector3.Distance(player.transform.position, transform.position) > enemy.attackRange)
        {
            //这里继续寻路
            if (agent.isStopped)
                agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            anim.SetAnimator(AnimType.Run);
        }
        else
        {
            if (enemy.attackCoolTimer == 0)//冷却时间
            {
                agent.isStopped = true;
                enemy.Attack(player);
                enemy.attackCoolTimer = enemy.attackCool;
            }
        }
    }
    
}
