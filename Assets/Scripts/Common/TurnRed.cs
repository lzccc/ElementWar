using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRed : MonoBehaviour
{

    SkinnedMeshRenderer renderer1;
    float showTime = 0.5f;
    float showedTime = 0;
    private bool onHurt = false;
    Color originColor = Color.white;
    private Color thisColor = new Color(0.847f, 0.305f, 0.305f);
    bool isHurt = false;
    bool isShowing = false;
    [Header("毫秒单位，越大越快")]
    private float turnTime=30;
    // Use this for initialization
    void Start()
    {
        renderer1 = gameObject.GetComponent<SkinnedMeshRenderer>();
    }

    public void OnHurtColor()
    {
        if (isShowing == false)
        {
            isShowing = true;
            onHurt = true;
        }
    }

    void changeColor()
    {
        if (gameObject.GetComponent<SkinnedMeshRenderer>().material.color == thisColor)
        {
            gameObject.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
        }
        else
        {
            gameObject.GetComponent<SkinnedMeshRenderer>().material.color = thisColor;
        }
    }

    private void ChangePlayerColor(GameObject player)
    {
        Color playerColor = gameObject.GetComponent<SkinnedMeshRenderer>().material.color;
        if (playerColor == thisColor)
        {
            isHurt = true;
        }
        if (playerColor == originColor)
        {
            isHurt = false;
        }
        if (!isHurt)
        {
            player.GetComponent<SkinnedMeshRenderer>().material.color = Color.Lerp(playerColor, thisColor, turnTime * Time.deltaTime);
        }
        else
        {
            player.GetComponent<SkinnedMeshRenderer>().material.color = Color.Lerp(playerColor, originColor, turnTime * Time.deltaTime);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (onHurt)
        {
            showedTime += Time.deltaTime;
            ChangePlayerColor(gameObject);
            if (showedTime > showTime)
            {
                isShowing = false;
                onHurt = false;
                showedTime = 0;
                gameObject.GetComponent<SkinnedMeshRenderer>().material.color = originColor;
            }
        }
    }
}
