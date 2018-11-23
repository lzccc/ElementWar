using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanel : PanelBase {

    //private Image equipLine;    //装备显示栏
    //private int equipNum = 6;   //装备栏数量
    //private Image[] equipments; //6种装备的图片

    private Button returnToTitle;  //返回标题界面按钮
    private Button closeBtn;       //关闭暂停页面
   // private Button help;           //帮助按钮

    private Button music;          //音乐按钮
    private Button sound;          //声音按钮
    private Image musicClose;          //音乐关闭图片
    private Image soundClose;          //声音关闭图片

    private GameObject toggleParent;    //怪物技能开关 父物体
    private Toggle[] toggles;           //怪物技能开关

    #region 生命周期

    public override void Init(params Object[] args)
    {
        base.Init(args);
        //初始化预设体路径和面板层级
        skinPath = "PausePage";
        layer = PanelLayer.Tips;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        returnToTitle = skinTrans.Find("returnToTitle").GetComponent<Button>();
        closeBtn = skinTrans.Find("close").GetComponent<Button>();
       // help = skinTrans.Find("help").GetComponent<Button>();
        music = skinTrans.Find("music").GetComponent<Button>();
        sound = skinTrans.Find("sound").GetComponent<Button>();
        musicClose = skinTrans.Find("musicClose").GetComponent<Image>();
        soundClose = skinTrans.Find("soundClose").GetComponent<Image>();

        //怪物技能开关
        toggles = skinTrans.Find("monsterSkillDisplay").GetComponentsInChildren<Toggle>();
        toggles[0].isOn = BasePlayerAttribute.instance.SkillProcessShow;  
        toggles[1].isOn = !BasePlayerAttribute.instance.SkillProcessShow;
        //开关事件
        for (int i = 0; i < 2; i++)
        {
            toggles[i].onValueChanged.AddListener(MonsterSkillDisplay);  //开关事件
        }

        //其他事件
        music.onClick.AddListener(delegate () { AudioManager.Instance.SetMusicOrSouond(musicClose, SoundType.music.ToString()); });
        sound.onClick.AddListener(delegate () { AudioManager.Instance.SetMusicOrSouond(soundClose, SoundType.sound.ToString()); });
        returnToTitle.onClick.AddListener(ReturnToTitle);
        closeBtn.onClick.AddListener(ReturnToInGame);
        //help.onClick.AddListener(HelpPage);

    }

    /// <summary>
    /// TODO 设置怪物技能提示 是否显示
    /// </summary>
    /// <param name="display"></param>
    private void MonsterSkillDisplay(bool display) {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
        if (display && toggles[1].isOn)
        {
            Debug.Log("关闭怪物技能提示！");
            //开启怪物技能提示
            EventManager.AllEvent.OnSkillProcessSet(false);
        }
        if (display && toggles[0].isOn)
        {
            Debug.Log("开启怪物技能提示！");
            //关闭怪物技能提示
            EventManager.AllEvent.OnSkillProcessSet(true);
        }
    }

    #endregion

    /// <summary>
    /// 返回标题界面
    /// </summary>
    private void ReturnToTitle()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
        AudioManager.Instance.ChangeMusic(AudioType.BeginGame);

        //保存数据
        if (Global.loadName != "Scene_01" && !BasePlayerAttribute.instance.inInfinite) {
            SaveAndLoad.SaveGameData(BaseCharacter.player);
            SaveAndLoad.saveCurrentLevel(BasePlayerAttribute.instance.nowScene);
        }
        SceneManager.LoadScene("BeginGame");
    }

    /// <summary>
    /// 返回战斗界面：即关闭暂停页面
    /// </summary>
    private void ReturnToInGame()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonClose);
        PanelMgr.instance.ClosePanel("PausePanel");

        Time.timeScale = 1;//取消暂停
        EventManager.AllEvent.OnEasyTouchSet(true);//开启轮盘
    }

    /// <summary>
    /// 显示帮助页面
    /// </summary>
    //private void HelpPage()
    //{
    //    AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
    //    PanelMgr.instance.OpenPanel<HelpPanel>("");
    //}
}
