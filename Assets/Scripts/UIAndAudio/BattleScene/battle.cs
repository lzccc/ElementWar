using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battle : MonoBehaviour {

    private GameObject panelMgr;

    // Use this for initialization
    void Awake () {
        panelMgr = Resources.Load<GameObject>("WXY/Prefabs/PanelMgr");
        Instantiate(panelMgr);
        //打开战斗界面
        PanelMgr.instance.OpenPanel<BattlePanel>("");
        //PanelMgr.instance.OpenPanel<SuccessPanel>("");

    }
    private void Start()
    {
        EventManager.AllEvent.OnGameFinishEvent += OnGameFinish;
    }
    private void OnDestroy()
    {
        EventManager.AllEvent.OnGameFinishEvent -= OnGameFinish;
    }
    /// <summary>
    /// 游戏结束事件调用的方法
    /// </summary>
    /// <param name="type"></param>
    public void OnGameFinish(GameFinishType type)
    {
        if (type == GameFinishType.Fail)
        {
            PanelMgr.instance.OpenPanel<FailPanel>("");
          //  EventManager.AllEvent.OnEasyTouchSet(false);//关闭轮盘
            //Time.timeScale = 0; //游戏暂停
        }else if (type == GameFinishType.Win)
        {
            //PanelMgr.instance.OpenPanel<SuccessPanel>("");
        }
    }
}
