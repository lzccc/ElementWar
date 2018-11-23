using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FireController : MonoBehaviour
{
    public static FireController fireController;
    public static bool isFire = false;

    [Header("二级技能的能量圈")]
    public GameObject beamBall;
    private MagicBeamScript beamScript;
    public MagicBeamScript BeamScript
    {
        get
        {
            if (beamScript == null)
            {
                beamScript = GetComponent<MagicBeamScript>();
            }
            return beamScript;
        }
    }
    private Animator anim;
    public Animator Anim
    {
        get
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
            return anim;
        }
    }
    private float coolTime = 0.6f;
    private float coolTimer = 0.6f;
    private void Awake()
    {
        fireController = this;
    }

    private void Update()
    {
        if (coolTimer > 0)
        {
            coolTimer -= Time.deltaTime;
            if (coolTimer <= 0)
            {
                coolTimer = 0;
            }
        }

    }

    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
    }

    //移动摇杆结束


    void OnJoystickMoveEnd(MovingJoystick move)
    {
        //停止时，角色恢复idle
        if (move.joystickName == "FireJoystick")
        {
            isFire = false;
            MagicBeamScript.beamEffect.DestroyEffect();//强行销毁所有激光
            Anim.SetBool(AnimType.Attack.ToString(), false);//转换到不攻击动画
        }
    }


    //移动摇杆中
    void OnJoystickMove(MovingJoystick move)
    {
        if (move.joystickName != "FireJoystick" || BaseCharacter.player.isVertigo)
        {
            return;
        }
        isFire = true;
        Anim.SetBool(AnimType.Attack.ToString(), true);//转换到攻击动画
        //获取摇杆中心偏移的坐标
        float joyPositionX = move.joystickAxis.x;
        float joyPositionY = move.joystickAxis.y;


        if (joyPositionY != 0 || joyPositionX != 0)
        {
            //设置角色的朝向（朝向当前坐标+摇杆偏移量）
            transform.LookAt(new Vector3(transform.position.x + joyPositionX, transform.position.y, transform.position.z + joyPositionY));
            if (coolTimer > 0)
            {
                return;
            }
            PlayerController.player.Attack();
            coolTimer = coolTime;
            //transform.gameObject.GetComponent<PlayerCharacter>().attack(new Vector3(joyPositionX, 0, joyPositionY));
            //transform.LookAt();
            //移动玩家的位置（按朝向位置移动）
            //transform.Translate(Vector3.Normalize(new Vector3(joyPositionX, 0, joyPositionY)) * Time.deltaTime * 20);
            //TODO:播放奔跑动画
        }
    }
    /// <summary>
    /// 变换冷却时间
    /// </summary>
    public void ChangeCool(float time)
    {
        coolTime = time;
        coolTimer = 0;
    }

    private void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }
}

