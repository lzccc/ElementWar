using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUseKind : MonoBehaviour {

    [Tooltip("同时发射的数量")]
    public int ballNum=1;
    [Tooltip("每个球的偏移角度")]
    public float offsetX=15;

    private float time;
    Skill skill;
    private void Start()//将自身的特效总共需要的时间算出来，时间到后这个物体直接销毁
    {
        skill = GetComponentInChildren<Skill>();
        time = skill.flyTime+1;
        float maxTime=0;
        for (int i = 0; i < skill.resultList.Count; i++)
        {
            if (skill.resultList[i].skillKeepTime_Static > maxTime&& skill.resultList[i].skillKeepTime_Static!=999)
            {
                maxTime = skill.resultList[i].skillKeepTime_Static;
            }
        }
        if (skill.isBeam)
        {
            time = maxTime + 1;
        }else
            time += maxTime;
    }
    public virtual void UseSkill(Vector3 forward)
    {
        if (ballNum == 1)//只有一个球
        {
            GameObject go = transform.Find("ball").gameObject;
            float sizeScale = go.GetComponent<Skill>().sizeScale;
            go.transform.localScale *= sizeScale;
            go.GetComponent<SkillPrefabController>().InitTarget(forward);
        }
        else
        {
            float offsetAngle;
            if (ballNum % 2 == 0)
            {
                offsetAngle = -(ballNum / 2 * offsetX)+offsetX/2;
            }
            else
            {
                offsetAngle = -(ballNum / 2 * offsetX);
            }
            GameObject ball; GameObject go;
            ball = transform.Find("ball").gameObject;
            ball.GetComponent<SkillPrefabController>().InitTarget(RotationMatrix(forward, offsetAngle));
            for (int i = 1; i < ballNum; i++)
            {
                go = Instantiate(ball, transform);
                go.GetComponent<SkillPrefabController>().InitTarget(RotationMatrix(forward, offsetAngle+i*offsetX));
            }
        }
    }
    private void Update()//如果时间到了自动销毁
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 旋转向量，使其方向改变，大小不变
    /// </summary>
    /// <param name="v">需要旋转的向量</param>
    /// <param name="angle">旋转的角度</param>
    /// <returns>旋转后的向量</returns>
    private Vector3 RotationMatrix(Vector3 v, float angle)
    {
        float x = v.x;
        float y = v.z;
        float sin = Mathf.Sin(Mathf.PI * angle / 180);
        float cos = Mathf.Cos(Mathf.PI * angle / 180);
        float newX = x * cos + y * sin;
        float newY = x * -sin + y * cos;
        return new Vector3(newX,v.y, newY);
    }
}
