using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoad : MonoBehaviour
{

    // Use this for initialization
    /// <summary>
    /// 用来看本机中是否有之前存储过的游戏存档
    /// </summary>
    public static bool CheckSavedData()
    {
        return PlayerPrefs.HasKey("currentLevel");
    }

    /// <summary>
    /// 游戏每新进入一关时调用
    /// </summary>
    public static void SaveGameData(BaseCharacter baseCharacter)
    {
        SaveBaseAttribute();
        SavePlayerData(baseCharacter);
        //这句话用来存储人物目前所在的关卡，需要在人物属性里加一个变量currentLevel
        //PlayerPrefs.SetInt("currentLevel", baseCharacter.currentLevel); 
    }
    /// <summary>
    /// 调用保存场景123分别为123关卡
    /// </summary>
    /// <param name="level"></param>
    public static void saveCurrentLevel(int level)
    {
        PlayerPrefs.SetInt("currentLevel", level);
    }

    public static int loadCurrentLevel()
    {
        return PlayerPrefs.GetInt("currentLevel");
    }

    /// <summary>
    /// 存储基本人物信息
    /// </summary>
    public static void SaveBaseAttribute()
    {
        string extraBuffLv="";
        for (int i = 0; i < BasePlayerAttribute.instance.ExtraBuffLv.Length; i++)
        {
            extraBuffLv += BasePlayerAttribute.instance.ExtraBuffLv[i] + ",";
        }
        string skillLv = "";
        for (int i = 0; i < 4; i++)
        {
            skillLv += PlayerController.player.skillLv[i] + ",";
        }
        
        if (ES2.Exists(Application.persistentDataPath + "/base1Attribute.text")) {
            ES2.Delete(Application.persistentDataPath + "/base1Attribute.text");
        }
        string path = Application.persistentDataPath + "/base1Attribute.text";
        ES2.Save(BasePlayerAttribute.instance.maxHealth, path + "?tag=maxHealth");
        ES2.Save(BasePlayerAttribute.instance.nowSkillLvNum, path + "?tag=nowSkillLvNum");
        ES2.Save(extraBuffLv, path + "?tag=ExtraBuffLv");
        ES2.Save(skillLv, path + "?tag=SkillLv");
        //ES2.Save(BasePlayerAttribute.instance.ExtraBuffLv, path + "?tag=ExtraBuffLv");
        ES2.Save(BasePlayerAttribute.instance.isClear, path + "?tag=isClear");
        //ES2.Save(BasePlayerAttribute.instance.currentLevel, path + "?tag = currentLevel");
    }

    /// <summary>
    /// 存储游戏内人物其他信息
    /// </summary>
    public static void SavePlayerData(BaseCharacter baseCharacter)
    {
        if (ES2.Exists(Application.persistentDataPath + "/base1Character.text"))
        {
            ES2.Delete(Application.persistentDataPath + "/base1Character.text");
        }
        string path = Application.persistentDataPath + "/base1Character.text";
        ES2.Save(baseCharacter.Health, path + "?tag=Health");
        ES2.Save(baseCharacter.ElementFragment, path + "?tag=ElementFragment");
        //ES2.Save(baseCharacter.angerIncrease, path + "?tag=angerIncrease");
        //ES2.Save(baseCharacter.angerIncreaseTime, path + "?tag=angerIncreaseTime");
    }



    public static void SaveInfinityGameData(int group, int kill)
    {
        if (PlayerPrefs.HasKey("maxGroup"))
        {
            if (group > PlayerPrefs.GetInt("maxGroup"))
            {
                PlayerPrefs.SetInt("maxGroup", group);
            }
        }
        else
        {
            PlayerPrefs.SetInt("maxGroup", group);
        }

        if (PlayerPrefs.HasKey("maxKill"))
        {
            if (kill > PlayerPrefs.GetInt("maxKill"))
            {
                PlayerPrefs.SetInt("maxKill", kill);
            }
        }
        else
        {
            PlayerPrefs.SetInt("maxKill", kill);
        }
    }

    public static int[] LoadInfinityData()
    {
        int[] data = new int[2];
        data[0] = PlayerPrefs.GetInt("maxGroup");
        data[1] = PlayerPrefs.GetInt("maxKill");
        return data;
    }
    /// <summary>
    /// 选择读取之前存档时调用，返回值为false表示没有存档
    /// </summary>
    public static bool LoadGameData(BaseCharacter baseCharacter)
    {
        if (!CheckSavedData())
        {
            return false;
        }
        //LoadGameScene();
        LoadBaseAttribute();
        LoadBaseCharacter(baseCharacter);
        return true;
    }

    /// <summary>
    /// 加载基本信息
    /// </summary>
    public static void LoadBaseAttribute()
    {
        string[] extraBuffLvs;
        string[] skillLvs;
        string path = Application.persistentDataPath + "/base1Attribute.text";
        BasePlayerAttribute.instance.maxHealth = ES2.Load<int>(path + "?tag=maxHealth");
        BasePlayerAttribute.instance.isClear = ES2.Load<bool>(path + "?tag=isClear");
        //BasePlayerAttribute.instance.ExtraBuffLv = ES2.Load<int[]>(path + "?tag=ExtraBuffLv");
        extraBuffLvs= (ES2.Load<string>(path + "?tag=ExtraBuffLv")).Split(',');
        for (int i = 0; i < BasePlayerAttribute.instance.ExtraBuffLv.Length; i++)
        {
            BasePlayerAttribute.instance.ExtraBuffLv[i] = int.Parse(extraBuffLvs[i]);
        }
        skillLvs = (ES2.Load<string>(path + "?tag=SkillLv")).Split(',');
        for (int i = 0; i < 4; i++)
        {
            PlayerController.player.skillLv[i] = int.Parse(skillLvs[i]);
        }
        BasePlayerAttribute.instance.nowSkillLvNum = ES2.Load<int>(path + "?tag=nowSkillLvNum");
        BasePlayerAttribute.instance.nowScene = loadCurrentLevel();
        //BasePlayerAttribute.instance.currentLevel = ES2.Load<int>(path + "?tag = currentLevel");      
    }

    /// <summary>
    /// 加载其他角色信息
    /// </summary>
    /// <param name="baseCharacter"></param>
    public static void LoadBaseCharacter(BaseCharacter baseCharacter)
    {
        string path = Application.persistentDataPath + "/base1Character.text";
        baseCharacter.Health = ES2.Load<int>(path + "?tag=Health");
        baseCharacter.ElementFragment = ES2.Load<int>(path + "?tag=ElementFragment");
        //baseCharacter.angerIncrease = ES2.Load<float>(path + "?tag=angerIncrease");
        //baseCharacter.angerIncreaseTime = ES2.Load<int>(path + "?tag=angerIncreaseTime");
    }

    /// <summary>
    /// 加载战斗场景
    /// </summary>
    //public static void LoadGameScene()
    //{
    //    int currentScence = PlayerPrefs.GetInt("currentLevel");
    //    switch (currentScence)
    //    {
    //        case 1: SceneManager.LoadScene("Transcript1"); break;
    //        case 2: SceneManager.LoadScene("Transcript2"); break;
    //        case 3: SceneManager.LoadScene("Transcript3"); break;
    //    }
    //}
}
