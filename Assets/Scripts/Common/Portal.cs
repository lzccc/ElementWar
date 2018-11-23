using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(BasePlayerAttribute.instance.nowScene);
        if (other.CompareTag(CharacterType.Player.ToString()))
        {
            BasePlayerAttribute.instance.nowScene++;
            if (BasePlayerAttribute.instance.nowScene > 3)
                BasePlayerAttribute.instance.nowScene = 3;
            Global.loadName = "Scene_0" + BasePlayerAttribute.instance.nowScene;
            SaveAndLoad.SaveGameData(BaseCharacter.player);
            SaveAndLoad.saveCurrentLevel(BasePlayerAttribute.instance.nowScene);
            SceneManager.LoadScene("Loading");
        }
    }
}
