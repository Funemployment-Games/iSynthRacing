using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
	public GUITexture m_trackNameSlot;
//	public GUITexture m_modeNameSlot;

	public GUIText m_tipsSlot;
	public GUIText m_tipsTitle;
	
	public Texture2D m_track1;
	public Texture2D m_track2;
	public Texture2D m_track3;
	
//	public Texture2D m_endlessMode;
//	public Texture2D m_grandPrixMode;
///	public Texture2D m_speedMode;
	
	public string[] m_tips;

	// Use this for initialization
	void Awake()
	{
	//	DontDestroyOnLoad(this);
	//	gameObject.SetActiveRecursively(false);
	}
	
	public void Enable()
	{
		print("enab load "+Time.time);
		gameObject.SetActiveRecursively(true);
		print(gameObject.active);
	/*	if(m_modeNameSlot)
		{
			m_modeNameSlot.enabled = true;
			
			if(GameMetrics.activeGameMode == GameMode.Endless)
			{
				m_modeNameSlot.texture = m_endlessMode;
			}
			else if(GameMetrics.activeGameMode == GameMode.GrandPrix)
			{
				m_modeNameSlot.texture = m_grandPrixMode;
			}
			else if(GameMetrics.activeGameMode == GameMode.Speed)
			{
				m_modeNameSlot.texture = m_speedMode;
			}
			else
			{
				m_modeNameSlot.enabled = false;
			}
		}
		*/
		if(m_trackNameSlot)
		{
			m_trackNameSlot.enabled = true;
			
			if(GameMetrics.selectedTrack == 1)
			{
				m_trackNameSlot.texture = m_track1;
			}
			else if(GameMetrics.selectedTrack == 2)
			{
				m_trackNameSlot.texture = m_track2;
			}
			else if(GameMetrics.selectedTrack == 3)
			{
				m_trackNameSlot.texture = m_track3;
			}
			else
			{
				m_trackNameSlot.enabled = false;
			}
		}
		
		if(GameMetrics.selectedTrack != 0)
		{
			m_tipsSlot.enabled = true;
			m_tipsSlot.text = m_tips[Random.Range(0, m_tips.Length)];
			
			m_tipsTitle.enabled = true;
		}
		else
		{
			m_tipsSlot.enabled = false;
			m_tipsTitle.enabled = false;
		}
	}
	
	public void Disable()
	{
		print("disab load "+Time.time);
		gameObject.SetActiveRecursively(false);
	}
}
