using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown("p"))
			OnMouseDown();
		
		if(Input.GetKeyUp("p"))
			OnMouseUp();
		
	}
	
	void OnMouseDown()
	{
		if(GameManager.gameState != GameState.Running)
		{
			//return;
		}
		GameManager.SetGameState(GameState.Paused);
		MenuManager.OpenOptionsMenu();
	}
	
	void OnMouseUp()
	{
	}
}
