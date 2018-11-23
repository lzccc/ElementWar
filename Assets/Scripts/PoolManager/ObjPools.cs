using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
//README:
//-------------------------------------------------------------------------------
//2018.8.2:
//-ObjPools.cs定义了缓冲池类ObjPools，其中包括了非活动队列和激活队列，取出收回功能。
//-ObjPoolManager.cs定义了管理ObjPools字典，便于管理缓冲池和创建多个缓冲池。
//-两者共同构成了缓冲池管理器.


    
public class ObjPools:MonoBehaviour {

	private GameObject prefab;//存储在缓冲区的prefab
    public GameObject objClone;
    public string poolName="";
    //private Queue<GameObject> ActiveObjects = new Queue<GameObject>();//处于活动中的对象合集
    public Queue<GameObject> InactiveObjects;//非活动状态的游戏对象合集
    [SerializeField]
    public List<GameObject> list=new List<GameObject>();

    public void InitPools(GameObject obj, int initialCapacity)
	{
		prefab = obj;
        InactiveObjects = new Queue<GameObject>(initialCapacity);

		for (int i = 0; i < initialCapacity; i++)
		{
            objClone = GameObject.Instantiate(prefab, transform);
            objClone.name = prefab.name;
            objClone.SetActive(false);
			InactiveObjects.Enqueue(objClone);
            list.Add(objClone);

        }

	}


    //激活游戏对象，返回对象，不安排位置
    public GameObject Active()//******************************不知道同名有没有问题，先放在这里
    {
        GameObject obj = null;

        if (InactiveObjects.Count > 0)//队列中有对象储存
        {
            obj = InactiveObjects.Dequeue();
            list.Remove(obj);
        }
        else//如果没有对象，则添加新对象进入
        {
            obj = GameObject.Instantiate(prefab);
            obj.SetActive(false);
            obj.name = poolName;
        }
        //按类别进行初始化
        while (obj == null)
        {
            if (InactiveObjects.Count > 0)//队列中有对象储存
            {
                obj = InactiveObjects.Dequeue();
                list.Remove(obj);
            }
            else//如果没有对象，则添加新对象进入
            {
                obj = GameObject.Instantiate(prefab);
                obj.name = poolName;
            }
        }
        if (obj.CompareTag(CharacterType.Enemy.ToString()))
        {
            obj.GetComponent<Enemy>().ReStart();
        }else if (obj.CompareTag(CharacterType.Effect.ToString()))
        {
            obj.name = poolName;
            obj.GetComponent<EffectDestory>().Init();
        }
        obj.SetActive(true);
        return obj;
	}

	//收回游戏对象
	public void Deactive(GameObject obj)
    {
        list.Add(obj);
        obj.transform.SetParent(transform);
        InactiveObjects.Enqueue(obj);
        obj.transform.position = Vector3.zero;
        obj.SetActive(false);
    }

	//以下为额外功能
	//清空两个池
	public void ClearAllQueue()
	{
		//ActiveObjects.Clear();
		InactiveObjects.Clear();
	}

	//彻底删除所有活动池中的对象
	public void DestroyPool()
	{
		foreach (GameObject obj in InactiveObjects)
		{
			GameObject.Destroy(obj);
		}
		//foreach(GameObject obj in ActiveObjects)
		//{
		//	GameObject.Destroy(obj);
		//}
		//ActiveObjects.Clear();
		InactiveObjects.Clear();
	}
}
