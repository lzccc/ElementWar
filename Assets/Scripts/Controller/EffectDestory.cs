using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestory : MonoBehaviour {
    [Header("特效存活时间")]
    public float lifeTime=0.3f;
    private float lifeTimer;
    // Use this for initialization
    [Header("是否不销毁回到缓冲池")]
    public bool isPool=false;
	void Start () {
        lifeTimer = lifeTime;

    }
    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            if (isPool)
            { 
                ObjPoolManager.objpoolmanager.GetPoolsForName(gameObject.name).Deactive(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void Init()
    {
        lifeTimer = lifeTime;
    }
}
