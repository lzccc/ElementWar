using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeLight : MonoBehaviour {
    public Light directionalLight;
    bool fade = false;
    public float showNum = 1;
    float showedNum = 0;
    public void UpGrade(int i) {
        showNum = i;
        fade = true;
    }

	// Use this for initialization
	void Start () {
		
	}
	
    public void test()
    {
        UpGrade(1);
    }

	// Update is called once per frame
	void Update () {
        if (fade) {
            showedNum += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(1, 0, showedNum / showNum);
            if (directionalLight.intensity == 0) {
                directionalLight.intensity = 1;
                showedNum = 0;
                fade = false;
            }
        }
	}
}
