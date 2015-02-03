using UnityEngine;
using System.Collections;

public class OptionsSensitivityButtonManager : SelectButtonManager
{

	// Use this for initialization
	void Start ()
	{
		base.Start();
		
		SelectButton(OptionsMenu.sensitivity);
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update();
	}
	
	public override void SelectButton(int number)
	{
		base.SelectButton(number);
		
		OptionsMenu.SetSensitivity((SensitivitySetting) number);
	}
	
	public int selectedTrackNumber{
		get{
			return m_selectedButtonIndex;
		}
	}
}
