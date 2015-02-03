using UnityEngine;
using System.Collections;

public class ResultsRetryButton : Button
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
		GameManager.Reset();
	}
}
