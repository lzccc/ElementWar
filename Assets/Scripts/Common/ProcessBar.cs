using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 技能读条
/// </summary>
public class ProcessBar : MonoBehaviour {
    public GameObject processBar;
    private Image image;
    public Text text;
    public bool showBar = false;
    public bool useText = true;
    public float processTime;
    public float showedTime = 0;
	// Use this for initialization
	void Start () {
        text.enabled = false;
        processBar.SetActive(false);
        image = processBar.GetComponent<Image>();
        image.fillAmount = 0;
	}
    /// <summary>
    /// 调用会显示技能读条
    /// </summary>
    /// <param name="endTime"></param>
    /// <param name="newText"></param>
    public void startToShow(float endTime, string newText) {
        if (!BasePlayerAttribute.instance.SkillProcessShow)
        {
            return;
        }
        text.enabled = useText;
        processBar.SetActive(true);
        text.text = newText;
        processTime = endTime;
        showBar = true;
    }
    public void show() {
        startToShow(2, "test");
        image.fillAmount = 0;
    }

	// Update is called once per frame
	void Update () {
        if (showBar)
        {
            image.transform.rotation = Camera.main.transform.rotation;
            text.transform.rotation = Camera.main.transform.rotation;
            showedTime += Time.deltaTime;
            image.fillAmount = showedTime / processTime;
            if (image.fillAmount >= 1f) {
                showedTime = 0;
                processBar.SetActive(false);
                text.enabled = false;
                showBar = false;
                image.fillAmount = 0;
            }
        }
    }
}
