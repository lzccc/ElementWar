using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinish : MonoBehaviour {

	public void OnGameFinish()
    {
        AudioManager.Instance.PlaySound(AudioType.ButtonNormal);
        AudioManager.Instance.ChangeMusic(AudioType.BeginGame);
        SceneManager.LoadScene("BeginGame");
    }
}
