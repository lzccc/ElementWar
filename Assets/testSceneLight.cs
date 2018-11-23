using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testSceneLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(wait());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadSceneAsync("TestScene");
    }
}
