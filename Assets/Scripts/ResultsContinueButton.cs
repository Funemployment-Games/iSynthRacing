using UnityEngine;
using System.Collections;

public class ResultsContinueButton : Button
{
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	void OnMouseUp()
	{
		base.OnMouseUp();
		SoundManager.PauseAll();
		MenuManager.LoadMainMenu(true);
	}
}
