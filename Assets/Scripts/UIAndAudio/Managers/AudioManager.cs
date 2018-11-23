using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//大分类 ：音乐 音效
public enum SoundType { music, sound };  

//每个音乐和对应的存放路径
public enum AudioType
{
    //BGM，循环
    BeginGame,         //开始游戏界面

    NewVillage,        //游戏中，新手村
    PlayingGame,       //游戏中，正常模式
    PlayingGameNervous,//游戏中，激烈时
    Battle,            //游戏中，和Boss战斗时

    Awake,             //觉醒
    FinalWin,          //通关

    //音效，不可循环
    PlayerReleaseSkill,//玩家释放技能
    ButtonYes,         //确定按钮
    ButtonClose,       //关闭按钮
    ButtonNormal,      //其他按钮
    Arrow,             //射箭
    BossYellSound,     //boss吼叫
    BossNoseSound,     //boss鼻子发出的怒气
    BossFeet,          //boss跺脚
    BossHitWall,       //boss撞墙
    BossDead,          //boss死亡
    EnemyExplode,      //怪物自爆
    EnemyDead,         //怪物死亡
    FragmentCollect,   //收集碎片
    SelectBuff         //选择buff 
}


/// <summary>
/// 音效管理器
/// </summary>
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;                         //单例
    public static AudioManager Instance
    {
        set
        {
            instance = value;
        }
        get
        {
            return instance;
        }
    }

    private AudioSource audioSource;                             //播放声音的组件
    private Dictionary<AudioType, string> audioPathDict;         //存放声音类型和路径
    private Dictionary<AudioType, AudioSource> audioClipDict;    //缓存音频文件,不需要再加载

    public static bool isExist = false;

    //音乐、音效是否停止
    public static bool musicStop = false;
    public static bool soundStop = false;

    /// <summary>
    /// 初始化
    /// </summary>
    private void Awake()
    {
        Instance = this;
        if (!isExist)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            isExist = true;
        }
        else
        {
            Destroy(this);
            Instance = GameObject.Find("AudioManager(Clone)").GetComponent<AudioManager>();
        }

        audioPathDict = new Dictionary<AudioType, string>()
        {
            { AudioType.BeginGame, "WXY/Musics/bgm/BeginGame"},
            { AudioType.PlayingGame, "WXY/Musics/bgm/PlayingGame"},
            { AudioType.Battle, "WXY/Musics/bgm/Battle"},
            { AudioType.PlayingGameNervous, "WXY/Musics/bgm/PlayingGameNervous"},
            { AudioType.Awake, "WXY/Musics/bgm/Awake"},
            { AudioType.NewVillage, "WXY/Musics/bgm/NewVillage"},
            { AudioType.FinalWin, "WXY/Musics/bgm/FinalWin"},


            { AudioType.PlayerReleaseSkill, "WXY/Musics/PlayerReleaseSkill"},
            { AudioType.ButtonYes, "WXY/Musics/ButtonYes"},
            { AudioType.ButtonClose, "WXY/Musics/ButtonClose"},
            { AudioType.ButtonNormal, "WXY/Musics/ButtonNormal"},
            { AudioType.Arrow, "WXY/Musics/Arrow"},
            { AudioType.BossYellSound, "WXY/Musics/BossYellSound"},
            { AudioType.BossNoseSound, "WXY/Musics/BossNoseSound"},
            { AudioType.BossFeet, "WXY/Musics/BossFeet"},
            { AudioType.BossHitWall, "WXY/Musics/BossHitWall"},
            { AudioType.BossDead, "WXY/Musics/BossDead"},
            { AudioType.EnemyExplode, "WXY/Musics/EnemyExplode"},
            { AudioType.EnemyDead, "WXY/Musics/EnemyDead"},
            { AudioType.FragmentCollect, "WXY/Musics/FragmentCollect"},
            { AudioType.SelectBuff, "WXY/Musics/SelectBuff"}
        };
        audioSource = gameObject.AddComponent<AudioSource>();
        audioClipDict = new Dictionary<AudioType, AudioSource>();
        GetAllAudioClip(); //加载全部的音效，存放在缓冲池中

    }

    /// <summary>
    /// 在开始阶段，加载全部的音效
    /// </summary>
    private void GetAllAudioClip()
    {
        foreach (AudioType type in Enum.GetValues(typeof(AudioType)))
        {
            GetAudioClip(type);
        }
    }

    /// <summary>
    /// 从Resources中获取音频文件，放进缓存列表中
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private AudioSource GetAudioClip(AudioType type)
    { 
        if (!audioClipDict.ContainsKey(type))
        {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            temp.clip = Resources.Load(audioPathDict[type]) as AudioClip;
            audioClipDict.Add(type, temp);
        }
        return audioClipDict[type];
    }


    /// <summary>
    /// Start()
    /// </summary>
    private void Start()
    {
        //Instance.PlayMusic(AudioType.BackgroundMusic);

    }

    /// <summary>
    /// 关闭背景音乐
    /// </summary>
    public void StopMusic()
    {
        musicStop = true;
        audioSource.Stop();
    }

    /// <summary>
    /// 打开背景音乐
    /// </summary>
    public void PlayMusic()
    {
        musicStop = false;
        audioSource.Play();
    }

    /// <summary>
    /// 切换背景音乐
    /// </summary>
    /// <param name="type"></param>
    public void ChangeMusic(AudioType type)
    {
        StopMusic();
        PlayMusic(type);
    }

    /// <summary>
    /// 打开指定类型的背景音乐，默认循环播放，只有一种背景音乐
    /// </summary>
    public void PlayMusic(AudioType type)
    {
        musicStop = false;
        audioSource = audioClipDict[type];
        audioSource.clip.LoadAudioData();
        audioSource.loop = true;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }

    /// <summary>
    /// 播放指定类型的音效
    /// </summary>
    /// <param name="type"></param>
    public void PlaySound(AudioType type)
    {
        soundStop = false;
        AudioSource temp = gameObject.AddComponent<AudioSource>();
        temp = audioClipDict[type];
        temp.clip.LoadAudioData();
        temp.loop = false;
        temp.volume = 1.0f;
        temp.Play();
    }

    /// <summary>
    /// 关闭所有音效
    /// </summary>
    public void StopAllSound()
    {
        soundStop = true;
        foreach (AudioType type in Enum.GetValues(typeof(AudioType)))
        {
            if (type != AudioType.BeginGame && type != AudioType.PlayingGame && type!=AudioType.Battle)
            {
                audioClipDict[type].mute = true;
            }
        }
    }

    /// <summary>
    /// 打开所有音效
    /// </summary>
    public void OpenAllSound()
    {
        soundStop = false;
        foreach (AudioType type in Enum.GetValues(typeof(AudioType)))
        {
            if (type != AudioType.BeginGame && type != AudioType.PlayingGame && type != AudioType.Battle)
            {
                audioClipDict[type].mute = false;
            }
        }
    }

    public bool setMusic = false;
    public bool setSound = false;
    /// <summary>
    /// 设置 音乐或音效：关闭时换成关闭图片，开启时换成开启图片
    /// </summary>
    public void SetMusicOrSouond(Image ImageClosed, string soundType)
    {
        PlaySound(AudioType.ButtonNormal);

        if (soundType == "music")
        {
            if (!setMusic && !musicStop)
            {
                StopMusic();
                ImageClosed.color = new Color(ImageClosed.color.r, ImageClosed.color.g, ImageClosed.color.b, 1);

            }
            else
            {
                PlayMusic();
                ImageClosed.color = new Color(ImageClosed.color.r, ImageClosed.color.g, ImageClosed.color.b, 0);
            }
            setMusic = !setMusic;
        }

        if (soundType == "sound")
        {
            if (!setSound && !soundStop)
            {
                StopAllSound();
                ImageClosed.color = new Color(ImageClosed.color.r, ImageClosed.color.g, ImageClosed.color.b, 1);
            }
            else
            {
                OpenAllSound();
                ImageClosed.color = new Color(ImageClosed.color.r, ImageClosed.color.g, ImageClosed.color.b, 0);
            }
            setSound = !setSound;
        }
    }
}
