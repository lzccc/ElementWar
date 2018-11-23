using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {
    protected Enemy enemy;
    protected GameObject player;
    protected NavMeshAgent agent;
    protected bool aiTrigger = true;//寻路开关
    protected bool stopTrigger=false;
    public virtual void Awake()
    {
        enemy = this.GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemy.moveSpeed;
    }
    /// <summary>
    /// 设置ai是否寻路
    /// </summary>
    /// <param name="b">true为不循路</param>
    public void SetAi(bool b)
    {
        aiTrigger = !b;
        if (agent.isOnNavMesh)
        {
            if (b)
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
            }
            else
            {
                if (gameObject.activeSelf)
                {
                    agent.isStopped = false;
                }
            }
        }
        else
        {
            ObjPoolManager.objpoolmanager.GetPoolsForName(gameObject.name).Deactive(gameObject);
        }
    }
    /// <summary>
    /// 死亡后停止所有动作
    /// </summary>
    public void OnDeathStopAI()
    {
        agent.SetDestination(transform.position);
        agent.isStopped = true;
        stopTrigger = true;
    }
}
