using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
	float m_percentAlongSegment;
	
	void Start()
	{
		
	}
	
	public float percentAlongSegment{
		get{
			return m_percentAlongSegment;
		}
		set{
			m_percentAlongSegment = value;
		}
	}
	
	public void HandleCleared()
	{
		if(GameManager.gameState == GameState.Running)
			SoundManager.PlaySoundEvent(SoundEvent.ObstacleCleared, transform.position);
	}
}
