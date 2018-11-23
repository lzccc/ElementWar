using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEndTrigger : MonoBehaviour {

    [Header("刷怪点")]
    public StorySummonTerrainController sstc;
    private GameObject characterManager;
    [Header("通关传送阵")]
    public GameObject portal;
    [Header("传送阵生成地点")]
    public GameObject portalTarget;
    private bool canCount=false;
    public GameObject CharacterManager
    {
        get
        {
            if (characterManager == null)
            {
                characterManager = GameObject.Find("CharacterManager");
            }
            return characterManager;
        }
    }
    int counter = 0;
    private void Update()
    {
        if (canCount)
        {
            if (CharacterManager.transform.childCount <= 1)//表示清光怪了
            {
                GetComponent<BoxCollider>().enabled = false;
                EventManager.AllEvent.OnMesShowEventUse("恭喜你成功击杀了牛魔王分身,你获得了<color=red>一次强化的机会</color>,进入传送门即可前往下一个世界");
                BattlePanel.Instance.MaskFadeIn();
                //保存数据
                SaveAndLoad.SaveGameData(BaseCharacter.player);
                SaveAndLoad.saveCurrentLevel(BasePlayerAttribute.instance.nowScene + 1);
                //生成传送门
                Instantiate(portal, portalTarget.transform.position, portal.transform.rotation);
                canCount = false;
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!(other.CompareTag(CharacterType.Player.ToString())))return;
        canCount = true;
        GetComponent<BoxCollider>().enabled = false;
        
    }
}
