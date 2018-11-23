using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人发出的箭矢
/// </summary>
public class AttackArrowAI : MonoBehaviour {
    [Tooltip("飞行速度")]
    private float flySpeed;
    [Tooltip("飞行时间")]
    private float flyTime;
    [Tooltip("碰撞距离")]
    public float colliderDis=1f;
    private bool flyTrigger = false;
    private int harmNum;//伤害值
    private int speedCut;//减速百分比
    private float keepTime;//buff持续时间
    private Vector3 forward;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    bool isHit;
    // Update is called once per frame
    void Update () {
        flyTime -= Time.deltaTime;
        if (flyTrigger)
        {
            transform.position += forward * flySpeed * Time.deltaTime;
            RaycastHit hit;
            isHit = Physics.SphereCast(transform.position, 0.4f, player.transform.position, out hit, colliderDis);
            if (isHit)
            {
                if (hit.collider.CompareTag(CharacterType.Player.ToString()))
                {
                    OnCollisionPlayer();
                    Destroy(this.gameObject);
                }
                else if (hit.collider.CompareTag(CharacterType.Wall.ToString())|| hit.collider.CompareTag(CharacterType.AttackWall.ToString()))
                {
                    Destroy(transform.gameObject);
                }
            }
        }
        if (flyTime <= 0) Destroy(gameObject);
	}
    /// <summary>
    /// 初始化飞行方向
    /// </summary>
    /// <param name="v">飞行的方向</param>
    /// <param name="harm">伤害</param>
    /// <param name="speedCut">减速百分比</param>
    public void InitForward(Vector3 v,int harm,int speedCut,float keepTime,float flySpeed,float flyTime)
    {
        forward = v.normalized;
        harmNum = harm;
        this.speedCut = speedCut;
        this.keepTime = keepTime;
        this.flySpeed = flySpeed;
        this.flyTime = flyTime;
        transform.forward = forward;
        flyTrigger = true;
    }
    /// <summary>
    /// 当碰撞到玩家时触发
    /// </summary>
    public void OnCollisionPlayer()
    {
        //扣血
        BaseCharacter.player.removeHealth(harmNum);
        SpeedCutBuff scb = player.AddComponent<SpeedCutBuff>();
        scb.buffPercentage = speedCut;//buff效果百分比
        scb.buffTime = keepTime;//持续时间
        scb.UseBuff(player);
    }
}
