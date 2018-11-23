using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummonMonsterTerrain : MonoBehaviour {

    [Header("召唤的怪物数组")]
    public GameObject[] summonEnemy;
    [Header("召唤的怪物数组对应的数量")]
    public int[] summonEnemyNum;
    [Header("每种怪的数量上限")]
    public int[] maxAddNum;
    [Header("Boss")]
    public GameObject boss;
    [Header("下一次增加的数量,每种怪")]
    public int nextAddNum;
    [Header("怪物相对于召唤点的半径距离")]
    public float dis;
    [Header("每只怪物之间的角度偏移")]
    public float angle;
    Vector3 forward;
    //private Transform characterManager;
    private void Awake()
    {
        forward = transform.forward.normalized;
    }
    GameObject go;

    public void StartSummon1(Transform characterManager)
    {
        //this.characterManager = characterManager;
        for (int i = 0, num = 0; i < summonEnemy.Length; i++)
        {
            for (int j = 0; j < summonEnemyNum[i]; j++)
            {
                go = ObjPoolManager.objpoolmanager.GetPoolsForName(summonEnemy[i].name).Active();
                go.GetComponent<NavMeshAgent>().Warp(RotationMatrix(forward, angle * num) + transform.position);
                go.transform.SetParent(characterManager);
                num++;
            }
        }
        AddAllNum();
    }

    public void StartSummon2(Transform characterManager)
    {
        int randomEnemy = Random.Range(0, summonEnemy.Length);
        //this.characterManager = characterManager;

        for (int j = 0; j < summonEnemyNum[randomEnemy]; j++)
        {
            //go = ObjPoolManager.objpoolmanager.GetPoolsForName(summonEnemy[randomEnemy].name).Active();
            go = Instantiate(summonEnemy[randomEnemy], characterManager);
            go.GetComponent<NavMeshAgent>().Warp(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)) + transform.position);
            go.name = summonEnemy[randomEnemy].name;
            Debug.Log(go.transform.position);
            //go.transform.position= new Vector3(Random.Range(1, 5), Random.Range(1, 10), Random.Range(1, 5)) + transform.position;
            //go.transform.SetParent(characterManager);
        }
        //AddAllNum();
    }

    public void StartBoss(Transform characterManager)
    {
        if (boss != null)
        {
            go = Instantiate(boss, characterManager);
            go.GetComponent<NavMeshAgent>().Warp(transform.position);
        }
    }
    /// <summary>
    /// 增加下一次的数量
    /// </summary>
    public void AddAllNum()
    {
        for (int i = 0; i < summonEnemyNum.Length; i++)
        {
            if (summonEnemyNum[i] >= maxAddNum[i]) break;
            summonEnemyNum[i] += nextAddNum;
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
