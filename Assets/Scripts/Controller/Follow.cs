using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public Vector3 offset;
    public Transform player;
    public float speed = 10;
    private Vector3 target;
    public bool isGameSuccess = false;
    public Transform terrain;
    public Vector3 SuccessOffset;
    public GameObject gameFinishPage;
    Quaternion targetRotation;
    void Start()
    {
        offset = transform.position - player.position;
        targetRotation = Quaternion.Euler(5, -2, 0);
        EventManager.AllEvent.OnGameFinishEvent += GameSuccess;
    }
    private void OnDestroy()
    {
        EventManager.AllEvent.OnGameFinishEvent -= GameSuccess;
    }
    public void GameSuccess(GameFinishType type)
    {
        if (type != GameFinishType.Win) return;
        PanelMgr.instance.ClosePanel("BattlePanel");
        GameObject.Find("JoystickManager").SetActive(false);
        isGameSuccess = true;
        StartCoroutine(gameEnd());
    }
    IEnumerator gameEnd()
    {
        yield return new WaitForSeconds(3);
        gameFinishPage.SetActive(true);
    }
    void LookDown()
    {
        target = terrain.position + SuccessOffset;
        transform.position = Vector3.Lerp(transform.position, target, 0.5f * Time.deltaTime);
    }

    void RotateCamera()
    {
        transform.Rotate(new Vector3(4, -3, 0), Time.deltaTime * 0.5f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 0.5f);
        //transform.localEulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(5, transform.localEulerAngles.y, 0), 0.5f * Time.deltaTime);
    }



    void Update()
    {
        if (!isGameSuccess)
        {
            target = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            LookDown();
            RotateCamera();
        }
    }

    public void ShowEndPage()
    {
        gameFinishPage.SetActive(true);
    }
}
