using UnityEngine;
using System.Collections;

public class OptionsMusicButtonManager : SelectButtonManager
{

	// Use this for initialization
	void Start ()
	{
		base.Start();

		int sel = OptionsMenu.musicOn ? 1 : 2;
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
		
		OptionsMenu.SetMusic((OnOffSetting) number);
	}
	
	public int selectedTrackNumber{
		get{
			return m_selectedButtonIndex;
		}
	}
}
