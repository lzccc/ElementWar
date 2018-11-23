using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class BeginGamePanel : PanelBase {

    //定义面板上的组件
    private Button tapToStart;

    private Button setBtn;
    private Image setBg;
    private Button musicBtn;
    private Button soundBtn;
    private Button helpBtn;

    private Button newGameBtn;
    private Button continueGameBtn;
    private Button infiniteBtn;

    //布尔条件
    public static bool tapToStartMove = false;
    public bool startGameActive = false;
    public static bool setPageMove = false;
    public bool setPageActive = false;
    public bool continueGameActive = false;

    //音乐和音效关闭时的图像
    private Image musicClose;
    private Image soundClose;


    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioType.BeginGame);
    }

    #region 生命周期

    public override void Init(params Object[] args)
    {
        base.Init(args);
        //初始化预设体路径和面板层级
        skinPath = "BeginGamePanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;

        setBg = skinTrans.Find("setBg").GetComponent<Image>();


        tapToStart = skinTrans.Find("start").GetComponent<Image>().GetComponent<Button>();
        setBtn = skinTrans.Find("setBtn").GetComponent<Button>();
        musicBtn = GameObject.Find("music").GetComponent<Button>();
        soundBtn = GameObject.Find("sound").GetComponent<Button>();
        //helpBtn = GameObject.Find("help").GetComponent<Button>();

        musicClose = GameObject.Find("musicClose").GetComponent<Image>();
        soundClose = GameObject.Find("soundClose").GetComponent<Image>();

        newGameBtn = GameObject.Find("newGame").GetComponent<Button>();
        continueGameBtn = GameObject.Find("continueGame").GetComponent<Button>();
        infiniteBtn = GameObject.Find("infinite").GetComponent<Button>();

        ////如果第一次玩游戏，则显示新游戏，否则还显示继续游戏
        if (!SaveAndLoad.CheckSavedData())
        {
            ChangeButtonColor(continueGameBtn,false);
            continueGameBtn.enabled = false;
            PlayerPrefs.SetString("first", "true");
        }
        else
        {
            if (SaveAndLoad.loadCurrentLevel() < 2)
            {
                ChangeButtonColor(continueGameBtn, false);
                continueGameBtn.enabled = false;
                PlayerPrefs.SetString("first", "true");
            }
            else
            {
                ChangeButtonColor(continueGameBtn, true);
                continueGameBtn.enabled = true;
            }
        }

        if (!PlayerPrefs.HasKey("InfiniteTrigger"))
        {
            ChangeButtonColor(infiniteBtn, false);
            infiniteBtn.enabled = false;
        }
        else {
            ChangeButtonColor(infiniteBtn, true);
            infiniteBtn.enabled = true;
        }

       

        tapToStart.onClick.AddListener(OnTapToStart);
        setBtn.onClick.AddListener(OnSetPage);
        musicBtn.onClick.AddListener(delegate () { AudioManager.Instance.SetMusicOrSouond(musicClose, SoundType.music.ToString()); });
        soundBtn.onClick.AddListener(delegate () { AudioManager.Instance.SetMusicOrSouond(soundClose, SoundType.sound.ToString()); });
       // helpBtn.onClick.AddListener(OpenHelpPanel);
        continueGameBtn.onClick.AddListener(ContinueGame);
        newGameBtn.onClick.AddListener(BeginGame);
        infiniteBtn.onClick.AddListener(InfiniteMode);
    }
    #endregion

    /// <summary>
    /// 改变按钮颜色
    /// </summary>
    /// <param name="btn"></param>
    private void ChangeButtonColor(Button btn, bool active) {
        if (!active) {
            btn.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
       
        else {
            Color c = btn.GetComponent<Image>().color;
            btn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
            
    }

    /// <summary>
    /// 点击 新游戏
    /// </summary>
    private void BeginGame()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
        SaveAndLoad.saveCurrentLevel(1);
        //TODO 要改
        //如果是第一次，则进入剧情介绍页面，否则直接进入战斗界面
        if (!PlayerPrefs.HasKey("first"))
      // if(true)
        {
            PanelMgr.instance.ClosePanel("BeginGamePanel");
            PanelMgr.instance.OpenPanel<StoryIntroduction>("");
        }
        else
        {
            Global.newGame = true;
            Global.loadName = "Scene_01";
            SceneManager.LoadScene("Loading");
        }
    }
    /// <summary>
    /// 继续游戏
    /// </summary>
    private void ContinueGame()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
        Global.newGame = false;
        int level= SaveAndLoad.loadCurrentLevel();

        Global.loadName = "Scene_0"+level;
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 点击进入无限模式
    /// </summary>
    private void InfiniteMode() {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
        Global.loadName = "Infinite";
        SceneManager.LoadScene("Loading");
    }


    IEnumerator WaitSoundPlay() {
        yield return new WaitForSecondsRealtime(0.5f);
        AudioManager.Instance.StopAllSound();
    }

    private void OnSetPage()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal); 
        if(!startGameActive)
            setPageMove = true;
    }

    private void setBgMove() {
        if (setPageMove)
        {
            if (!setPageActive)
            {
                setBg.transform.position += new Vector3(5.7f,0, 0);
                if (setBg.transform.position.x >= 0)
                {
                    setPageMove = false;
                    setPageActive = true;
                }
            }
            else
            {
                setBg.transform.position -= new Vector3(5.7f,0,0);
                if (setBg.transform.position.x <= (-1)*setBg.rectTransform.rect.width)
                {
                    setPageMove = false;
                    setPageActive = false;
                }
            }
        }
    }


    /// <summary>
    /// tap to start ,弹出新游戏和继续游戏的选项卡
    /// </summary>
    private void OnTapToStart()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
        setPageMove = true;  
    }
 

    public override void Update()
    {
        setBgMove();
    }
}
