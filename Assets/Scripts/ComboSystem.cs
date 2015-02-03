using UnityEngine;
using System.Collections;

public class ComboSystem
{
	////// singleton
	private static ComboSystem m_instance;
	
	public static ComboSystem instance
	{
		get
		{
			if( m_instance == null )
				m_instance = new ComboSystem();
			
			if(m_instance == null)
			{
				Debug.LogError("Can't find singleton class");
			}
			return m_instance;
		}
	}
	
	////////////////////////////////////
	///
	
	public static void Init()
	{
		instance.m_currentComboCount = 0;
	}
	
	int m_currentComboCount = 0;
	
	float m_lastObstacleHitTime = 0f;
	
	public void ObstacleCleared()
	{
		// check that some time has passed, so we don't count as cleared an obstacle we just hit
		if(Time.time - m_lastObstacleHitTime > 0.1f)
		{
			m_currentComboCount++;
			GameMetrics.UpdateCombo(m_currentComboCount);
		}
		GameMetrics.UpdateObstacle(false);
	}
	
	public void ObstacleHit()
	{
		m_lastObstacleHitTime = Time.time;
		
		m_currentComboCount = 0;
		
		GameMetrics.UpdateObstacle(true);
	}
	
	
	
	
	//// Getters & setters
	///
	
	public int currentComboCount{
		get{
			return m_currentComboCount;
		}
	}
}
