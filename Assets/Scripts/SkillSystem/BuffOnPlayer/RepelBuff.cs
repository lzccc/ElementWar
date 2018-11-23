using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物的击退buff
/// </summary>
public class RepelBuff : Buff {
    private GameObject player;
    private Vector3 forward;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(CharacterType.Player.ToString());
    }
    private void Start()
    {
        buffId = 3;
    }

    public override void UseBuff(GameObject target)
    {
        forward = transform.position - target.transform.position;
        player.GetComponent<BaseCharacter>().BeRepel(forward.normalized, repelSpeed, buffTime);
        player.GetComponent<BaseCharacter>().buffList.Add(this);
    }

    public override void ReBuff()
    {
        Destroy(this);
    }
}
