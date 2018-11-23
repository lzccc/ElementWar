using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 消息盒子控制器
/// </summary>
public class ShowMsgController : MonoBehaviour {
    public static ShowMsgController msgController;
    public GameObject msgBox;
    Queue<GameObject> boxQueue = new Queue<GameObject>();
    [Header("最大消息盒子数")]
    public int maxBoxNum = 3;
    private float coolTimer=0.4f;//0.3s间隔

    [Header("消息间隔")]
    public float coolTime = 0.4f;
    private void Awake()
    {
        msgController = this;
    }
    private void Start()
    {
        EventManager.AllEvent.OnMesShowEvent += StartShow;
    }
    private void OnDestroy()
    {
        EventManager.AllEvent.OnMesShowEvent -= StartShow;
    }
    private void Update()
    {
        if (coolTimer > 0)
        {
            coolTimer -= Time.deltaTime;
            if (coolTimer <= 0)
                coolTimer = 0;
        }
    }
    /// <summary>
    /// 出现一个文本消息框,需要控制一下出现的间隔
    /// </summary>
    /// <param name="msg"></param>
    public void StartShow(string msg)
    {
        coolTimer+= coolTime;
        StartCoroutine(startShow(msg));
    }
    GameObject destroyBox;
    [Header("消息盒子开始的位置")]
    public Vector3 startPos=new Vector3(0,-300,0);
    IEnumerator startShow(string msg)
    {

        yield return new WaitForSeconds(coolTimer - coolTime);
        if (boxQueue.Count > maxBoxNum)//如果同时存在多个消息盒子则移除一个以前的
        {
            destroyBox=boxQueue.Dequeue();
            Destroy(destroyBox);
        }
        // = Instantiate(msgBox, transform);
        destroyBox = ObjPoolManager.objpoolmanager.GetPoolsForName(PoolType.MesBox.ToString()).Active();
        destroyBox.transform.SetParent(transform);
        destroyBox.transform.localPosition = startPos;
        destroyBox.GetComponent<MesBox>().StartAnim(msg);
        boxQueue.Enqueue(destroyBox);
    }
}
