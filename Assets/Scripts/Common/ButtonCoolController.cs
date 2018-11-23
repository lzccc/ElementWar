using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonCoolController : MonoBehaviour {
    bool coolTrigger = false;
    public float attackCool = 0.2f;
    private Image mask;
    // Use this for initialization
    void Start()
    {
        mask.fillAmount = 0;
    }
    private void Awake()
    {
        mask = transform.Find("Mask").GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        if (coolTrigger)
        {
            mask.fillAmount -= Time.deltaTime / attackCool;
            if (mask.fillAmount <= 0)
            {
                this.GetComponent<Button>().enabled = true;
                coolTrigger = false;
            }
        }
    }
    /// <summary>
    /// 点击按钮后进入冷却
    /// </summary>
    public void OnClickGoods()
    {
        this.GetComponent<Button>().enabled = false;
        mask.fillAmount = 1;
        coolTrigger = true;
    }
}
