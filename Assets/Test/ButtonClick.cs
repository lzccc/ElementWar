using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour {

	public void OnFireSoilClick()
    {
        PlayerController.player.skillId = 10;
        PlayerController.player.firstSkillId = 1;
        PlayerController.player.secondSkillId = 2;
    }

    public void OnFireWoodClick()
    {
        PlayerController.player.skillId = 11;
        PlayerController.player.firstSkillId = 1;
        PlayerController.player.secondSkillId = 3;
    }
    public void OnFireWindClick()
    {
        PlayerController.player.skillId = 12;
        PlayerController.player.firstSkillId = 1;
        PlayerController.player.secondSkillId = 4;
    }
    public void OnSoilWoodClick()
    {
        PlayerController.player.skillId = 13;
        PlayerController.player.firstSkillId = 2;
        PlayerController.player.secondSkillId = 3;
    }
    public void OnSoilWindClick()
    {
        PlayerController.player.skillId = 14;
        PlayerController.player.firstSkillId = 2;
        PlayerController.player.secondSkillId = 4;
    }
    public void OnWoodWindClick()
    {
        PlayerController.player.skillId = 15;
        PlayerController.player.firstSkillId = 3;
        PlayerController.player.secondSkillId = 4;
    }
}
