using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornTest : MonoBehaviour {

    Color originColor;
    bool isClear = false;
    bool enteredOnce = false;
    float damageCD = 0;
    //每秒钟的伤害数值
    private int damageNum = 15;

    private void OnTriggerEnter(Collider other)
    {
        //if (other.name==(CharacterType.Boss.ToString())) return;
        //    originColor = other.GetComponent<MeshRenderer>().material.color;
        if (other.tag == "Player" && enteredOnce == false)
        {
            enteredOnce = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enteredOnce = false;
            //other.GetComponent<MeshRenderer>().material.color = originColor;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag(CharacterType.Player.ToString()))
        //{
        //    ChangePlayerColor(other.gameObject);
        //}
        damageCD += Time.deltaTime;
        if (damageCD >= 0.25f) {
            if (other.CompareTag(CharacterType.Player.ToString())){
                //ChangePlayerColor(other.gameObject);
                other.gameObject.GetComponent<BaseCharacter>().Health -= damageNum;
            } else if (other.CompareTag(CharacterType.Enemy.ToString())) {
                other.gameObject.GetComponent<Enemy>().HpChange(-damageNum+5, null);
            }                  
            damageCD = 0;
        }

    }

    private void ChangePlayerColor(GameObject player) {
        //Color playerColor = player.GetComponent<MeshRenderer>().material.color;
        //if (playerColor == Color.red) {
        //    isClear = true;
        //}
        //if (playerColor == originColor)
        //{
        //    isClear = false;
        //}
        //if (!isClear)
        //{
        //    player.GetComponent<MeshRenderer>().material.color = Color.Lerp(playerColor, Color.red, 40 * Time.deltaTime);
        //}
        //else {
        //    player.GetComponent<MeshRenderer>().material.color = Color.Lerp(playerColor, originColor, 40 * Time.deltaTime);
        //}
        
    }
}
