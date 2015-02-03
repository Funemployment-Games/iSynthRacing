using UnityEngine;
using System.Collections;

public class SpeedModeResultsScreen : MonoBehaviour
{
	public GUIText m_topSpeed;
	public GUIText m_distance;
	public GUIText m_fastestLap;
	public GUITexture m_speedBar;
	public GUIText m_trackName;
	
	public GUIText m_topSpeedRecord;
	public GUIText m_distanceRecord;
	public GUIText m_fastestLapRecord;

	float m_currentTopSpeed;
	int m_targetTopSpeed;
	float m_speedBarSpeed = 30f;
	
	bool m_active = false;
	
	public void Init()
	{
		GameMetrics.LoadAchievements();
		
		int runs = GameMetrics.GetTotalRuns();
		string num = runs.ToString();
		if(runs < 100)
		{
			num = "0"+num;
			if(runs < 10)
				num = "0"+num;
		}
		
		m_trackName.text = "Track "+GameMetrics.selectedTrack+" -- Run "+num;
		
		////////
		//records
		
		m_topSpeedRecord.text = ((int)GameMetrics.GetRecordSpeed()).ToString();
		m_topSpeedRecord.text += " kph";
		
		m_fastestLapRecord.text = NumberFormat.FloatToString(GameMetrics.GetRecordLap(),2);
		m_fastestLapRecord.text += " s";
		
		m_distanceRecord.text = NumberFormat.FloatToString(GameMetrics.GetRecordDistance(),2);
		m_distanceRecord.text += " km";
		
		////////
		///
		///
		
		m_currentTopSpeed = 100;
		m_targetTopSpeed = ((int) GameMetrics.maxSpeed);
		//	m_targetTopSpeed = 200;
		m_topSpeed.text = "0 kph";
		
		if(GameMetrics.bestLap < 1000)
		{
			m_fastestLap.text = NumberFormat.FloatToString(GameMetrics.bestLap,2);
			m_fastestLap.text += " s";
		}
		else
		{
			m_fastestLap.text = "n/a";
		}
		
		m_distance.text = NumberFormat.FloatToString(GameMetrics.totalDistance,2);
		m_distance.text += " km";
		
		if(GameMetrics.bestLap < GameMetrics.GetRecordLap())
		{
			m_fastestLapRecord.text = m_fastestLap.text;
			m_fastestLapRecord.material.color = Color.green;
			m_fastestLap.material.color = Color.green;
		}
		if(GameMetrics.totalDistance > GameMetrics.GetRecordDistance())
		{
			m_distanceRecord.text = m_distance.text;
			m_distanceRecord.material.color = Color.green;
			m_distance.material.color = Color.green;
		}
		
		GameMetrics.SetSpeedResults();
		
		m_active = false;
		
	//	gameObject.SetActiveRecursively(false);
	}

	// Use this for initialization
	void Start ()
	{
		gameObject.SetActiveRecursively(false);
	}
	
	public void Deactivate()
	{
		gameObject.SetActiveRecursively(false);
		m_active = false;
	}
	
	public void Activate()
	{
		gameObject.SetActiveRecursively(true);
		Init();
		m_active = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!m_active) return;
		
		if((int)m_currentTopSpeed < (int)m_targetTopSpeed)
		{
			m_currentTopSpeed += m_speedBarSpeed * Time.deltaTime;
			
			m_topSpeed.text = ((int)m_currentTopSpeed).ToString();
			m_topSpeed.text += " kph";
			
			float barWidth=0;
			if(m_currentTopSpeed > 100)
			{
				barWidth = (m_currentTopSpeed-100)*390/300;
			}
			
			if(m_currentTopSpeed > GameMetrics.GetRecordSpeed())
			{
				m_topSpeedRecord.text = m_topSpeed.text;
				m_topSpeedRecord.material.color = Color.green;
				m_topSpeed.material.color = Color.green;
			}
			
			Rect pixelInset = m_speedBar.pixelInset;
			pixelInset.width = barWidth;
			m_speedBar.pixelInset = pixelInset;
		}
	}
}
