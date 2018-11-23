using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshTest : MonoBehaviour {

    // Use this for initialization
    [Header("玩家减速百分比")]
    public int speedCutRate=50;
    [Header("怪物减速的固定数值")]
    public float speedCutRate_Enemy = 1.5f;
    private float speedCutNum;
    bool enteredOnce = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CharacterType.Player.ToString()) && enteredOnce == false)
        {
            if (BasePlayerAttribute.instance.isClear) return;//免疫减速
            if (!BaseCharacter.player.isInMarsh)
            {
                speedCutNum = BaseCharacter.player.speed * (speedCutRate / 100f);
                BaseCharacter.player.speed -= speedCutNum;
                BaseCharacter.player.isInMarsh = true;
                enteredOnce = true;
            }
        }
        else if (other.CompareTag(CharacterType.Enemy.ToString()))
        {
            if (!other.gameObject.GetComponent<Enemy>().isInMarsh)
            {
                other.gameObject.GetComponent<Enemy>().SpeedChange(-speedCutRate_Enemy);
                other.gameObject.GetComponent<Enemy>().isInMarsh = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(CharacterType.Player.ToString())&& enteredOnce)
        {
            if (BaseCharacter.player.isInMarsh)
            {
                BaseCharacter.player.speed += speedCutNum;
                BaseCharacter.player.isInMarsh = false;
            }
            enteredOnce = false;
        }
        else if (other.CompareTag(CharacterType.Enemy.ToString()))
        {
            if (other.gameObject.GetComponent<Enemy>().isInMarsh)
            {
                other.gameObject.GetComponent<Enemy>().SpeedChange(speedCutRate_Enemy);
                other.gameObject.GetComponent<Enemy>().isInMarsh = false;
            }
        }
    }

}
