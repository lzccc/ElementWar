using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveController : MonoBehaviour
{
    public float keyCodeSpeed = 10;
    private Animator anim;
    public Animator Anim
    {
        get { if (anim == null)
            {
                anim = GetComponent<Animator>();
            }return anim;
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
        if (move.joystickName == "MoveJoystick")
        {
            Anim.SetBool(AnimType.Run.ToString(), false);
        }
    }

    //移动摇杆中
    void OnJoystickMove(MovingJoystick move)
    {
        if (move.joystickName != "MoveJoystick" || BaseCharacter.player.isVertigo)
        {
            return;
        }
        //获取摇杆中心偏移的坐标
        float joyPositionX = move.joystickAxis.x;
        float joyPositionY = move.joystickAxis.y;
        if (!FireController.isFire)//如果没有在攻击，则转方向
        {
            transform.LookAt(new Vector3(transform.position.x + joyPositionX, transform.position.y, transform.position.z + joyPositionY));
        }
        if (joyPositionY != 0 || joyPositionX != 0)
        {
            //设置角色的朝向（朝向当前坐标+摇杆偏移量）
            //transform.LookAt(new Vector3(transform.position.x + joyPositionX, transform.position.y, transform.position.z + joyPositionY));
            //transform.LookAt();
            //移动玩家的位置（按朝向位置移动）
            transform.Translate(Vector3.Normalize(new Vector3(joyPositionX, 0, joyPositionY)) * Time.deltaTime * BaseCharacter.player.speed, Space.World);
            Anim.SetBool(AnimType.Run.ToString(),true);
            //TODO:播放奔跑动画
        }
    }

    private void Update()
    {
        if (BaseCharacter.player.isVertigo) return;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-keyCodeSpeed * Time.deltaTime, 0, 0);
            //BeamScript.SendBeam();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(keyCodeSpeed * Time.deltaTime, 0, 0);
            //BeamScript.SendBeam();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0, keyCodeSpeed * Time.deltaTime);
            //BeamScript.SendBeam();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, 0, -keyCodeSpeed * Time.deltaTime);
            //BeamScript.SendBeam();
        }
    }
    private void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }
}
