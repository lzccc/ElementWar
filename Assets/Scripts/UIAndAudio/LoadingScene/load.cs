using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load : MonoBehaviour {

    private AsyncOperation async;//异步加载操作
    private GameObject panelMgr;

    public float time = 3f;

    // Use this for initialization
    void Start()
    {
        Debug.Log("进入加载界面啦！！！");

        panelMgr = Resources.Load<GameObject>("WXY/Prefabs/PanelMgr");
        Instantiate(panelMgr);
        PanelMgr.instance.OpenPanel<LoadingPanel>("");

        //异步加载
        StartCoroutine(LoadResources());
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadResources()
    {
        async = SceneManager.LoadSceneAsync(Global.loadName);
        async.allowSceneActivation = false;
        yield return async;      
    }

    // Update is called once per frame
    void Update()
    {
        LoadingPanel.loading.value += Time.deltaTime / time;
        if (LoadingPanel.loading.value >= 1)
        {
            Debug.Log("进度满啦" );
            LoadingPanel.loading.value = 1;
            async.allowSceneActivation = true;
        }
    }

}
