using UnityEngine;
using System.Collections;

public class SegmentManager : MonoBehaviour
{
	////// singleton
	private static SegmentManager m_instance;
	
	public static SegmentManager instance
	{
		get
		{
			return m_instance;
		}
	}
	
	void Awake()
	{
		if( m_instance != null )
		{
			return;
		}
		m_instance = this;
	}
	
	///////
	/// ///
	///
	
	public Track m_track;
	
	Segment[] m_segments;
	
	int m_currentSegmentIndex = 0;
	
	float m_currentPlayerSegmentPerc = 0f;
	
	Vector3 m_currentPlayerPos;
	
	float m_segmentStartTime = 0f;
	
	Obstacle m_nextObstacle;
	
	Obstacle m_prevObstacle;
	
	float m_lastObstacleClearedPercent;
	
	public static void Init()
	{
		instance.InstantiateSegments();
		
		foreach (Segment segment in instance.m_segments)
		{
			segment.InitObstacles();
		}
		
		foreach (Segment m_segment in instance.m_segments)
		{
			m_segment.gameObject.active = false;
		}
		
		instance.m_currentSegmentIndex = 0;
		
		instance.m_currentPlayerSegmentPerc = 0f;
		
		instance.m_segmentStartTime = 0f;
		
		instance.m_nextObstacle = instance.m_segments[0].GetNextObstacle(0f);
	}
	
	/*
	void HandleSegmentActivation()
	{
		if(m_currentSegmentIndex == 0)
		{
			m_segments[m_segments.Length-1].gameObject.active = false;
		}
		else
		{
			m_segments[m_currentSegmentIndex-1].gameObject.active = false;
		}
		
		m_segments[m_currentSegmentIndex].gameObject.active = true;
		m_segments[m_currentSegmentIndex+1].gameObject.active = true;
	}*/
	
	void InstantiateSegments()
	{
		if(!m_track)
		{
			m_track = (Track)FindObjectOfType(typeof(Track));
		}
		
		m_track.Init();
		m_segments = (Segment[])m_track.segments;
	}
	
	
	public Vector3 GetNextPosition(float m_speed)
	{
		if(instance.m_segments == null)
			return m_currentPlayerPos;
		
		m_currentPlayerSegmentPerc += Time.deltaTime * m_speed/*/m_segments[m_currentSegmentIndex].length*/;

		if(m_currentPlayerSegmentPerc >= 100f)
		{
			ChangeSegment();
		}

		m_currentPlayerPos = m_segments[m_currentSegmentIndex].GetPathPositionAt(m_currentPlayerSegmentPerc);
		
		if(m_nextObstacle != null)
		{
			if(m_currentPlayerSegmentPerc > m_nextObstacle.percentAlongSegment)
			{
				HandleObstacleCleared();
				
				m_nextObstacle = m_segments[m_currentSegmentIndex].GetNextObstacle(m_currentPlayerSegmentPerc);
			}
		}
		
		return m_currentPlayerPos;
	}
	
	
	void ChangeSegment()
	{
		float m_segmentTime = Time.time - m_segmentStartTime;
		
		m_segmentStartTime = Time.time;
		
		m_currentPlayerSegmentPerc = 0f;
		
		if(++m_currentSegmentIndex >= m_track.segmentCount)
		{
			GameManager.OnLap();
			
			m_currentSegmentIndex = 0;
		}
		
		m_nextObstacle = m_segments[m_currentSegmentIndex].GetNextObstacle(0f);
	}
	
	void HandleObstacleCleared()
	{
		m_nextObstacle.HandleCleared();
		// don't count cleared obstacles that are too close together
		if(m_nextObstacle.percentAlongSegment - m_lastObstacleClearedPercent < 0.3f)
		{
			m_lastObstacleClearedPercent = m_currentPlayerSegmentPerc;
			return;
		}
		
		
		
		m_lastObstacleClearedPercent = m_currentPlayerSegmentPerc;
		ComboSystem.instance.ObstacleCleared();
	}
	
	public static Track activeTrack{
		get{
		//	if(instance.m_track == null)
			{
		//		instance.m_track = (Track) GameObject.FindObjectOfType(typeof(Track));
			}
			return instance.m_track;
		}
	}
}
