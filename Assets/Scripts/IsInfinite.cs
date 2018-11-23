using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 设置是无尽模式
/// </summary>
public class IsInfinite : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        BasePlayerAttribute.instance.inInfinite = true;
	}
	
}
