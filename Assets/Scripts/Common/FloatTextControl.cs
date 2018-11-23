using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatTextControl : MonoBehaviour {

    public GameObject floatText;
    public Text text;
    public float showTime;
    public float showedTime;
    public bool showText = false;
    private Vector3 originPosition;

    // Use this for initialization
    void Start ()
    {
        EventManager.AllEvent.OnBossMsgEvent += OnMesShow;
        text.enabled = false;
        text.color = new Color(255, 255, 255, 1);
        originPosition = text.transform.localPosition;
        //floatText.SetActive(false);
    }
    private void OnDestroy()
    {
        EventManager.AllEvent.OnBossMsgEvent -= OnMesShow;
    }
    public void startToShow(float endTime, string newText)
    {
        text.enabled = true;
        floatText.SetActive(true);
        text.text = newText;
        showTime = endTime;
        showText = true;
    }


    // Update is called once per frame
    void Update () {
        if (showText)
        {
            text.transform.rotation = Camera.main.transform.rotation;
            showedTime += Time.deltaTime;
            text.transform.localPosition = text.transform.localPosition + new Vector3(0, Time.deltaTime, 0);
            if (showedTime >= showTime)
            {
                showedTime = 0;
                //floatText.SetActive(false);
                text.enabled = false;
                showText = false;
                text.transform.localPosition = originPosition;
            }
        }
    }
    Color color;
    public void OnMesShow(string newText,ElementAttributeType type)
    {
        if (type == ElementAttributeType.Fire)
        {
            color = Color.red;
        }else if (type == ElementAttributeType.Soil)
        {
            color = new Color(0, 0, 255);
        }else if (type == ElementAttributeType.Wood)
        {
            color = Color.green;
        }else
        {
            color = new Color(0, 255, 200);
        }
        text.color = color;
        startToShow(showTime, newText);
    }
}
