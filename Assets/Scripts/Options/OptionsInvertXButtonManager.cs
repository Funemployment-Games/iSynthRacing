using UnityEngine;
using System.Collections;

public class OptionsInvertXButtonManager : SelectButtonManager
{

	// Use this for initialization
	void Start ()
	{
		base.Start();
		
		int sel = OptionsMenu.invertX ? 1 : 2;
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
		
		OptionsMenu.SetInvertX((OnOffSetting) number);
	}
	
	public int selectedTrackNumber{
		get{
			return m_selectedButtonIndex;
		}
	}
}
