using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : PanelBase {

    private static BattlePanel instance;
    public static BattlePanel Instance {
        set {
            instance = value;
        }
        get {
            return instance;
        }
    }

    private Image hp;                 //血量
    private Image mp;
    //private Text levelID;           //关卡数
    private Button pauseBtn;          //暂停按钮
    public Text patchNum;             //碎片数量
    //private Button updateEleBtn;    //元素升级按钮
    private Image UpdateWindow;       //元素升级窗口
    private Image awake;              //觉醒
    private Image mask;               //遮罩，buff页面出现前使用
    private bool active = true;      //遮罩是否完全显示  
    private Image fragmentFullHint;   //碎片已满的提示
    //public Image FragmentFullHint
    //{
    //    get
    //    {
    //        if (fragmentFullHint == null)
    //        {
    //            fragmentFullHint= skin.transform.Find("hand").GetComponent<Image>();
    //        }
    //        return fragmentFullHint;
    //    }
    //}

    //mini buffs
    public Button[] miniBuffs;        //mini buff小图标
    public Image buffInfo;            //mini buff 信息框
    public Dictionary<int, bool> buffIsGet = new Dictionary<int, bool>(); //buffID 和 是否获得了buff

    private Button awakeButton;         //觉醒按钮
    private bool clickAwake = false;    //是否点击了觉醒

    private Dictionary<int, Image> elements = new Dictionary<int, Image>(); //火、土、木、风技能按钮和遮罩
    public Queue<int> queue = new Queue<int>();      //保存当前选中的技能ID

    /// <summary>
    /// 用于查看是否处于新手教程
    /// </summary>
    private PlayerLearnController playerLearn;
    private void Awake()
    {
        Instance = this;
        //碎片指示，一开始不可见
        //fragmentFullHint = 
        //Debug.Log(1);
        //黑色遮罩，一开始透明
    }
    private void Start()
    {
        UpdateHPMP(PlayerDataType.All);
        UpdatePatchNum(PlayerDataType.All);
        mask = skin.transform.Find("mask").GetComponent<Image>();
        playerLearn = GameObject.Find("Canvas").GetComponent<PlayerLearnController>();
        EventManager.AllEvent.OnPlayerDataEvent += UpdateHPMP;
        EventManager.AllEvent.OnPlayerDataEvent += UpdatePatchNum;
        fragmentFullHint = skin.transform.Find("hand").GetComponent<Image>();
        mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 0);
        SetHand(false);
    }

    /// <summary>
    /// 设置碎片提示是否显示
    /// </summary>
    /// <param name="active"></param>
    public void SetHand(bool active)
    {
        if (fragmentFullHint == null) return;
        fragmentFullHint.gameObject.SetActive(active);
    }

    public override void Update()
    {
        //测试buff
        if (Input.GetKeyDown(KeyCode.A))
        {
            //SetHand(true);
            MaskFadeIn();
        }
        if (!active)
        {
            mask.color += new Color(0, 0, 0, Time.deltaTime);
            if (mask.color.a >= 1)
            {
                mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 0);
                active = true;
                PanelMgr.instance.OpenPanel<SuccessPanel>("", true);
            }
        }

    }

    /// <summary>
    /// buff页面出现前，黑色遮罩渐入
    /// </summary>
    public void MaskFadeIn() {
        EventManager.AllEvent.OnMesShowEventUse("你获得了一次强化机会");
        active = false;
    }

    private void OnDestroy()
    {
        EventManager.AllEvent.OnPlayerDataEvent -= UpdateHPMP;
        EventManager.AllEvent.OnPlayerDataEvent -= UpdatePatchNum;
    }

    #region 生命周期
    public override void Init(params Object[] args)
    {
        base.Init(args);
        //初始化预设体路径和面板层级
        skinPath = "BattlePage";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;

        //血量、暂停、升级、碎片数量、觉醒（一开始不显示）
        hp = skinTrans.Find("HPBg").GetChild(1).GetComponent<Image>();
        mp = skinTrans.Find("MPBg").GetChild(1).GetComponent<Image>();
        pauseBtn = skinTrans.Find("pause").GetComponent<Button>();
        patchNum = skinTrans.Find("patchNum/patchNum").GetComponent<Text>();
        awake = skinTrans.Find("Elements").GetChild(0).GetComponent<Image>();
        awake.gameObject.SetActive(clickAwake);
        awakeButton = awake.GetComponent<Button>();
        awakeButton.onClick.AddListener(OpenAwake);

        //mini Buff
        miniBuffs = skinTrans.Find("miniBuffs").GetComponentsInChildren<Button>();
        buffInfo = skinTrans.Find("miniBuffs").GetChild(6).GetComponent<Image>();
        //首先加载历史buff信息：玩家已获取的buff等级和信息
        for (int i = 0; i < 6; i++)
        {
            buffIsGet.Add(i + 1, false);
        }
        for (int i = 0; i < 6; i++)
        {
            // 从本地缓存中读取ID为i+1的buff等级：是否为0级，如果是则不显示；如果不是，则获取等级和对应信息，并依次显示再左上角
            if (Global.loadName != "Scene_01" && Global.loadName!="Infinite")
            {
                SaveAndLoad.LoadBaseAttribute();
                if (BasePlayerAttribute.instance.ExtraBuffLv[i] > 0)
                {
                    DisplayMinibuff(i + 1, true);
                    if (i == 5)
                    {
                        BaseCharacter.player.speed += BaseCharacter.player.speed * ((BasePlayerAttribute.instance.GetBuffValueForId(i + 1)) / 100f);
                    }
                }
                else
                {
                    miniBuffs[i].gameObject.SetActive(false);      //一开始不可见
                    buffInfo.gameObject.SetActive(false);
                }
            }
            else {
                miniBuffs[i].gameObject.SetActive(false);      //一开始不可见
                buffInfo.gameObject.SetActive(false);
            }
        }

        //升级元素页面
        UpdateWindow = skinTrans.Find("UpdateWindow").GetComponent<Image>();
        UpdateWindow.gameObject.SetActive(false);

        //添加按钮事件
        pauseBtn.onClick.AddListener(Pause);
        //updateEleBtn.onClick.AddListener(UpdateEle);
        skinTrans.Find("patchNum").GetComponent<Button>().onClick.AddListener(UpdateEle);

        //火、土、木、风 技能按钮（获取按钮、添加事件）
        for (int i = 0; i < 4; i++)
        {
            Button tmpBtn = skinTrans.Find("Elements").GetChild(i + 1).GetComponent<Button>();
            elements.Add(i + 1, tmpBtn.transform.Find("shadow").GetComponent<Image>());
            tmpBtn.onClick.AddListener(delegate () { this.SkillClick(int.Parse(tmpBtn.name)); });
        }
        //默认技能 火
        queue.Enqueue(1);
    }
    #endregion

    /// <summary>
    /// 点击暂停
    /// </summary>
    private void Pause()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
        EventManager.AllEvent.OnEasyTouchSet(false);//关闭轮盘
        //暂停游戏
        Time.timeScale = 0;
        PanelMgr.instance.OpenPanel<PausePanel>("");
    }

    /// <summary>
    /// 打开升级元素窗口
    /// </summary>
    private void UpdateEle()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
        EventManager.AllEvent.OnEasyTouchSet(false);//关闭轮盘
        Time.timeScale = 0;
        UpdateWindow.gameObject.SetActive(true);
    }

    /// <summary>
    /// 点击技能按钮，获得技能
    /// </summary>
    /// <param name="skillName"></param>
    private void SkillClick(int skillId)
    {
        FireController.isFire = false; //取消上一个激光技能
        foreach (int i in queue)//判断前几次的点击与这次相同就不生效
        {
            if (i == skillId)
                return; 
        }
        /*** 未觉醒 ***/
        if (!clickAwake && queue.Count > 0)
            Global.ChangeShadowColor(elements[queue.Dequeue()],false);//出队，变暗
        /***  觉醒 ***/
        else if (clickAwake && queue.Count >= 2)
                Global. ChangeShadowColor(elements[queue.Dequeue()], false);//出队，变暗
        else{ }
        queue.Enqueue(skillId); //入队，高亮
        Global.ChangeShadowColor(elements[skillId], true);
        SkillGet(queue); //操作盘上具有该技能
        AudioManager.Instance.PlaySound(AudioType.PlayerReleaseSkill);//播放选择技能音效
    }

    private List<int> skillIdList = new List<int>();
    int id;
    /// <summary>
    /// 把选中的技能赋予到操作盘上
    /// </summary>
    /// <param name="skillQueue"> 队列中的内容是当前技能对应的Button们 </param>
    public void SkillGet(Queue<int> skillQueue)
    {
        if (skillQueue.Count <= 1)
        {
            skillIdList.Add(skillQueue.Peek());//得到单个技能的id
        }
        else//得到多个技能的id
        {
            foreach (int i in queue)
            {
                skillIdList.Add(i);
            }
        }
        //将技能Id传递
        PlayerController.player.InputSkillId(skillIdList);
        skillIdList.Clear();
        FireController.isFire = true; //开启被取消的激光技能
    }

    /// <summary>
    /// 更新HP或MP
    /// </summary>
    /// <param name="type"> 字符串类型，输入"hp"或"mp" </param>
    /// <param name="num">  fillAmount变化量，0-1之间的小数      </param>
    public void UpdateHPMP(PlayerDataType type)
    {
        if (type == PlayerDataType.Hp|| type == PlayerDataType.All)
        {
            hp.fillAmount = BaseCharacter.player.Health / (float)BasePlayerAttribute.instance.maxHealth;
        }
        if (type == PlayerDataType.Anger || type == PlayerDataType.All)
            mp.fillAmount = BaseCharacter.player.AngerValue/BasePlayerAttribute.instance.maxAngerValue;
    }

    /// <summary>
    /// 更新元素碎片的数量
    /// </summary>
    /// <param name="num"></param>
    public void UpdatePatchNum(PlayerDataType type) {
        if (type == PlayerDataType.Fragment || type == PlayerDataType.All)
            patchNum.text = BaseCharacter.player.ElementFragment.ToString();
    }

    /// <summary>
    /// 觉醒出现或消失
    /// </summary>
    public void AwakeAppearOrNot(bool isAppear)
    {
        awake.gameObject.SetActive(isAppear);
        if(!isAppear)
            clickAwake = isAppear;
        //觉醒消失时，若有两个技能，则出队一个
        if (!isAppear && queue.Count > 1) {
            Global.ChangeShadowColor(elements[(int)queue.Dequeue()], false);
        }
        MagicBeamScript.beamEffect.DestroyEffect();
        PlayerController.player.InputSkillId(queue.Peek()); //重新更新技能冷却
    }

    /// <summary>
    /// 点击觉醒
    /// </summary>
    private void OpenAwake()
    {
        if (playerLearn != null)//处于新手教程
        {
            playerLearn.EighthLearn();//提示觉醒站桩怪
        }
        clickAwake = true;
        awake.gameObject.SetActive(false);
        BaseCharacter.player.UseAnger(mp);
    }

    /// <summary>
    /// 将获得的mini buff的图标显示在左上角：2行3列
    /// </summary>
    /// <param name="buffID"></param>
    public void DisplayMinibuff(int buffID, bool trigger=false) {
        buffIsGet[buffID] = true; //获得buff
        int buffNum = HavedBuffNum();
        if (!trigger)
        {
            if (BasePlayerAttribute.instance.ExtraBuffLv[buffID - 1] <= 1)
            {
                if (buffNum <= 3)
                {
                    miniBuffs[buffID - 1].transform.localPosition = new Vector3((buffNum - 1) * 100 - 80, 50, 0);
                }
                else
                {
                    miniBuffs[buffID - 1].transform.localPosition = new Vector3((buffNum - 4) * 100 - 80, -50, 0);
                }
                miniBuffs[buffID - 1].gameObject.SetActive(true);
            }
        }
        else {
            if (buffNum <= 3)
            {
                miniBuffs[buffID - 1].transform.localPosition = new Vector3((buffNum - 1) * 100 - 80, 50, 0);
            }
            else
            {
                miniBuffs[buffID - 1].transform.localPosition = new Vector3((buffNum - 4) * 100 - 80, -50, 0);
            }
            miniBuffs[buffID - 1].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 已拥有的buff个数
    /// </summary>
    /// <returns></returns>
    public int HavedBuffNum() {
        int count = 0;
        for (int i=0;i<6;i++) {
            if (buffIsGet[i + 1])
                count++;
        }
        return count;
    }
}
