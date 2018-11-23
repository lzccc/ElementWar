using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerAttribute : MonoBehaviour
{


    /// <summary>
    /// 这是玩家固定数据的对象
    /// </summary>
    public static BasePlayerAttribute instance;
    /// <summary>
    /// 123分别表示123层
    /// </summary>
    public int nowScene=1;
    public bool inInfinite=false;//是否是无尽模式
    [Header("最大血量")]
    public int maxHealth = 1750;
    [Header("最大愤怒值")]
    public int maxAngerValue = 100;
    [Header("额外buff的等级,0-5分别为长生，勇猛，清醒，强壮，闪避，迅捷")]
    public int[] ExtraBuffLv = new int[]
    {
        0,0,0,0,0,0
    };
    /// <summary>
    /// 额外buff的属性,等级与每级属性0-5分别为长生，勇猛，清醒，强壮，闪避，迅捷
    /// 
    /// </summary>
    [Header("额外buff的属性,等级与每级属性")]
    public int[][] ExtraBuff = new int[][]
    {
        new int[]{0,30,50,70,80 },
        new int[]{0,30,50,70,80 },
        new int[]{0,0,0,0,0 },
        new int[]{0,25,40,50,60 },
        new int[]{0,20,30,35,40 },
        new int[]{0,12, 20, 26, 32 }
    };
    [Header("额外buff的最高等级")]
    public int[] ExtraBuffMaxLv = new int[]
    {
        4,4,1,4,4,4
    };
    [Header("元素技能的每级需要的碎片数量")]
    [HideInInspector]
    public int[] SkillUpNeedFragment = new int[]
    {
        100,150,200,250,999999
    };
    [Header("当前升级的技能数量")]
    public int nowSkillLvNum=0;
    [Header("清醒")]
    public bool isClear = false;
    [Header("击杀怪物的数量")]
    public int killNum=0;
    /// <summary>
    /// 允许技能读条显示
    /// </summary>
    private bool canShow = true;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        EventManager.AllEvent.OnSkillProcessEvent += SetSkillProcess;
        if (!inInfinite&&nowScene==1)//新手村背景音乐
        {
            AudioManager.Instance.ChangeMusic(AudioType.NewVillage);
        }
        else//正常背景音乐
        {
            AudioManager.Instance.ChangeMusic(AudioType.PlayingGame);
        }
    }
    private void OnDestroy()
    {
        EventManager.AllEvent.OnSkillProcessEvent -= SetSkillProcess;
    }
    /// <summary>
    /// 得到技能读条显示设置
    /// </summary>
    public bool SkillProcessShow
    {
        get { return canShow; }
    }
    /// <summary>
    /// 返回是否可以升级一个技能
    /// </summary>
    /// <returns></returns>
    public bool CanUpSkill()
    {
        if(BaseCharacter.player.ElementFragment< SkillUpNeedFragment[nowSkillLvNum])
        {
            return false;
        }
        else
        {
            //BaseCharacter.player.RemoveFragment(SkillUpNeedFragment[nowSkillLvNum]);//移除对应数量的碎片
            //nowSkillLvNum++;
            return true;
        }
    }
    /// <summary>
    /// 增加2级技能个数
    /// </summary>
    public void AddSkillNum()
    {
        nowSkillLvNum++;
    }
    //永久增益buff
    /// <summary>
    /// 获得额外buff的数值，1-6分别为长生，勇猛，清醒，强壮，闪避，迅捷
    /// </summary>
    /// <returns></returns>
    public int GetBuffValueForId(int id)
    {
        return ExtraBuff[id - 1][ExtraBuffLv[id - 1]];
    }
    /// <summary>
    /// 升级对应额外buff等级，1-6分别为长生，勇猛，清醒，强壮，闪避，迅捷
    /// </summary>
    public void UpBuffLvForId(int id)
    {
        if(CanUpBuff(id))
        {
            int addNum,oldHp; 
            if (id == 1)//为长生时先减去原先的buff，然后再增加血量上限
            {
                maxHealth = (int)(maxHealth / (1 + ExtraBuff[id - 1][ExtraBuffLv[id - 1]] / 100f));//先恢复原本血量的buff
                oldHp = maxHealth;
                maxHealth= (int)(maxHealth *(1 + ExtraBuff[id - 1][ExtraBuffLv[id - 1]+1] / 100f));//增加新buff
                addNum = maxHealth - oldHp;//给当前血量增加对应数值
                BaseCharacter.player.AddHealth(addNum);
            }else if (id == 3)
            {
                isClear = true;
            }else if (id == 6)
            {
                float speed = BaseCharacter.player.speed;
                speed = (speed / (1 + ExtraBuff[id - 1][ExtraBuffLv[id - 1]] / 100f));//先恢复原本的速度
                speed= (speed * (1 + ExtraBuff[id - 1][ExtraBuffLv[id - 1]+1] / 100f));//增加新速度
                BaseCharacter.player.speed = speed;
            }
            ExtraBuffLv[id - 1]++;
        }
    }
    /// <summary>
    /// 是否可以升级对应buff,1-6分别为长生，勇猛，清醒，强壮，闪避，迅捷
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool CanUpBuff(int id)
    {
        return ExtraBuffLv[id - 1] < ExtraBuffMaxLv[id - 1];
    }
    /// <summary>
    /// 设置技能读条是否显示
    /// </summary>
    /// <param name="b"></param>
    public void SetSkillProcess(bool b)
    {
        canShow = b;
    }
    string[] buffName = new string[]
    {
        "长生",
        "勇猛",
        "清醒",
        "强壮",
        "闪避",
        "迅捷"
    };
    string info = "";
    /// <summary>
    /// 得到额外buff的当前信息
    /// 1-6分别为长生，勇猛，清醒，强壮，闪避，迅捷
    /// </summary>
    public string GetExtraBuffNowInfo(int id)
    {
        info = buffName[id-1]+" 当前等级:" + ExtraBuffLv[id - 1] + "\n";
        switch (id)
        {
            case 1:
                info += "增加生命 " + ExtraBuff[id - 1][ExtraBuffLv[id - 1]] + "%\n";
                break;
            case 2:
                info += "增加怒气恢复速度 " + ExtraBuff[id - 1][ExtraBuffLv[id - 1]] + "%\n";
                break;
            case 3:
                if(ExtraBuffLv[id - 1]==0)
                    info += "无\n";
                else
                    info += "免疫减速\n";
                break;
            case 4:
                info += "伤害增加 " + ExtraBuff[id - 1][ExtraBuffLv[id - 1]] + "%\n";
                break;
            case 5:
                info += "闪避提高 " + ExtraBuff[id - 1][ExtraBuffLv[id - 1]] + "%\n";
                break;
            case 6:
                info += "速度提高 " + ExtraBuff[id - 1][ExtraBuffLv[id - 1]] + "%\n";
                break;
        }
        return info;
    }
    /// <summary>
    /// 得到额外buff的升级面板信息
    /// 1-6分别为长生，勇猛，清醒，强壮，闪避，迅捷
    /// </summary>
    public string GetExtraBuffUpInfo(int id)
    {   
        GetExtraBuffNowInfo(id);
        if (CanUpBuff(id))
        {
            info += "下级效果:\n";
            switch (id)
            {
                case 1:
                    info += "增加生命 " + ExtraBuff[id - 1][ExtraBuffLv[id - 1] + 1] + "%";
                    break;
                case 2:
                    info += "增加怒气恢复速度 " + ExtraBuff[id - 1][ExtraBuffLv[id - 1] + 1] + "%";
                    break;
                case 3:
                    info += "免疫减速\n";
                    break;
                case 4:
                    info += "伤害增加 " + ExtraBuff[id - 1][ExtraBuffLv[id - 1] + 1] + "%";
                    break;
                case 5:
                    info += "闪避提高 " + ExtraBuff[id - 1][ExtraBuffLv[id - 1] + 1] + "%";
                    break;
                case 6:
                    info += "速度提高 " + ExtraBuff[id - 1][ExtraBuffLv[id - 1] + 1] + "%";
                    break;
            }
        }
        else
        {
            info += "已满级";
        }
        return info;
    }

    public int GetSkillUpFragment()
    {
        if (nowSkillLvNum >= 4) return 999999;
        return SkillUpNeedFragment[nowSkillLvNum];
    }
}