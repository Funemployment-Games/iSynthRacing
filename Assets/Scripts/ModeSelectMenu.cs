using UnityEngine;
using System.Collections;

public class ModeSelectMenu : Menu
{
	string m_levelToLoad;
	bool m_loadOnNextUpdate = false;
	
	void OnModeSelectGoButtonPressed()
	{
		MenuManager.EnableLoadingScreen();
		
		m_levelToLoad = "TrackSelectMenu"; // track select
		
		m_loadOnNextUpdate = true;
	}
	
	void OnModeSelectBackButtonPressed()
	{
		MenuManager.LoadMainMenu(true);
	}
	
	void Update()
	{
		if(m_loadOnNextUpdate)
		{
			m_loadOnNextUpdate = false;
			Application.LoadLevel(m_levelToLoad);
		}
	}
	
}
