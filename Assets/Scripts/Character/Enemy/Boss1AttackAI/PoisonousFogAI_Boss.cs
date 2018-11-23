using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousFogAI_Boss : MonoBehaviour {
    [Tooltip("毒雾半径")]
    public float radius=5;
    [Tooltip("毒雾伤害0.5s一次")]
    public int harmNum = 10;

    [Tooltip("伤害计时间隔")]
    public float coolTime=0.5f;
    private float coolTimer;

    [Tooltip("存在时间")]
    public float liftTime=6;
    private GameObject player;
    private void Awake()
    {
        coolTimer = coolTime;
        player = GameObject.FindGameObjectWithTag(CharacterType.Player.ToString());

    }
    private void Update()
    {
        liftTime -= Time.deltaTime;
        if (liftTime <= 0)
        {
            Destroy(gameObject);
        }
        coolTimer -= Time.deltaTime;
        if (coolTimer <= 0)//表示触发一次毒素伤害，玩家在范围内都受到伤害
        {
            coolTimer = coolTime;
            float dis = Vector3.Distance(player.transform.position, transform.position);
            if (dis <= radius)//玩家在范围内
            {
                BaseCharacter.player.removeHealth(harmNum);//扣血
            }
        }
    }

}
