using UnityEngine;
using System.Collections;

public class ModeSelectButtonManager : SelectButtonManager
{
	public GUITexture m_modeDetails;
	
	GameMode m_selectedMode;
	
	// Use this for initialization
	void Start ()
	{
		base.Start();
		SelectButton(2);
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update();
	}
	
	public override void SelectButton(int number)
	{
		base.SelectButton(number);
		
		switch (base.m_selectedButtonIndex)
		{
			case 1:
				m_selectedMode = GameMode.Speed;
				break;
			case 2:
				m_selectedMode = GameMode.GrandPrix;
				break;
			case 3:
				m_selectedMode = GameMode.Endless;
				break;
			default:
				break;
		}
		
		GameMetrics.activeGameMode = m_selectedMode;

		if(m_selectedButton.trackDetailsTexture && m_modeDetails)
			m_modeDetails.texture = m_selectedButton.trackDetailsTexture;
	}
	
	public GameMode selectedTrackNumber{
		get{
			return m_selectedMode;
		}
	}
}
