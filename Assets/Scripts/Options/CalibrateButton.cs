using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CalibrateButton : Button
{
	protected virtual void OnMouseDown()
	{
		base.OnMouseDown();
		
		OptionsMenu.CalibrateNow();
	}
	
	protected virtual void Update()
	{
		base.Update();
		
		if(pressed)
		{
			OptionsMenu.CalibrateNow();
		}
	}
}
