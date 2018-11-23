using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingChange : MonoBehaviour {

    float fadeFogeTime = 4;
    float passedFadeTime = 0;
    public bool isGameSuccess = false;
    float originDistance;
    // Use this for initialization
    void Start () {
        originDistance = RenderSettings.fogEndDistance;
        EventManager.AllEvent.OnGameFinishEvent += GameSuccess;
    }

    private void OnDestroy()
    {
        EventManager.AllEvent.OnGameFinishEvent -= GameSuccess;
    }
    public void GameSuccess(GameFinishType type)
    {
        if (type != GameFinishType.Win) return;
        isGameSuccess = true;
    }
    public void hideFog() {
        RenderSettings.fogEndDistance = Mathf.Lerp(originDistance, 350, passedFadeTime / fadeFogeTime);
    }
	
	// Update is called once per frame
	void Update () {
        if (isGameSuccess) {
            passedFadeTime += Time.deltaTime;
            hideFog();
        }
	}
}
