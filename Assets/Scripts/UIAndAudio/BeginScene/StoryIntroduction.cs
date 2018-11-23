using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryIntroduction : PanelBase {

    //定义面板上的组件
    private Image background;
    private Button nextScene;

    public override void Init(params Object[] args)
    {
        base.Init(args);
        //初始化预设体路径和面板层级
        skinPath = "StoryIntroduction";
        layer = PanelLayer.Tips;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        Debug.Log(skin);
        nextScene = skin.GetComponent<Button>();
        nextScene.onClick.AddListener(NextScene);
    }

    /// <summary>
    /// 剧情介绍后，进入
    /// </summary>
    private void NextScene()
    {
        if (!PlayerPrefs.HasKey("first"))
        {
            PanelMgr.instance.ClosePanel("StoryIntroduction");
            //TODO 进入新手引导页面，还没实现新手引导

        }
        else {
            Global.loadName = "TestScene";
            SceneManager.LoadScene("Loading");
        }

    }
}
