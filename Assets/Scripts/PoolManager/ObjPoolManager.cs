using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolManager : MonoBehaviour {

	public static ObjPoolManager objpoolmanager;
    [Header("放入顺序要遵守枚举中的顺序")]
    public GameObject[] poolOb;
    [Header("对应序号的缓冲池数量")]
    public int[] poolObNum;
	public Dictionary<string, ObjPools> PoolsDic = new Dictionary<string, ObjPools>();

	//加入池

	public void Awake()
	{
        objpoolmanager = this;
        //DontDestroyOnLoad(gameObject);
        InitPool();
    }

    public void InitPool()
    {
        GameObject go; ObjPools pool;
        for (int i = 0; i < poolOb.Length; i++)
        {
            go = new GameObject(poolOb[i].name);
            go.transform.SetParent(transform);
            pool = go.AddComponent<ObjPools>();
            pool.InitPools(poolOb[i], poolObNum[i]);
            pool.poolName = go.name;
            add(go.name, pool);
        }
    }

	public void add(string str,ObjPools pool)
	{
		if (PoolsDic.ContainsKey(str)) return;

		PoolsDic.Add(str, pool);
	}

	//删除不用的池
	public void DestroyUnusedPool()
	{
		foreach(KeyValuePair<string,ObjPools> KeyValue in PoolsDic)
		{
			KeyValue.Value.DestroyPool();
		}
	}

	//清空池字典
	void ClearPoolsDic()
	{
		PoolsDic.Clear();
	}

    public ObjPools GetPoolsForName(string poolName)
    {
        ObjPools pool;
        PoolsDic.TryGetValue(poolName,out pool);
        return pool;
    }
}
