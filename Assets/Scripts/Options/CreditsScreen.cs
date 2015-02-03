using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CreditsScreen : Button
{
	protected virtual void Start()
	{
		base.Start();
	}
	
	protected virtual void OnMouseUp()
	{
		base.OnMouseUp();
		
		OptionsMenu.ShowCredits(false);
	}
}
