using UnityEngine;
using System.Collections;

public class OptionsInvertYButtonManager : SelectButtonManager
{

	// Use this for initialization
	void Start ()
	{
		base.Start();
		
		int sel = OptionsMenu.invertY ? 1 : 2;
		SelectButton(sel);
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update();
	}
	
	public override void SelectButton(int number)
	{
		base.SelectButton(number);
		
		OptionsMenu.SetInvertY((OnOffSetting) number);
	}
	
	public int selectedTrackNumber{
		get{
			return m_selectedButtonIndex;
		}
	}
}
