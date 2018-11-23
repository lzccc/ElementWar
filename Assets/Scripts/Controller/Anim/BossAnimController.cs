using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimController : MonoBehaviour
{
    protected Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetAnimator(AnimType type)
    {
        if (type == AnimType.Idle)
        {
            anim.SetBool(AnimType.Run.ToString(), false);
        }
        else if (type == AnimType.Run)
        {
            anim.SetBool(AnimType.Run.ToString(), true);
        }
        else if (type == AnimType.Sprint)//使用冲刺
        {
            anim.SetBool(AnimType.Run.ToString(), false);
            anim.SetBool(AnimType.Sprint.ToString(), true);

        }else if (type == AnimType.UseSkill)//使用技能动画
        {
            anim.SetTrigger(AnimType.UseSkill.ToString());
        }
        else if (type == AnimType.Attack)//使用攻击动画
        {
            anim.SetBool(AnimType.Run.ToString(), false);
            anim.SetTrigger(AnimType.Attack.ToString());
        }else if (type == AnimType.UseSprint)//使用冲刺动画
        {
            anim.SetTrigger(AnimType.UseSprint.ToString());
        }else if (type == AnimType.Death)//使用死亡动画
        {
            anim.SetBool(AnimType.Run.ToString(), false);
            anim.SetBool(AnimType.Sprint.ToString(), false);
            anim.SetTrigger(AnimType.Death.ToString());
        }
    }
    public void StopAnim(AnimType type)
    {
        if (type == AnimType.Sprint)
        {
            anim.SetBool(AnimType.Sprint.ToString(), false);
        }
    }
}
