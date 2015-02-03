using UnityEngine;
using System.Collections;

public class SpeedModeGUI : MonoBehaviour
{
	Player m_player;
	
	public BoostBar m_retroBoostBar;
	
	public GUIText m_lapTime;
	
	public GUIText m_speed;
	
	public GUIText m_timeLeft;
	
	public GUIText m_speedLevel;
	
	public GUIText m_framesPerSecond;
	
	float m_currentLapTime;
	
	int m_prevSpeedLevel;
	
	float m_speedLevelBackToNormalTime;

	// Use this for initialization
	void Start ()
	{
		if(!GameManager.instance) return;
		
		m_player = (Player) FindObjectOfType(typeof(Player));
		
		if(GameMetrics.selectedTrack == 4)
		{
			m_timeLeft.enabled = false;
		}
		
		m_prevSpeedLevel = GameManager.currentSpeedLevel;
		
		m_framesPerSecond.gameObject.SetActiveRecursively(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!GameManager.instance) return;
		
		if(GameManager.gameState == GameState.Results)
		{
			return;
		}
		
	//	m_distanceText.text = "dist: "+GameMetrics.totalDistance;
		
		m_speed.text = m_player.currentSpeed.ToString()+"km/h";
		if(m_player.currentSpeed > 100 && m_player.currentSpeed >= GameMetrics.maxSpeed)
		{
			m_speed.material.color = Color.green;
		}
		else
		{
			m_speed.material.color = Color.white;
		}
		
		if(m_timeLeft.enabled)
		{
			m_timeLeft.text = "time: "+GameManager.timeLeft;
			if(GameManager.timeLeft < 10)
			{
				m_timeLeft.material.color = Color.red;
			}
			else
			{
				m_timeLeft.material.color = Color.white;
			}
		}
		
		m_speedLevel.text = GameManager.currentSpeedLevel.ToString()+" :speed level";
		
		if(GameManager.currentSpeedLevel != m_prevSpeedLevel)
		{
			if(GameManager.currentSpeedLevel > m_prevSpeedLevel)
				m_speedLevel.material.color = Color.green;
			else
				m_speedLevel.material.color = Color.red;
			
			m_speedLevelBackToNormalTime = Time.time + 2f;
		}
		m_prevSpeedLevel = GameManager.currentSpeedLevel;
		
		if(Time.time > m_speedLevelBackToNormalTime && m_speedLevelBackToNormalTime > 0)
		{
			SwitchSpeedLeveLBackToNormal();
		}
		
		if(m_retroBoostBar)
		{
			m_retroBoostBar.SetBoostBarPercent(m_player.retroBoostComponent.normalizedReserve);
			
			m_retroBoostBar.SetInUse(m_player.retroBoostComponent.isActive);
		}
		
		if(GameManager.lapNumber > 0 && GameManager.lapTime < 5f)
		{
			//m_currentLapTime = GameManager.lapTime;
			if(m_currentLapTime < GameManager.bestLapTime)
			{
				m_lapTime.material.color = Color.green;
				m_lapTime.text = "best lap! "+NumberFormat.FloatToString(m_currentLapTime,2);
			}
			else
			{
				m_lapTime.text = "last lap: "+NumberFormat.FloatToString(m_currentLapTime,2);
			}
		}
		else
		{
			m_currentLapTime = GameManager.lapTime;
			m_lapTime.material.color = Color.white;
			m_lapTime.text = "lap: "+NumberFormat.FloatToString(m_currentLapTime,2);
		}
	}
	
	void SwitchSpeedLeveLBackToNormal()
	{
		m_speedLevel.material.color = Color.white;
	}
}
