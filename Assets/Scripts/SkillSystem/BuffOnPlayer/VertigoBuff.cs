using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 人物眩晕buff
/// </summary>
public class VertigoBuff : Buff {
    
    private GameObject player;
    private void Start()
    {
        buffId = 2;
    }
    public override void UseBuff(GameObject player)
    {
        this.player = player;
        player.GetComponent<BaseCharacter>().isVertigo = true ;
        player.GetComponent<BaseCharacter>().buffList.Add(this);
    }

    public override void ReBuff()
    {
        player.GetComponent<BaseCharacter>().isVertigo = false; 
        Destroy(this);
    }
}
