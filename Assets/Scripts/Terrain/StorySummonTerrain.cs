using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StorySummonTerrain : MonoBehaviour {
    [Header("是否为默认精英怪概率，默认true概率为0.1,否则为false，预制体也需专门的")]
    public bool hasNoMutatation=true;
    [Header("第一波刷怪的数组")]
    public GameObject[] summonEnemy_First;
    [Header("第一波对应刷怪的数量")]
    public int[] summonEnemyNum_First;
    [Header("第2波刷怪的数组")]
    public GameObject[] summonEnemy_Second;
    [Header("第2波对应刷怪的数量")]
    public int[] summonEnemyNum_Second;
    [Header("第3波刷怪的数组")]
    public GameObject[] summonEnemy_Third;
    [Header("第3波对应刷怪的数量")]
    public int[] summonEnemyNum_Third;
    [Header("第4波刷怪的数组")]
    public GameObject[] summonEnemy_Fourth;
    [Header("第4波对应刷怪的数量")]
    public int[] summonEnemyNum_Fourth;
    [Header("怪物相对于召唤点的半径距离")]
    public float dis;
    [Header("每只怪物之间的角度偏移")]
    public float angle;
    Vector3 forward;

    private void Awake()
    {
        forward = transform.forward.normalized;
    }

    GameObject go;
    GameObject[] summonEnemy;
    int[] summonEnemyNum;
    /// <summary>
    /// 开始刷怪
    /// </summary>
    public void StartSummon(Transform characterManager,int nowNum)
    {
        if (nowNum == 0)
        {
            summonEnemy = summonEnemy_First;
            summonEnemyNum = summonEnemyNum_First;
        }
        else if (nowNum == 1)
        {
            summonEnemy = summonEnemy_Second;
            summonEnemyNum = summonEnemyNum_Second;
        }
        else if (nowNum == 2)
        {
            summonEnemy = summonEnemy_Third;
            summonEnemyNum = summonEnemyNum_Third;
        }
        else
        {
            summonEnemy = summonEnemy_Fourth;
            summonEnemyNum = summonEnemyNum_Fourth;
        }
        for (int i = 0, num = 0; i < summonEnemy.Length; i++)
        {
            for (int j = 0; j < summonEnemyNum[i]; j++)
            {
                if (hasNoMutatation)
                {
                    go = ObjPoolManager.objpoolmanager.GetPoolsForName(summonEnemy[i].name).Active();
                    go.GetComponent<NavMeshAgent>().Warp(RotationMatrix(forward, angle * num) + transform.position);
                    go.transform.SetParent(characterManager);
                }
                else
                {
                    go = Instantiate(summonEnemy[i], characterManager);
                    go.GetComponent<NavMeshAgent>().Warp(RotationMatrix(forward, angle * num) + transform.position);
                    go.name = summonEnemy[i].name;
                }
                num++;
            }
        }
    }






    /// <summary>
    /// 计算矩阵的角度公式
    /// </summary>
    /// <param name="v"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Vector3 RotationMatrix(Vector3 v, float angle)
    {
        float x = v.x;
        float y = v.z;
        float sin = Mathf.Sin(Mathf.PI * angle / 180);
        float cos = Mathf.Cos(Mathf.PI * angle / 180);
        float newX = x * cos + y * sin;
        float newY = x * -sin + y * cos;
        return new Vector3(newX, v.y, newY);
    }
}
