using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour {
    protected Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetAnimator(AnimType type)
    {
        if (type == AnimType.Attack)
        {
            anim.SetBool(AnimType.Run.ToString(), false);
            anim.SetTrigger(AnimType.Attack.ToString());
        }
        else if (type == AnimType.Run)
        {
            anim.SetBool(AnimType.Run.ToString(), true);
        }
    }

    public void OnAttackAnimFinish()
    {
        transform.parent.GetComponent<Enemy>().OnAttackAnimFinish();
    }
}
