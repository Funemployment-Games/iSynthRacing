using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CreditsButton : Button
{
	void OnEnable()
	{
		if(GameManager.instance == null)
		{
			gameObject.SetActiveRecursively(true);
		}
		else
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
		
		if(GameManager.instance == null)
		{
			OptionsMenu.ShowCredits(true);
		}
	}
}
