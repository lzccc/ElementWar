using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 自爆敌人
/// </summary>
public class ExplosionEnemy : Enemy {
    [Tooltip("爆炸范围")]
    public float explosionRange;
    private float skillTime=0.5f;//技能前摇
    [Header("自爆特效")]
    public GameObject explosionEffect;
    [Header("精英怪自爆球")]
    public GameObject fireBall;
    //初始化随机精英怪
    public override void Start()
    {
        Init();
        base.Start();
    }
    GameObject areaGo;
    public void Init()
    {
        float chance = Random.Range(0, 1f);
        if (chance < mutatationProbability)
        {
            isMutatation = true;
            transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
            explosionRange += 2;
            skillTime = 1;
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
        maxHp *= 2;
        hp *= 2;
        explosionRange -= 2;
        skillTime = 0.5f;
        Destroy(areaGo);
    }
    public override void ReStart()
    {
        Init();
        base.ReStart();
    }
    public override void Attack(GameObject target)
    {
        if(gameObject.activeSelf)
           StartCoroutine(BeginAttack());
        //Destroy(gameObject);
    }
    public override void OnDeath()
    {
        Attack(GameObject.FindGameObjectWithTag("Player"));
    }
    GameObject explosionGo;
    IEnumerator BeginAttack()
    {
        AI.SetAi(true);//设置不自动寻路
        PBar.startToShow(skillTime, "自爆");
        yield return new WaitForSeconds(skillTime);
        explosionGo = ObjPoolManager.objpoolmanager.GetPoolsForName(explosionEffect.name).Active();
        explosionGo.transform.position = transform.position;
        //判断是否是精英怪
        if (isMutatation)
        {
            UseSkill_BarrageBall();
        }
        //调用爆炸动画
        //发射射线检测是否炸到,或直接检测玩家是否在爆炸距离
        GameObject player = GameObject.FindGameObjectWithTag(CharacterType.Player.ToString());
        float dis = Vector3.Distance(player.transform.position, transform.position);
        if (dis <= explosionRange)//玩家被炸到，进行扣血
        {
            BaseCharacter.player.Health -= attack;
        }
        ReInit();
        AudioManager.Instance.PlaySound(AudioType.EnemyExplode);//播放自爆音效
        base.OnDeath();
    }

    //new
    GameObject ball;
    //new

    /// <summary>
    /// 发射九个火球，精英怪技能
    /// //new
    /// </summary>
    public void UseSkill_BarrageBall()
    {
        for (int i = 0; i < 9; i++)
        {
            ball = Instantiate(fireBall, transform.position, Quaternion.identity);
            ball.GetComponent<AttackBallAI_Boss>().InitForward(transform.forward);
            transform.Rotate(0, 40, 0);
        }
    }
}
