using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SplitEnemy : Enemy
{
    private GameObject targetPlayer;

    [Header("默认分裂时间")]
    public float splitTime = 10f;
    [Header("怪兽已经存活的时间")]
    public float survive = 0;
    [Header("是否已经分裂过")]
    public int isSplitedNum = 2;
    [Header("分裂后的血量比例，越大越小")]
    public int newHp=2;
    [Header("精英怪的技能毒雾预制体")]
    public GameObject poisonousFog;
    [Header("变异概率出现的速度")]
    public float variationSpeed=20;
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
            areaGo.transform.localPosition = Vector3.zero - new Vector3(0, 0.5f, 0);
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
    /// 精英怪的技能，死亡后释放毒雾
    /// </summary>
    void poisonFrog()
    {
        GameObject go;
        go = Instantiate(poisonousFog, transform.position, poisonousFog.transform.rotation);
        go.transform.localScale = new Vector3(5, 0.2f, 5);
    }

    public override void Attack(GameObject target)
    {
        targetPlayer = target;
        attackCoolTimer = attackCool;//攻击冷却开启
        float range = Vector3.Distance(targetPlayer.transform.position, transform.position);
        if (range <= attackRange)
        {
            BaseCharacter.player.Health -= attack;
        }
    }
    GameObject newSplit;
    public void TrySplit()
    {
        if (isSplitedNum <= 0) return;
        survive += Time.deltaTime;
        if (survive >= splitTime)
        {
            survive = -1;
            if (Random.Range(0f, 1) < 0.2)
            {
                GetComponent<NavMeshAgent>().speed = variationSpeed;//变异速度小怪
                moveSpeed = variationSpeed;
                HpChange(-hp / 50,null);//减血百分50
                areaGo = Instantiate(areaEffect, transform);//生成精英光环
                areaGo.transform.localPosition = Vector3.zero - new Vector3(0, 0.5f, 0);
            }
            else
            {
                StartCoroutine(BeginAttack());
            }
        }
    }
    SplitEnemy split;
    IEnumerator BeginAttack()
    {
        AI.SetAi(true);//设置不自动寻路
        PBar.startToShow(1, "分裂");
        yield return new WaitForSeconds(1);
        newSplit = ObjPoolManager.objpoolmanager.GetPoolsForName(PoolType.SplitEnemy.ToString()).Active();
        // Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity);
        newSplit.GetComponent<NavMeshAgent>().Warp(transform.position);
        newSplit.transform.SetParent(transform.parent);
        newSplit.name = "SplitEnemy";
        isSplitedNum --;
        split = newSplit.GetComponent<SplitEnemy>();
        split.hp = maxHp / newHp;
        split.isSplitedNum = isSplitedNum;
        AI.SetAi(false);//恢复寻路
    }
    // Update is called once per frame
    void Update () {
        OnUpdate();
        if (AI.enabled)
        {
            TrySplit();
        }
	}

    public override void ReStart()
    {
        survive = 0;
        Init();
        base.ReStart();
    }
    
    public override void OnDeath()
    {
        if (isMutatation)
        {
            poisonFrog();
        }
        ReInit();
        AudioManager.Instance.PlaySound(AudioType.EnemyDead);//播放自爆音效
        base.OnDeath();
    }
}
