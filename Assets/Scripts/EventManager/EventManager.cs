using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public static EventManager AllEvent;
    private void Awake()
    {
        AllEvent = this;
    }
    #region 消息事件，发送一条消息
    public delegate void OnMesShowDelegate(string str);
    public event OnMesShowDelegate OnMesShowEvent;
    //显示一条系统消息
    public void OnMesShowEventUse(string msg)
    {
        OnMesShowEvent(msg);
    }



    #endregion
    #region 玩家数据变动的事件
    public delegate void OnPlayerDataChangeDelegate(PlayerDataType type);
    public event OnPlayerDataChangeDelegate OnPlayerDataEvent;
    /// <summary>
    /// 玩家数据发生变动的事件
    /// </summary>
    /// <param name="type"></param>
    public void OnPlayerDataChange(PlayerDataType type)
    {
        OnPlayerDataEvent(type);
    }
    #endregion

    #region 技能读条设置事件
    public delegate void OnSkillProcessDelegaet(bool b);
    public event OnSkillProcessDelegaet OnSkillProcessEvent;
    /// <summary>
    /// 技能读条设置事件
    /// </summary>
    /// <param name="b"></param>
    public void OnSkillProcessSet(bool b)
    {
        OnSkillProcessEvent(b);
    }
    #endregion
    #region 操作轮盘激活的设置
    public delegate void OnEasyTouchDelegate(bool b);
    public event OnEasyTouchDelegate OnEasyTouchEvent;
    /// <summary>
    /// 操作轮盘激活的设置
    /// </summary>
    /// <param name="b"></param>
    public void OnEasyTouchSet(bool b)
    {
        OnEasyTouchEvent(b);
    }
    #endregion

    #region 游戏结束事件
    public delegate void OnGameFinishDelegate(GameFinishType type);
    public event OnGameFinishDelegate OnGameFinishEvent;
    /// <summary>
    /// 游戏结束事件
    /// </summary>
    /// <param name="b"></param>
    public void OnGameFinish(GameFinishType type)
    {
        OnGameFinishEvent(type);
    }
    #endregion
    #region 需要摇晃镜头时调用
    public delegate void OnCameraShakeDelegate(float a,float b,float c);
    public event OnCameraShakeDelegate OnCameraShakeEvent;
    /// <summary>
    /// 需要摇晃镜头时调用
    /// </summary>
    /// <param name="shakeLv">震动等级1开始递增</param>
    /// <param name="shakeTime">震动时间</param>
    /// <param name="shakeFPS">震动的FPS一般为45</param>
    public void OnCameraShake(float shakeLv, float shakeTime, float shakeFPS)
    {
        OnCameraShakeEvent(shakeLv, shakeTime, shakeFPS);
    }
    #endregion

    #region boss飘字事件
    public delegate void OnBossMsgDelegate(string newText, ElementAttributeType type);
    public event OnBossMsgDelegate OnBossMsgEvent;
    /// <summary>
    /// boss飘字事件
    /// </summary>
    /// <param name="newText"></param>
    /// <param name="type"></param>
    public void OnBossMsgShow(string newText, ElementAttributeType type)
    {
        OnBossMsgEvent(newText,type);
    }
    #endregion
}
