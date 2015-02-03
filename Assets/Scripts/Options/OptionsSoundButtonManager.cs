using UnityEngine;
using System.Collections;

public class OptionsSoundButtonManager : SelectButtonManager
{

	// Use this for initialization
	void Start ()
	{
		base.Start();
		
		int sel = OptionsMenu.soundOn ? 1 : 2;
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
		
		OptionsMenu.SetSound((OnOffSetting) number);
	}
	
	public int selectedTrackNumber{
		get{
			return m_selectedButtonIndex;
		}
	}
}
