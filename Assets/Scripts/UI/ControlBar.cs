//this script is just used for the demo...nothing to see here move along.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ControlBar : MonoBehaviour {

	//a list of all the UIScript_HP
	public List<UIBarScript> HPScriptList = new List<UIBarScript>();
    [SerializeField] private GameObject target;
	// Update is called once per frame
	void Update() 
	{
		//for every UIScript_HP update it.
		foreach (UIBarScript HPS in HPScriptList)
		{
			HPS.UpdateValue(Status.HP, Status.MAX_HP);
		}
	}
}
