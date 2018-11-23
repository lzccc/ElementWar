using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBallAI_Boss : MonoBehaviour {

    private Vector3 forward;
    private bool flyTrigger=false;
    [Tooltip("飞球伤害")]
    public int attackHarm = 15;
    [Tooltip("飞行存活时间")]
    public float flyTime = 5;
    [Tooltip("飞信速度")]
    public float flySpeed = 10;
    [Tooltip("能量球的大小缩放")]
    public float sizeScale = 1;//能量球的大小缩放
    public GameObject explosionEffect;
    private void Awake()
    {
        transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);
    }
    public Vector3 Forward
    {
        set
        {
            forward = value;
        }
    }
    bool noNull;
    GameObject go;
    private void Update()
    {
        flyTime -= Time.deltaTime;
        if (flyTime <= 0) Destroy(gameObject);
        if (flyTrigger)
        {
            transform.position += forward * flySpeed * Time.deltaTime;
        }
        RaycastHit hit;
        noNull = Physics.SphereCast(transform.position, sizeScale, forward, out hit, sizeScale);
        if (noNull)//碰撞到物体
        {
            go = ObjPoolManager.objpoolmanager.GetPoolsForName(explosionEffect.name).Active();
            go.name = explosionEffect.name;
            go.transform.position = hit.collider.gameObject.transform.position;
            if (hit.collider.CompareTag(CharacterType.Player.ToString()))
            {
                BaseCharacter.player.removeHealth(attackHarm);
                Destroy(transform.gameObject);
            }
            else if (hit.collider.CompareTag(CharacterType.Wall.ToString())|| hit.collider.CompareTag(CharacterType.AttackWall.ToString()))
            {
                Destroy(transform.gameObject);
            }
        }
    }

    public void InitForward(Vector3 v)
    {
        Forward = v.normalized;
        flyTrigger = true;
    }
}
