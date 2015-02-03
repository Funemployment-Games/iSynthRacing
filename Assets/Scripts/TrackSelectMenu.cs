using UnityEngine;
using System.Collections;

public class TrackSelectMenu : Menu
{
	string m_levelToLoad;
	
	bool m_loadOnNextUpdate = false;
	
	public GUITexture m_modeNameSlot;
//	public Texture2D m_speedMode;
//	public Texture2D m_endlessMode;
//	public Texture2D m_grandPrixMode;
	
	protected virtual void Start()
	{
		base.Start();
		
		if(m_modeNameSlot)
		{
			if(GameMetrics.activeGameMode == GameMode.Endless)
			{
	//			m_modeNameSlot.texture = m_endlessMode;
			}
			else if(GameMetrics.activeGameMode == GameMode.GrandPrix)
			{
	//			m_modeNameSlot.texture = m_grandPrixMode;
			}
			else if(GameMetrics.activeGameMode == GameMode.Speed)
			{
	//			m_modeNameSlot.texture = m_speedMode;
			}
			else
			{
				m_modeNameSlot.enabled = false;
			}
		}
		
		if(Application.isEditor && MenuManager.SkipMenusAndGoStraightToRaceDebug)
		{
			OnTrackSelectGoButtonPressed();
		}
	}
	
	void OnTrackSelectGoButtonPressed()
	{
		MenuManager.EnableLoadingScreen();
		
		TrackSelectButtonManager m_manager = (TrackSelectButtonManager) FindObjectOfType(typeof(TrackSelectButtonManager));
		
		m_levelToLoad = "track_"+(m_manager.selectedTrackNumber);
		
		m_loadOnNextUpdate = true;
	}
	
	void OnTrackSelectBackButtonPressed()
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
