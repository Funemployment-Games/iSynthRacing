using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DoneButton : Button
{
	protected virtual void OnMouseUp()
	{
		base.OnMouseUp();
		
		MenuManager.CloseOptionsMenu();
		
		if(GameManager.instance != null)
		{
			GameManager.SetGameState(GameState.Countdown);
		}
	}
}
