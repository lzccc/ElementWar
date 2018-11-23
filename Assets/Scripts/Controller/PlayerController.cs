using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {

    public static PlayerController player;
    [Tooltip("存放所有的普通攻击模型")]
    public List<GameObject> skillPrefab = new List<GameObject>();
    /// <summary>
    /// 将技能与ID对应填入字典中
    /// </summary>
    public Dictionary<int, GameObject> skillList = new Dictionary<int, GameObject>();
    /// <summary>
    /// 技能等级
    /// </summary>
    public int[] skillLv = new int[]
    {
        1,1,1,1
    };
    [Header("法杖的顶点")]
    public Transform startSendPos;
    public int skillId = 1;
    public int firstSkillId = 0;
    public int secondSkillId = 0;
    public MagicBeamScript BeamScript;
    public GameObject bloodMask;
    private void Awake()
    {
        player = this;
        for (int i = 0; i < skillPrefab.Count; i++)
        {
            skillList.Add(int.Parse(skillPrefab[i].name), skillPrefab[i]);
        }
    }
    private void Start()
    {
        EventManager.AllEvent.OnEasyTouchEvent+=SetEasyTouch;
        EventManager.AllEvent.OnPlayerDataEvent += OnHpShowBloodMask;
    }
    private void OnDestroy()
    {
        EventManager.AllEvent.OnEasyTouchEvent -= SetEasyTouch;
        EventManager.AllEvent.OnPlayerDataEvent -= OnHpShowBloodMask;
    }
    GameObject skillgo;
    Skill skill;
    private Enemy enemy;//能量球的命中目标
    public void Attack()
    {
        //得到角色的朝向
        //从这个方向释放出技能
        //生成一个特效
        //将方向，技能初始化给到特效
        skillgo = Instantiate(skillList[skillId], startSendPos.position,Quaternion.identity);
        skill = skillgo.GetComponentInChildren<Skill>();
        //ObjPoolManager.objpoolmanager.GetPoolsForName(((PoolType)skillId).ToString()).Active(); 
        //skill.transform.position = transform.position;
        Vector3 forward = player.gameObject.transform.forward;
        if (skillId >4) //释放的是组合技能，将两个技能的等级传入
        {
            int firstLevel = skillLv[Mathf.Min(firstSkillId-1, secondSkillId-1)];//得到第一个技能的等级
            int secondLevel = skillLv[Mathf.Max(firstSkillId-1, secondSkillId-1)];//得到第二个技能的等级
            Skill[] sList = skillgo.GetComponentsInChildren<Skill>();
            for (int i = 0; i < sList.Length; i++)
            {
                sList[i].InitSkillResult(firstLevel, secondLevel);
            }
        }
        else
        {
            skill.InitLv(skillLv[skillId - 1]);//如果不是组合技能则初始化技能等级
        }
        if (!skill.isBeam)//如果不是激光
        {
            BeamScript.SendBeam(null);
            skillgo.GetComponent<SkillUseKind>().UseSkill(forward);
        }
        else
        {
            BeamScript.SendBeam(skill);
        }
    }

    public void ChangeSkill(int id)
    {
        //更换技能id
        skillId = id;
        //更换攻击冷却
        float cooltime = skillList[skillId].GetComponentInChildren<Skill>().coolTime;
        FireController.fireController.ChangeCool(cooltime);
    }
    /// <summary>
    /// 升级技能，对应id1234火土木风
    /// </summary>
    /// <param name="id"></param>
    public bool SkillLevelUp(int id)
    {
        if (!BasePlayerAttribute.instance.CanUpSkill())
        {
            return false;
        }
        skillLv[id - 1]++;
        if(skillLv[id - 1] > 2)
        {
            skillLv[id - 1] = 2;
        }
        //扣除对应碎片数
        BaseCharacter.player.RemoveFragment(BasePlayerAttribute.instance.SkillUpNeedFragment[BasePlayerAttribute.instance.nowSkillLvNum]);
        return true;
    }
    /// <summary>
    /// 将选中的IDlist传入，使用元素
    /// </summary>
    /// <param name="skillIdList"></param>
    public void InputSkillId(List<int> skillIdList)
    {
        if (skillIdList.Count <= 1)
        {
            skillId = skillIdList[0];
            firstSkillId = 0;secondSkillId = 0;
        }
        else
        {
            if (skillIdList[0] > skillIdList[1])
            {
                skillId = GetDoubleSkillId(skillIdList[1], skillIdList[0]);
                firstSkillId = skillIdList[1];
                secondSkillId = skillIdList[0];
            }
            else
            {
                skillId = GetDoubleSkillId(skillIdList[0], skillIdList[1]);
                firstSkillId = skillIdList[0];
                secondSkillId = skillIdList[1];
            }
        }
        //调整CD
        float cooltime = skillList[skillId].GetComponentInChildren<Skill>().coolTime;
        FireController.fireController.ChangeCool(cooltime);
        //FireController.isFire = false;
    }
    /// <summary>
    /// 将选中的ID传入，使用元素
    /// </summary>
    /// <param name="skillId"></param>
    public void InputSkillId(int skillId)
    {
        this.skillId = skillId;
        firstSkillId = 0; secondSkillId = 0;
        float cooltime = skillList[skillId].GetComponentInChildren<Skill>().coolTime;
        FireController.fireController.ChangeCool(cooltime);
        //FireController.isFire = false;
    }
    /// <summary>
    /// 存入时需要排序，前者小后者大，得到组合技能ID
    /// </summary>
    /// <param name="skillId1"></param>
    /// <param name="skillId2"></param>
    /// <returns></returns>
    public int GetDoubleSkillId(int skillId1,int skillId2)
    {
        if (skillId1 == 1)
        {
            if (skillId2 == 2)
            {
                return 10;
            }
            else if (skillId2 == 3)
            {
                return 11;
            }
            else
            {
                return 12;
            }
        }
        else if (skillId1 == 2)
        {
            if (skillId2 == 3)
            {
                return 13;
            }
            else return 14;
        }
        else return 15;
    }

    /// <summary>
    /// 设置操作轮盘是否启动
    /// </summary>
    /// <param name="b"></param>
    public void SetEasyTouch(bool b)
    {
        EasyTouch.SetEnabled(b);
        if (b)
        {
            GameObject.Find("JoystickManager/FireJoystick").GetComponent<EasyJoystick>().areaColor = Color.white;
            GameObject.Find("JoystickManager/FireJoystick").GetComponent<EasyJoystick>().touchColor = Color.white;
        }
        else {
            GameObject.Find("JoystickManager/FireJoystick").GetComponent<EasyJoystick>().areaColor = Color.gray;
            GameObject.Find("JoystickManager/FireJoystick").GetComponent<EasyJoystick>().touchColor = Color.gray;
        }
    }
    /// <summary>
    /// 血量变化时判断是否要出现血雾效果
    /// </summary>
    public void OnHpShowBloodMask(PlayerDataType type)
    {
        if (bloodMask == null) return;
        if (type == PlayerDataType.Hp)
        {
            if (BaseCharacter.player.Health < (BasePlayerAttribute.instance.maxHealth / 5f))
            {
                bloodMask.SetActive(true);
            }else if (bloodMask.activeSelf)
            {
                bloodMask.SetActive(false);
            }
        }
    }
}
