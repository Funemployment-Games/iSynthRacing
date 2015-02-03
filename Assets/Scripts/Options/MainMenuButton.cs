using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MainMenuButton : Button
{
	void OnEnable()
	{
		if(GameManager.instance == null)
		{
			gameObject.SetActiveRecursively(false);
		}
	}
	
	protected virtual void Start()
	{
		base.Start();
		
	}
	
	protected virtual void OnMouseUp()
	{
		base.OnMouseUp();
		
		MenuManager.CloseOptionsMenu();
		
		if(GameManager.instance != null)
		{
			MenuManager.LoadMainMenu(true);
		}
	}
}
